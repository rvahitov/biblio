using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

/// <summary>
/// Unit tests for the <see cref="ChapterInfoValidator"/>.
/// </summary>
public class ChapterInfoValidatorTests
{
    private readonly ChapterInfoValidator _validator = new();

    /// <summary>
    /// Tests that validation passes when all properties are valid.
    /// </summary>
    [Fact]
    public void Should_BeValid_When_AllPropertiesAreValid()
    {
        // Arrange
        var dto = new ChapterInfo
        {
            Number = 5,
            Volume = 2,
            Title = "Valid Title"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation passes when optional properties are null.
    /// </summary>
    [Fact]
    public void Should_BeValid_When_OptionalPropertiesAreNull()
    {
        // Arrange
        var dto = new ChapterInfo
        {
            Number = 10,
            Volume = null,
            Title = null
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation fails when Number is out of range.
    /// </summary>
    /// <param name="invalidNumber">Invalid number value.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Should_BeInvalid_When_NumberIsOutOfRange(int invalidNumber)
    {
        // Arrange
        var dto = new ChapterInfo
        {
            Number = invalidNumber,
            Volume = null,
            Title = null
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Number");
    }

    /// <summary>
    /// Tests that validation fails when Volume is out of range (when provided).
    /// </summary>
    /// <param name="invalidVolume">Invalid volume value.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Should_BeInvalid_When_VolumeIsOutOfRange(int invalidVolume)
    {
        // Arrange
        var dto = new ChapterInfo
        {
            Number = 5,
            Volume = invalidVolume,
            Title = null
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Volume");
    }

    /// <summary>
    /// Tests that validation fails when Title is invalid (when provided).
    /// </summary>
    /// <param name="invalidTitle">Invalid title value.</param>
    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void Should_BeInvalid_When_TitleIsInvalid(string invalidTitle)
    {
        // Arrange
        var dto = new ChapterInfo
        {
            Number = 5,
            Volume = null,
            Title = invalidTitle
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().AllSatisfy(e => e.PropertyName.Should().Be("Title"));
    }
}