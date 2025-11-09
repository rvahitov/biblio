using Biblio.Citations.Domain.BookDomain.Commands;
using Biblio.Citations.Domain.BookDomain.Events;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.BookDomain.Monads;
using Biblio.Citations.Domain.BookDomain.Queries;
using Biblio.Common.Akka;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Actors;

/// <summary>
/// Actor responsible for persisting and querying the book collection state.
/// Delegates command processing and query handling to the <see cref="BookCollectionState"/> workflow
/// and provides the environment required by the workflow.
/// </summary>
public class BookCollectionActor :
    PersistableActor<BookCollectionState, IBookCollectionWorkflowEnvironment, IBookCommand, IBookEvent, Unit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookCollectionActor"/> with the specified persistence identifier.
    /// The constructor also registers the handler used to process <see cref="GetBookQuery"/> queries.
    /// </summary>
    /// <param name="persistenceId">The unique identifier used for persistence (actor journal/stream id).</param>
    public BookCollectionActor(string persistenceId)
    {
        PersistenceId = persistenceId;
        Query<GetBookQuery, Book>(query => BookCollectionState.ProcessQuery(State, query));
    }

    /// <summary>
    /// Gets the persistence identifier used by this actor.
    /// </summary>
    public override string PersistenceId { get; }

    /// <summary>
    /// Provides the workflow environment required to process commands and queries.
    /// Returns the current actor state as an <see cref="IBookCollectionWorkflowEnvironment"/>.
    /// </summary>
    /// <returns>An IO-wrapped environment instance.</returns>
    protected override IO<IBookCollectionWorkflowEnvironment> GetEnvironment() =>
        IO.pure<IBookCollectionWorkflowEnvironment>(State);
}
