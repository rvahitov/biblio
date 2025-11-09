using System;
using System.Diagnostics;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Biblio.Citations.Domain.BookDomain.Actors;
using Biblio.Citations.Domain.BookDomain.Commands;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.BookDomain.Monads;
using Biblio.Common.Extensions;
using LanguageExt;

namespace Biblio.Citations.Domain.Tests.BookDomain.Actors;

public class BookCollectionActorTests : TestKit
{
    [Fact]
    public void AddBookCommand_Should_Add_Book()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid().ToString("N"));
        var title = "Book for tests";
        var authors = HashSet.createRange(["Author1", "Author2"]);
        var command = new AddBookCommand
        {
            BookId = bookId,
            Title = title,
            Authors = authors,
            BibleInfo = Option<BibleInfo>.None
        };
        var bookCollectionActor = Sys.ActorOf(Props.Create(() => new BookCollectionActor("TestCollection")));

        var flow =
            from _1 in TestRunState.AddActorRef<BookCollectionActor>(bookCollectionActor)
            from _2 in BookCollectionActor<TestRunState>.ExecuteCommand(command)
            from book in BookCollectionActor<TestRunState>.GetBook(bookId)
            select book;

        // Act
        var fin = flow.Eval().RunSafe();
        Debug.WriteLine(fin);
        // Assert
        Assert.True(fin.IsSuccess(out var b, out var err));
        Assert.Null(err);
        Assert.NotNull(b);
        Assert.Equal(bookId, b.Id);
        Assert.Equal(title, b.Title);
        Assert.Contains("Author1", b.Authors);
        Assert.Contains("Author2", b.Authors);
    }

    [Fact]
    public void AddBookCommand_Should_Not_Add_Duplicate_Book()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid().ToString("N"));
        var title = "Book for tests";
        var authors = HashSet.createRange(["Author1", "Author2"]);
        var command = new AddBookCommand
        {
            BookId = bookId,
            Title = title,
            Authors = authors,
            BibleInfo = Option<BibleInfo>.None
        };
        var bookCollectionActor = Sys.ActorOf(Props.Create(() => new BookCollectionActor("TestCollection")));

        var flow =
            from _1 in TestRunState.AddActorRef<BookCollectionActor>(bookCollectionActor)
            from _2 in BookCollectionActor<TestRunState>.ExecuteCommand(command)
            from _3 in BookCollectionActor<TestRunState>.ExecuteCommand(command)
            from book in BookCollectionActor<TestRunState>.GetBook(bookId)
            select book;

        // Act
        var fin = flow.Eval().RunSafe();
        // Assert
        Assert.True(fin.IsFailure(out var err));
        Assert.Equal("Book already exists", err.Message);
    }

    [Fact]
    public void WhenBookExists_AddChapter_Should_Succeed()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid().ToString("N"));
        var title = "Book for tests";
        var authors = HashSet.createRange(["Author1", "Author2"]);
        var chapterNumber = 1;
        var chapterTitle = "Chapter 1";
        var addBook = new AddBookCommand
        {
            BookId = bookId,
            Title = title,
            Authors = authors,
            BibleInfo = Option<BibleInfo>.None
        };
        var addChapter = new AddChapterCommand
        {
            BookId = bookId,
            ChapterNumber = chapterNumber,
            VolumeNumber = 1,
            Title = chapterTitle
        };
        var bookCollectionActor = Sys.ActorOf(Props.Create(() => new BookCollectionActor("TestCollection")));

        var flow =
            from _1 in TestRunState.AddActorRef<BookCollectionActor>(bookCollectionActor)
            from _2 in BookCollectionActor<TestRunState>.ExecuteCommand(addBook)
            from _3 in BookCollectionActor<TestRunState>.ExecuteCommand(addChapter)
            from book in BookCollectionActor<TestRunState>.GetBook(bookId)
            select book;

        // Act
        var fin = flow.Eval().RunSafe();
        // Assert
        Assert.True(fin.IsSuccess(out var b, out var err));
        Assert.Null(err);
        Assert.NotNull(b);
        Assert.Contains(b.Chapters.Items.Values, ch => ch.Id.Number == chapterNumber && ch.Title == chapterTitle);
    }
}
