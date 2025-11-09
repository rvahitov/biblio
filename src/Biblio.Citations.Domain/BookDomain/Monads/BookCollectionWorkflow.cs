using System;
using Biblio.Citations.Domain.BookDomain.Commands;
using Biblio.Citations.Domain.BookDomain.Events;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.BookDomain.Queries;
using Biblio.Citations.Domain.Common;
using Biblio.Citations.Domain.Common.Monads;
using Biblio.Common.Extensions;
using LanguageExt;
using LanguageExt.Common;

namespace Biblio.Citations.Domain.BookDomain.Monads;

/// <summary>
/// Represents an abstract workflow for book collection operations.
/// Provides functional handling of commands and queries for book collections using monadic patterns.
/// </summary>
public abstract class BookCollectionWorkflow : Workflow
{
    private static readonly Workflow<BookCollection> BookCollection =
        AsksM<IBookCollectionWorkflowEnvironment, BookCollection>(env => Pure(env.BookCollection));

    private static Workflow<Book> GetBook(BookId bookId) =>
        from collection in BookCollection
        from book in Lift(collection.Find(bookId), () => Error.New("Book not found"))
        select book;

    private static Workflow<Chapter> GetChapter(Book book, ChapterId chapterId) =>
        book.Chapters.Find(chapterId).IsJust(out var chapter)
            ? Pure(chapter)
            : Fail<Chapter>(Error.New("Chapter not found"));

    private static Workflow<DateTimeOffset> Now => Workflow<DateTimeOffset>.LiftIO(IO.lift(() => DateTimeOffset.Now));

    private static Workflow<Unit> EnsureBookNotExists(BookId bookId) =>
        from collection in BookCollection
        from result in collection.Contains(bookId) ? Fail<Unit>(Error.New("Book already exists")) : Pure(Unit.Default)
        select result;

    private static Workflow<Unit> EnsureChapterNotExists(Book book, ChapterId chapterId) =>
        book.Chapters.Contains(chapterId) ? Fail<Unit>(Error.New("Chapter already exists")) : Pure(Unit.Default);

    private static Workflow<Unit> EnsureParagraphNotExists(Chapter chapter, int paragraph) =>
        chapter.Paragraphs.Contains(paragraph)
            ? Fail<Unit>(Error.New("Paragraph already exists"))
            : Pure(Unit.Default);


    /// <summary>
    /// Processes a book command using functional error handling within the workflow monad.
    /// Dispatches to specific command handlers based on the command type.
    /// </summary>
    /// <param name="command">The book command to process.</param>
    /// <returns>A workflow that produces Unit on success or fails with an error.</returns>
    public static Workflow<Unit> ProcessCommand(IBookCommand command) => command switch
    {
        AddBookCommand cmd => ProcessCommand(cmd),
        AddChapterCommand cmd => ProcessCommand(cmd),
        AddParagraphCommand cmd => ProcessCommand(cmd),
        _ => Fail<Unit>(Error.New("Unknown command"))
    };

    private static Workflow<Unit> ProcessCommand(AddBookCommand command) =>
        from _1 in EnsureBookNotExists(command.BookId)
        from now in Now
        from _2 in AddEvent(new BookAddedEvent
        {
            BookId = command.BookId,
            Title = command.Title,
            BibleInfo = command.BibleInfo,
            OccurredOn = now
        })
        from _3 in AddEvents(command.Authors.Map(IDomainEvent (author) => new BookAuthorAddedEvent
        {
            BookId = command.BookId,
            Author = author,
            OccurredOn = now
        }).ToSeq())
        select Unit.Default;

    private static Workflow<Unit> ProcessCommand(AddChapterCommand command) =>
        from book in GetBook(command.BookId)
        let chapterId = new ChapterId(command.ChapterNumber, command.VolumeNumber)
        from _1 in EnsureChapterNotExists(book, chapterId)
        from _2 in AddEvent(new ChapterAddedEvent
        {
            BookId = command.BookId,
            ChapterNumber = command.ChapterNumber,
            VolumeNumber = command.VolumeNumber,
            Title = command.Title,
            OccurredOn = DateTime.Now
        })
        select Unit.Default;

    private static Workflow<Unit> ProcessCommand(AddParagraphCommand command) =>
        from book in GetBook(command.BookId)
        from chapter in GetChapter(book, command.ChapterId)
        from _1 in EnsureParagraphNotExists(chapter, command.ParagraphNumber)
        from _2 in AddEvent(new ParagraphAddedEvent
        {
            BookId = command.BookId,
            ChapterId = command.ChapterId,
            ParagraphNumber = command.ParagraphNumber,
            OccurredOn = DateTime.Now
        })
        select Unit.Default;

    /// <summary>
    /// Processes a book query using the workflow environment.
    /// Retrieves a book by its identifier with functional error handling.
    /// </summary>
    /// <param name="query">The book query containing the book identifier.</param>
    /// <returns>An effect that produces the found book or fails with an error.</returns>
    public static Eff<IBookCollectionWorkflowEnvironment, Book> ProcessQuery(GetBookQuery query) =>
        from env in Prelude.runtime<IBookCollectionWorkflowEnvironment>()
        from book in GetBook(query.BookId).Eval(env)
        select book;

    /// <summary>
    /// Runs a book command and returns the resulting events.
    /// Executes the command within the workflow environment and collects emitted events.
    /// </summary>
    /// <param name="command">The book command to run.</param>
    /// <returns>An effect that produces a sequence of events emitted during command processing.</returns>
    public static Eff<IBookCollectionWorkflowEnvironment, Seq<IBookEvent>> RunCommand(IBookCommand command) =>
        from env in Prelude.runtime<IBookCollectionWorkflowEnvironment>()
        from state in ProcessCommand(command).Exec(env)
        select state.Events.Map(e => (IBookEvent)e);
}