using Biblio.Citations.Domain.BookDomain.Commands;
using Biblio.Citations.Domain.BookDomain.Events;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.BookDomain.Monads;
using Biblio.Citations.Domain.BookDomain.Queries;
using Biblio.Common;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Actors;

/// <summary>
/// Represents the persistent state of a book collection in the domain.
/// Manages the collection of books and applies domain events to maintain state consistency.
/// This record is immutable and intended to be used with an event-sourced workflow.
/// </summary>
/// <param name="Books">The current collection of books.</param>
public sealed record BookCollectionState(BookCollection Books) :
    IPersistableState<BookCollectionState, IBookCollectionWorkflowEnvironment, IBookCommand, IBookEvent, Unit>,
    IBookCollectionWorkflowEnvironment,
    IProcessQuery<BookCollectionState, IBookCollectionWorkflowEnvironment, GetBookQuery, Book>
{
    /// <summary>
    /// Gets the initial empty book collection state.
    /// </summary>
    public static BookCollectionState Initial => new(BookCollection.Empty);

    /// <summary>
    /// Implementation of <see cref="IBookCollectionWorkflowEnvironment.BookCollection"/>.
    /// Exposes the underlying <see cref="BookCollection"/> stored in this state.
    /// </summary>
    BookCollection IBookCollectionWorkflowEnvironment.BookCollection => Books;

    /// <summary>
    /// Processes a command in the workflow and returns the events produced together with a unit result.
    /// The actual command handling is delegated to <see cref="BookCollectionWorkflow.RunCommand"/>.
    /// </summary>
    /// <param name="self">The current state instance (unused by the workflow implementation but required by the interface).</param>
    /// <param name="command">The command to process.</param>
    /// <returns>An effect that produces a sequence of domain events and a unit result.</returns>
    public static Eff<IBookCollectionWorkflowEnvironment, (Seq<IBookEvent> Events, Unit Result)> ProcessCommand(BookCollectionState self, IBookCommand command) =>
        BookCollectionWorkflow.RunCommand(command).Map(events => (events, Unit.Default));

    /// <summary>
    /// Processes a query against the book collection state and returns the requested book.
    /// Delegates to <see cref="BookCollectionWorkflow.ProcessQuery"/>.
    /// </summary>
    /// <param name="self">The current state instance.</param>
    /// <param name="query">The query to execute.</param>
    /// <returns>An effect that produces the requested <see cref="Book"/>.</returns>
    public static Eff<IBookCollectionWorkflowEnvironment, Book> ProcessQuery(BookCollectionState self, GetBookQuery query) => BookCollectionWorkflow.ProcessQuery(query);

    /// <summary>
    /// Applies a domain event to this state and returns the updated state.
    /// Uses pattern matching to dispatch to specific event handlers.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <returns>A new <see cref="BookCollectionState"/> with the event applied.</returns>
    public BookCollectionState ApplyEvent(IBookEvent @event) => @event switch
    {
        BookAddedEvent e => ApplyEvent(e),
        BookAuthorAddedEvent e => ApplyEvent(e),
        ChapterAddedEvent e => ApplyEvent(e),
        ParagraphAddedEvent e => ApplyEvent(e),
        _ => this
    };

    /// <summary>
    /// Handles <see cref="BookAddedEvent"/> by creating a new book and adding it to the collection.
    /// </summary>
    /// <param name="event">The event describing the added book.</param>
    /// <returns>A new state containing the added book.</returns>
    private BookCollectionState ApplyEvent(BookAddedEvent @event)
    {
        var book = new Book(@event.BookId, @event.Title, [], ChapterCollection.Empty, @event.BibleInfo);
        return new BookCollectionState(Books.Add(book));
    }

    /// <summary>
    /// Handles <see cref="BookAuthorAddedEvent"/> by adding the author to the specified book.
    /// </summary>
    /// <param name="event">The event describing the added author.</param>
    /// <returns>A new state with the author added to the book.</returns>
    private BookCollectionState ApplyEvent(BookAuthorAddedEvent @event)
    {
        var books = Books.TryUpdateBook(@event.BookId, book => book with { Authors = book.Authors.Add(@event.Author) });
        return new BookCollectionState(books);
    }

    /// <summary>
    /// Handles <see cref="ChapterAddedEvent"/> by creating the chapter and adding it to the specified book.
    /// </summary>
    /// <param name="event">The event describing the added chapter.</param>
    /// <returns>A new state with the chapter added.</returns>
    private BookCollectionState ApplyEvent(ChapterAddedEvent @event)
    {
        var chapterId = new ChapterId(@event.ChapterNumber, @event.VolumeNumber);
        var chapter = new Chapter(chapterId, @event.Title, []);
        var books = Books.TryUpdateBook(@event.BookId, book => book.AddChapter(chapter));
        return new BookCollectionState(books);
    }

    /// <summary>
    /// Handles <see cref="ParagraphAddedEvent"/> by adding a paragraph to the specified chapter.
    /// </summary>
    /// <param name="event">The event describing the added paragraph.</param>
    /// <returns>A new state with the paragraph added to the chapter.</returns>
    private BookCollectionState ApplyEvent(ParagraphAddedEvent @event)
    {
        var books = Books.TryUpdateBook(@event.BookId, book => book.TryUpdateChapter(@event.ChapterId, chapter => chapter.AddParagraph(@event.ParagraphNumber)));
        return new BookCollectionState(books);
    }
}
