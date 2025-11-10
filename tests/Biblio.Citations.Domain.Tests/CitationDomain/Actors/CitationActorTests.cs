using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Actors;
using Biblio.Citations.Domain.CitationDomain.Commands;
using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Monads;
using Biblio.Common.Extensions;
using LanguageExt;

namespace Biblio.Citations.Domain.Tests.CitationDomain.Actors;

public sealed class CitationActorTests : TestKit
{
    [Fact]
    public void AddCitationCommand_Should_Add_Citation()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid().ToString("N"));
        var chapterId = new ChapterId(1, 1);
        var citationId = new CitationId(bookId, chapterId, 1);
        var text = "This is a test citation.";
        var meta = new CitationMeta("meta1", "value1");
        var addCitation = new AddCitationCommand
        {
            CitationId = citationId,
            Text = text,
            Tags = CitationTagCollection.Empty.Add("tag1"),
            Meta = CitationMetaCollection.Empty.Add(meta)
        };
        var citationActor = Sys.ActorOf(Props.Create(() => new CitationActor("TestCitationActor")));
        var flow =
            from _1 in TestRunState.AddActorRef<CitationActor>(citationActor)
            from _2 in CitationActor<TestRunState>.ExecuteCommand(addCitation)
            from citation in CitationActor<TestRunState>.GetCitation(citationId)
            select citation;
        // Act
        var fin = flow.Eval().RunSafe();
        // Assert
        Assert.True(fin.IsSuccess(out var c, out var err));
        Assert.Null(err);
        Assert.NotNull(c);
        Assert.Equal(citationId, c.Id);
        Assert.Equal(text, c.Text);
        Assert.Contains("tag1", c.Tags.Items);
        Assert.Contains(meta, c.Meta.ToIterable());
    }
}
