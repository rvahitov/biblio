using LanguageExt;
using Biblio.Common.Messages;

namespace Biblio.Common;

/// <summary>
/// Represents a persistable, effectful state for an aggregate or entity that can
/// process commands and apply events. This interface composes <see cref="IProcessCommand{T,TEnvironment,TCommand,TEvent,TResult}"/>
/// and <see cref="IApplyEvent{T,TEvent}"/> to provide a single contract for
/// stateful domain objects which are serializable to snapshots.
/// </summary>
/// <typeparam name="T">The concrete type implementing the interface (self type).</typeparam>
/// <typeparam name="TEnvironment">The ambient environment/dependencies required to execute effects (e.g. repositories, clocks).</typeparam>
/// <typeparam name="TCommand">The command type the state can process; constrained to <see cref="IFallibleCommand{TResult}"/>.</typeparam>
/// <typeparam name="TEvent">The domain event type that can be applied to the state.</typeparam>
/// <typeparam name="TResult">The result type produced by processing commands.</typeparam>
/// <remarks>
/// Implementations should prefer immutable updates: command processing should
/// return events and results as pure values, and <see cref="IApplyEvent{T,TEvent}.ApplyEvent"/>
/// should return a new instance representing the updated state. Side-effects
/// (persistence, messaging) must be performed by higher-level services.
/// </remarks>
public interface IPersistableState<T, TEnvironment, in TCommand, TEvent, TResult> :
    IProcessCommand<T, TEnvironment, TCommand, TEvent, TResult>,
    IApplyEvent<T, TEvent>
    where T : IPersistableState<T, TEnvironment, TCommand, TEvent, TResult>
    where TCommand : IFallibleCommand<TResult>
{
    /// <summary>
    /// The initial (empty/default) instance of the state.
    /// </summary>
    /// <remarks>
    /// Declared as a static abstract member so concrete types can provide a
    /// canonical initial value used when creating new aggregates or when a stream
    /// of events is empty.
    /// </remarks>
    public static abstract T Initial { get; }
}

/// <summary>
/// Extends <see cref="IPersistableState{T,TEnvironment,TCommand,TEvent,TResult}"/> with
/// snapshotting capabilities.
/// </summary>
/// <typeparam name="T">The concrete state type.</typeparam>
/// <typeparam name="TEnvironment">The environment type used by effectful operations.</typeparam>
/// <typeparam name="TCommand">The command type processed by the state.</typeparam>
/// <typeparam name="TEvent">The event type applicable to the state.</typeparam>
/// <typeparam name="TSnapshot">A serializable snapshot representation of the state.</typeparam>
/// <typeparam name="TResult">The result type for command processing.</typeparam>
public interface IPersistableState<T, TEnvironment, in TCommand, TEvent, TSnapshot, TResult> :
    IPersistableState<T, TEnvironment, TCommand, TEvent, TResult>
    where T : IPersistableState<T, TEnvironment, TCommand, TEvent, TResult>
    where TCommand : IFallibleCommand<TResult>
{
    /// <summary>
    /// Converts the current state instance to a snapshot representation.
    /// </summary>
    /// <param name="self">The state instance to snapshot.</param>
    /// <returns>An effectful computation producing <typeparamref name="TSnapshot"/>.</returns>
    /// <remarks>
    /// Use this to produce a compact, serializable representation suitable for
    /// persistence (e.g. for faster recovery than replaying an event log).
    /// </remarks>
    public static abstract Eff<TEnvironment, TSnapshot> ToSnapshot(T self);

    /// <summary>
    /// Reconstructs a state instance from the provided snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot representation to restore from.</param>
    /// <returns>An effectful computation producing the restored state instance.</returns>
    /// <remarks>
    /// Implementations should validate the snapshot and return an appropriate
    /// effect that can fail if the snapshot is invalid or incompatible.
    /// </remarks>
    public static abstract Eff<TEnvironment, T> FromSnapshot(TSnapshot snapshot);
}