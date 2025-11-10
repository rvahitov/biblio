using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class BookDetailsValidatorTests
{
    private readonly BookDetailsValidator _validator = new();

    [Fact]
    public void Should_BeValid_When_BookInfoAndChaptersValid()
    {
        // Arrange
        var dto = new BookDetails
        {
            BookInfo = new BookInfo { Id = "Book123", Title = "Book 123", Authors = ["Author"] },
            Chapters =
            [
                new ChapterDetails { ChapterInfo = new ChapterInfo { Number = 1, Volume = 1, Title = "Chapter 1" }, Paragraphs = [1] }
            ]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_BeInvalid_When_BookInfoIsNull()
    {
        // Arrange
        var dto = new BookDetails
        {
            BookInfo = null!,
            Chapters = [new ChapterDetails { ChapterInfo = new ChapterInfo { Number = 1, Volume = 1, Title = "C" }, Paragraphs = [1] }]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BookInfo");
    }

    [Fact]
    public void Should_BeInvalid_When_ChaptersIsNull()
    {
        // Arrange
        var dto = new BookDetails
        {
            BookInfo = new BookInfo { Id = "B1", Title = "T1", Authors = ["A"] },
            Chapters = null!
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Chapters");
    }

    [Fact]
    public void Should_BeInvalid_When_ChapterParagraphsContainNonPositive()
    {
        // Arrange
        var dto = new BookDetails
        {
            BookInfo = new BookInfo { Id = "B1", Title = "T1", Authors = ["A"] },
            Chapters =
            [
                new ChapterDetails { ChapterInfo = new ChapterInfo { Number = 1, Volume = 1, Title = "C" }, Paragraphs = [0, 1] }
            ]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Chapters"));
    }

    [Fact]
    public void Should_BeInvalid_When_NestedChapterInfoInvalid()
    {
        // Arrange
        var dto = new BookDetails
        {
            BookInfo = new BookInfo { Id = "B1", Title = "T1", Authors = ["A"] },
            Chapters =
            [
                new ChapterDetails { ChapterInfo = new ChapterInfo { Number = 0, Volume = null, Title = null }, Paragraphs = [1] }
            ]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Chapters"));
    }
}