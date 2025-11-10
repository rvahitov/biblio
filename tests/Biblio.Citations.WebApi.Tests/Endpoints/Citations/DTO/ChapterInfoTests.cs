using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

/// <summary>
/// Unit tests for the <see cref="ChapterInfo"/> DTO.
/// </summary>
public class ChapterInfoTests
{
    /// <summary>
    /// Tests that a <see cref="ChapterInfo"/> can be created with all required properties.
    /// </summary>
    [Fact]
    public void Should_CreateChapterInfo_When_AllRequiredPropertiesProvided()
    {
        // Arrange
        var number = 5;
        int? volume = 2;
        var title = "Introduction to Chapter";

        // Act
        var dto = new ChapterInfo
        {
            Number = number,
            Volume = volume,
            Title = title
        };

        // Assert
        dto.Number.Should().Be(number);
        dto.Volume.Should().Be(volume);
        dto.Title.Should().Be(title);
    }

    /// <summary>
    /// Tests that a <see cref="ChapterInfo"/> can be created with optional properties set to null.
    /// </summary>
    [Fact]
    public void Should_CreateChapterInfo_When_OptionalPropertiesAreNull()
    {
        // Arrange
        var number = 10;
        int? volume = null;
        string? title = null;

        // Act
        var dto = new ChapterInfo
        {
            Number = number,
            Volume = volume,
            Title = title
        };

        // Assert
        dto.Number.Should().Be(number);
        dto.Volume.Should().BeNull();
        dto.Title.Should().BeNull();
    }
}