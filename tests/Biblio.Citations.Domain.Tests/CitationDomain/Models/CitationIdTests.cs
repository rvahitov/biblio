using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Models;
using FluentAssertions;
using System;
using static LanguageExt.Prelude;

namespace Biblio.Citations.Domain.Tests.CitationDomain.Models;

public class CitationIdTests
{
    [Fact]
    public void Should_CreateCitationId_WithValidParameters()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid().ToString());
        var chapterId = new ChapterId(1, Some(2));
        var paragraphNumber = 5;

        // Act
        var citationId = new CitationId(bookId, chapterId, paragraphNumber);

        // Assert
        citationId.BookId.Should().Be(bookId);
        citationId.ChapterId.Should().Be(chapterId);
        citationId.ParagraphNumber.Should().Be(paragraphNumber);
    }

    [Fact]
    public void Should_ReturnCorrectPersistentId_WithVolume()
    {
        // Arrange
        var bookIdStr = "bookOne";
        var bookId = new BookId(bookIdStr);
        var chapterId = new ChapterId(1, Some(2));
        var paragraphNumber = 5;
        var citationId = new CitationId(bookId, chapterId, paragraphNumber);

        // Act
        var persistentId = citationId.ToPersistentId();

        // Assert
        persistentId.Should().Be($"citation_{bookIdStr}-02-001-005");
    }

    [Fact]
    public void Should_ReturnCorrectPersistentId_WithoutVolume()
    {
        // Arrange
        var bookId = new BookId("bookTwo");
        var chapterId = new ChapterId(1, None);
        var paragraphNumber = 5;
        var citationId = new CitationId(bookId, chapterId, paragraphNumber);

        // Act
        var persistentId = citationId.ToPersistentId();

        // Assert
        persistentId.Should().Be("citation_bookTwo-00-001-005");
    }
}