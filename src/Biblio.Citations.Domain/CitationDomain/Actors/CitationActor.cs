using Akka.Actor;
using Akka.Cluster.Sharding;
using Biblio.Citations.Domain.CitationDomain.Commands;
using Biblio.Citations.Domain.CitationDomain.Events;
using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Monads;
using Biblio.Citations.Domain.CitationDomain.Queries;
using Biblio.Common.Akka;
using LanguageExt;

namespace Biblio.Citations.Domain.CitationDomain.Actors;

/// <summary>
/// An actor responsible for managing a single citation's workflow. The actor
/// persists state changes and responds to workflow commands and queries.
/// </summary>
/// <remarks>
/// This class derives from <c>PersistableActor{TState,TEnv,TCommand,TEvent,TResult}</c>
/// and wires up the query handler in the constructor. It also provides helper
/// factory methods for actor props and a message extractor for cluster sharding.
/// </remarks>
public sealed class CitationActor :
    PersistableActor<CitationState, ICitationWorkflowEnvironment, ICitationCommand, ICitationEvent, Unit>
{
    /// <summary>
    /// Creates a new <see cref="CitationActor"/> bound to the specified persistence id.
    /// </summary>
    /// <param name="persistenceId">The persistence identifier used by the actor for event sourcing.</param>
    public CitationActor(string persistenceId)
    {
        PersistenceId = persistenceId;
        Query<GetCitationQuery, Citation>(query => CitationState.ProcessQuery(State, query));
    }

    /// <summary>
    /// Gets the persistence identifier for this actor instance.
    /// </summary>
    public override string PersistenceId { get; }

    /// <summary>
    /// Constructs the environment used by the workflow from the current actor state.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> containing the workflow environment.</returns>
    protected override IO<ICitationWorkflowEnvironment> GetEnvironment() => IO.pure<ICitationWorkflowEnvironment>(State);

    /// <summary>
    /// Extracts a persistent id from a received message for use with cluster sharding.
    /// </summary>
    /// <param name="message">The message from which to extract the id.</param>
    /// <returns>
    /// A persistent id string when the message contains a citation identifier; otherwise <c>null</c>.
    /// </returns>
    private static string? ExtractId(object message) => message switch
    {
        ICitationCommand cmd => cmd.CitationId.ToPersistentId(),
        GetCitationQuery query => query.CitationId.ToPersistentId(),
        _ => null
    };

    /// <summary>
    /// Creates <see cref="Props"/> to instantiate a <see cref="CitationActor"/> with the given persistence id.
    /// </summary>
    /// <param name="persistenceId">The persistence identifier for the created actor.</param>
    /// <returns>A <see cref="Props"/> instance that can be used to create the actor.</returns>
    public static Props CreateProps(string persistenceId) => Props.Create(() => new CitationActor(persistenceId));

    /// <summary>
    /// Creates an <see cref="IMessageExtractor"/> for use with cluster sharding.
    /// </summary>
    /// <param name="maxNumberOfShards">Maximum number of shards to be used by the extractor.</param>
    /// <returns>An <see cref="IMessageExtractor"/> instance that maps messages to shard ids and entity ids.</returns>
    public static IMessageExtractor CreateMessageExtractor(int maxNumberOfShards = 10) =>
        HashCodeMessageExtractor.Create(maxNumberOfShards, ExtractId);
}
