using System;
using Akka.Actor;
using Akka.Persistence;
using LanguageExt;
using Biblio.Common.Extensions;
using Biblio.Common.Messages;

namespace Biblio.Common.Akka;

/// <summary>
/// Base actor that provides a common pattern for processing commands, persisting produced events
/// and applying them to an immutable state object.
/// </summary>
/// <typeparam name="TState">The state type which implements <see cref="IPersistableState{TState,TEnv,TCmd,TE,TR}"/>.</typeparam>
/// <typeparam name="TEnvironment">The environment type required to run effectful computations (injected via <see cref="GetEnvironment"/>).</typeparam>
/// <typeparam name="TCommand">The command type handled by this actor; must implement <see cref="IFallibleCommand{TR}"/>.</typeparam>
/// <typeparam name="TEvent">The event type produced by handling commands.</typeparam>
/// <typeparam name="TResponse">The command result type returned on successful command processing.</typeparam>
public abstract class PersistableActor<TState, TEnvironment, TCommand, TEvent, TResponse> : ReceivePersistentActor
    where TState : IPersistableState<TState, TEnvironment, TCommand, TEvent, TResponse>
    where TCommand : IFallibleCommand<TResponse>
{
    /// <summary>
    /// Initializes a new instance of <see cref="PersistableActor{TState,TEnv,TCmd,TE,TR}"/>
    /// and configures recovery and command handling for the specific event and command types.
    /// </summary>
    protected PersistableActor()
    {
        Recover<TEvent>(ApplyEvent);
        Command<TCommand>(HandleCommand);
    }

    /// <summary>
    /// The current immutable state of the actor. Implementations should rely on for initial value.
    /// </summary>
    protected TState State { get; set; } = TState.Initial;

    /// <summary>
    /// Apply a persisted event to the current state.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    private void ApplyEvent(TEvent @event) => State = State.ApplyEvent(@event);

    /// <summary>
    /// Handles incoming commands of type <typeparamref name="TCommand"/>, runs the domain processing
    /// in the environment provided by <see cref="GetEnvironment"/>, persists resulting events
    /// and replies with a <see cref="Fin{TR}"/> indicating success or failure.
    /// </summary>
    /// <param name="command">The incoming command to handle.</param>
    private void HandleCommand(TCommand command)
    {
        var flow =
            from env in GetEnvironment()
            from res in TState.ProcessCommand(State, command).RunIO(env)
            select res;

        var fin = flow.RunSafe();
        if (fin.IsSuccess(out var t, out var err))
        {
            // Persist generated events and invoke the post-persist callback for each.
            PersistAll(t.Events, AfterEventPersisted);
            DeferAsync(Sender, sender => sender.Tell(Fin<TResponse>.Succ(t.Result)));
        }
        else
        {
            // Immediately return the failure to the sender.
            Sender.Tell(Fin<TResponse>.Fail(err));
        }
    }

    /// <summary>
    /// Resolve or create any environment/context required to run effectful domain computations.
    /// Implementations should provide environment acquisition as an <see cref="IO{T}"/>.
    /// </summary>
    /// <returns>An <see cref="IO{TEnv}"/> that yields the environment when executed.</returns>
    protected abstract IO<TEnvironment> GetEnvironment();

    /// <summary>
    /// Helper to register a query handler for a given query type. The provided handler receives the
    /// query and returns an <see cref="Eff{TEnv,TRep}"/> which will be executed in the actor's environment.
    /// The result (<see cref="Fin{TRep}"/>) is sent back to the requester.
    /// </summary>
    /// <typeparam name="TQuery">Query message type.</typeparam>
    /// <typeparam name="TReply">Query response type.</typeparam>
    /// <param name="queryHandler">Function that turns the query into an effectful computation.</param>
    protected void Query<TQuery, TReply>(Func<TQuery, Eff<TEnvironment, TReply>> queryHandler)
        where TQuery : IFallibleQuery<TReply>
    {
        Command<TQuery>(qry =>
        {
            var flow =
                from env in GetEnvironment()
                from res in queryHandler(qry).RunIO(env)
                select res;
            var fin = flow.RunSafe();
            Sender.Tell(fin);
        });
    }

    /// <summary>
    /// Called after each event has been persisted. Default behaviour applies the event to the state.
    /// Override to provide side-effects (e.g. publishing notifications) while keeping state application.
    /// </summary>
    /// <param name="event">Persisted event.</param>
    protected virtual void AfterEventPersisted(TEvent @event)
    {
        ApplyEvent(@event);
    }
}

/// <summary>
/// Extension of <see cref="PersistableActor{TState,TEnv,TCmd,TE,TR}"/> that supports snapshotting.
/// </summary>
/// <typeparam name="TState">State type supporting snapshot conversion.</typeparam>
/// <typeparam name="TEnvironment">Environment type.</typeparam>
/// <typeparam name="TCommand">Command type.</typeparam>
/// <typeparam name="TEvent">Event type.</typeparam>
/// <typeparam name="TSnapshot">Snapshot representation type.</typeparam>
/// <typeparam name="TResponse">Command result type.</typeparam>
public abstract class PersistableActor<TState, TEnvironment, TCommand, TEvent, TSnapshot, TResponse>
    : PersistableActor<TState, TEnvironment, TCommand, TEvent, TResponse>
    where TState : IPersistableState<TState, TEnvironment, TCommand, TEvent, TSnapshot, TResponse>
    where TCommand : IFallibleCommand<TResponse>
{
    /// <summary>
    /// Initializes snapshot-related recovery and commands.
    /// </summary>
    protected PersistableActor()
    {
        Recover<SnapshotOffer>(RecoverFromSnapshot);
        Command<SaveSnapshotSuccess>(AfterSnapshotSaved);
        Command<DeleteSnapshotsSuccess>(_ => true);
    }

    /// <summary>
    /// Interval (in events) between automatic snapshots. Override to change frequency.
    /// </summary>
    protected virtual long SnapshotInterval => 500L;

    /// <summary>
    /// Recover state from a previously stored snapshot.
    /// </summary>
    /// <param name="offer">Snapshot offer provided by Akka.Persistence during recovery.</param>
    private void RecoverFromSnapshot(SnapshotOffer offer)
    {
        var flow =
            from env in GetEnvironment()
            from snap in IO.lift(() => (TSnapshot)offer.Snapshot)
            from state in TState.FromSnapshot(snap).RunIO(env)
            select state;
        flow.RunSafe().Iter(state => State = state);
    }

    /// <summary>
    /// After persisting an event this method will be called. In addition to applying the event
    /// this implementation may save a snapshot when the configured interval is reached.
    /// </summary>
    /// <param name="event">Persisted event.</param>
    protected override void AfterEventPersisted(TEvent @event)
    {
        base.AfterEventPersisted(@event);
        if (LastSequenceNr % SnapshotInterval != 0L) return;
        var flow =
            from env in GetEnvironment()
            from snapshot in TState.ToSnapshot(State).RunIO(env)
            select snapshot;
        flow.RunSafe().Iter(snap => SaveSnapshot(snap));
    }

    /// <summary>
    /// Called when a snapshot save succeeds — deletes older snapshots up to the saved sequence number.
    /// </summary>
    /// <param name="success">Snapshot success metadata.</param>
    private void AfterSnapshotSaved(SaveSnapshotSuccess success)
    {
        var criteria = new SnapshotSelectionCriteria(success.Metadata.SequenceNr - 1L);
        DeleteSnapshots(criteria);
    }
}
