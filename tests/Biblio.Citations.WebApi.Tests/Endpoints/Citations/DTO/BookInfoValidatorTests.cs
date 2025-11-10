using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class BookInfoValidatorTests
{
    private readonly BookInfoValidator _validator = new();

    [Fact]
    public void Should_BeValid_When_RequiredPropertiesAreValid()
    {
        // Arrange
        var dto = new BookInfo
        {
            Id = "Book1",
            Title = "Book 1",
            Authors = ["Author"],
            BibleInfo = null
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("B1")]
    public void Should_BeInvalid_When_IdIsEmptyOrTooShort(string invalidId)
    {
        // Arrange
        var dto = new BookInfo
        {
            Id = invalidId,
            Title = "Book 1",
            Authors = ["Author"]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be("Id"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("T 1")]
    [InlineData("T1")]
    public void Should_BeInvalid_When_TitleIsEmptyOrTooShort(string invalidTitle)
    {
        // Arrange
        var dto = new BookInfo
        {
            Id = "Book1",
            Title = invalidTitle,
            Authors = ["Author"]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be("Title"));
    }

    [Fact]
    public void Should_BeInvalid_When_AuthorsIsNull()
    {
        // Arrange
        var dto = new BookInfo
        {
            Id = "Book1",
            Title = "Book 1",
            Authors = null!
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Authors");
    }

    [Fact]
    public void Should_BeInvalid_When_AuthorsContainEmptyEntry()
    {
        // Arrange
        var dto = new BookInfo
        {
            Id = "Book1",
            Title = "Book 1",
            Authors = ["Author", ""]
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().AllSatisfy(e => e.PropertyName.Should().StartWith("Authors"));
    }
}