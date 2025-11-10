using System.Collections.Generic;
using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class AddCitationRequestValidatorTests
{
    private readonly AddCitationRequestValidator _validator = new();

    [Fact]
    public void Should_BeValid_When_RequestIsValid()
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "Book1", Title = "Book 1", Authors = ["Author"] },
            Chapter = new ChapterInfo { Number = 1, Volume = null, Title = null },
            Paragraph = 1,
            Text = "Valid citation text",
            Metadata = new Dictionary<string, string> { ["k"] = "v" },
            Tags = ["t"]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1001)]
    public void Should_BeInvalid_When_ParagraphOutOfRange(int invalidParagraph)
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "Book1", Title = "Book 1", Authors = ["Author"] },
            Chapter = new ChapterInfo { Number = 1, Volume = null, Title = null },
            Paragraph = invalidParagraph,
            Text = "Valid text"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Paragraph");
    }

    [Fact]
    public void Should_BeInvalid_When_TextTooShort()
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "Book1", Title = "Book 1", Authors = ["Author"] },
            Chapter = new ChapterInfo { Number = 1, Volume = null, Title = null },
            Paragraph = 1,
            Text = "abc"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Text");
    }

    [Fact]
    public void Should_BeInvalid_When_MetadataContainsEmptyValue()
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "Book1", Title = "Book 1", Authors = ["Author"] },
            Chapter = new ChapterInfo { Number = 1, Volume = null, Title = null },
            Paragraph = 1,
            Text = "Valid text",
            Metadata = new Dictionary<string, string> { ["k"] = "" }
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Metadata[k]");
    }

    [Fact]
    public void Should_BeInvalid_When_TagsContainEmptyString()
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "Book1", Title = "Book 1", Authors = ["Author"] },
            Chapter = new ChapterInfo { Number = 1, Volume = null, Title = null },
            Paragraph = 1,
            Text = "Valid text",
            Tags = ["ok", ""]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Tags"));
    }

    [Fact]
    public void Should_BeInvalid_When_BookOrChapterIsNull()
    {
        // Arrange: Book null
        var dto1 = new AddCitationRequest
        {
            Book = null!,
            Chapter = new ChapterInfo { Number = 1, Volume = null, Title = null },
            Paragraph = 1,
            Text = "Valid text"
        };

        var res1 = _validator.Validate(dto1);
        res1.IsValid.Should().BeFalse();
        res1.Errors.Should().Contain(e => e.PropertyName == "Book");

        // Arrange: Chapter null
        var dto2 = new AddCitationRequest
        {
            Book = new BookInfo { Id = "Book1", Title = "Book 1", Authors = ["Author"] },
            Chapter = null!,
            Paragraph = 1,
            Text = "Valid text"
        };

        var res2 = _validator.Validate(dto2);
        res2.IsValid.Should().BeFalse();
        res2.Errors.Should().Contain(e => e.PropertyName == "Chapter");
    }
}