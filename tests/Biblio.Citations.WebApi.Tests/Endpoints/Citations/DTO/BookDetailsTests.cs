using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class BookDetailsTests
{
    [Fact]
    public void Should_CreateBookDetails_When_AllPropertiesProvided()
    {
        // Arrange
        var bookInfo = new BookInfo
        {
            Id = "B1",
            Title = "T1",
            Authors = ["A1"]
        };

        var chapter = new ChapterDetails
        {
            ChapterInfo = new ChapterInfo { Number = 1, Volume = 1, Title = "Ch1" },
            Paragraphs = [1, 2]
        };

        var chapters = new[] { chapter };

        // Act
        var dto = new BookDetails
        {
            BookInfo = bookInfo,
            Chapters = chapters
        };

        // Assert
        dto.BookInfo.Should().BeEquivalentTo(bookInfo);
        dto.Chapters.Should().BeEquivalentTo(chapters);
    }

    [Fact]
    public void Should_CreateBookDetails_When_ChaptersIsEmptyArray()
    {
        // Arrange
        var bookInfo = new BookInfo
        {
            Id = "B2",
            Title = "T2",
            Authors = ["A1"]
        };

        var dto = new BookDetails
        {
            BookInfo = bookInfo,
            Chapters = []
        };

        // Assert
        dto.Chapters.Should().BeEmpty();
    }
}