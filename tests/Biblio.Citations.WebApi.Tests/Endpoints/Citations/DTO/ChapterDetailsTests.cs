using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

/// <summary>
/// Tests for the <see cref="ChapterDetails"/> DTO.
/// </summary>
public class ChapterDetailsTests
{
    [Fact]
    public void Should_CreateChapterDetails_When_AllPropertiesProvided()
    {
        // Arrange
        var chapterInfo = new ChapterInfo
        {
            Number = 1,
            Volume = 2,
            Title = "Chapter One"
        };

        var paragraphs = new[] { 1, 2, 3 };

        // Act
        var dto = new ChapterDetails
        {
            ChapterInfo = chapterInfo,
            Paragraphs = paragraphs
        };

        // Assert
        dto.ChapterInfo.Should().BeEquivalentTo(chapterInfo);
        dto.Paragraphs.Should().BeEquivalentTo(paragraphs);
    }

    [Fact]
    public void Should_CreateChapterDetails_When_OptionalPropertiesNull()
    {
        // Arrange
        var chapterInfo = new ChapterInfo
        {
            Number = 10,
            Volume = null,
            Title = null
        };

        var paragraphs = new[] { 1 };

        // Act
        var dto = new ChapterDetails
        {
            ChapterInfo = chapterInfo,
            Paragraphs = paragraphs
        };

        // Assert
        dto.ChapterInfo.Should().BeEquivalentTo(chapterInfo);
        dto.Paragraphs.Should().BeEquivalentTo(paragraphs);
    }
}