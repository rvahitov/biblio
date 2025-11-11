using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

/// <summary>
/// Tests for <see cref="ChapterDetailsValidator"/>.
/// </summary>
public class ChapterDetailsValidatorTests
{
    private readonly ChapterDetailsValidator _validator = new();

    [Fact]
    public void Should_BeValid_When_ChapterInfoAndParagraphsValid()
    {
        // Arrange
        var dto = new ChapterDetails
        {
            ChapterInfo = new ChapterInfo { Number = 5, Volume = 1, Title = "Valid Title" },
            Paragraphs = new[] { 1, 2 }
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_BeInvalid_When_ParagraphsIsNull()
    {
        // Arrange
        var dto = new ChapterDetails
        {
            ChapterInfo = new ChapterInfo { Number = 5, Volume = null, Title = null },
            Paragraphs = null!
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Paragraphs");
    }

    [Fact]
    public void Should_BeInvalid_When_ParagraphContainsNonPositive()
    {
        // Arrange
        var dto = new ChapterDetails
        {
            ChapterInfo = new ChapterInfo { Number = 5, Volume = null, Title = null },
            Paragraphs = [1, 0]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Paragraphs"));
    }

    [Fact]
    public void Should_BeInvalid_When_NestedChapterInfoIsInvalid()
    {
        // Arrange
        var dto = new ChapterDetails
        {
            ChapterInfo = new ChapterInfo { Number = 0, Volume = null, Title = null },
            Paragraphs = [1]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("ChapterInfo"));
    }
}