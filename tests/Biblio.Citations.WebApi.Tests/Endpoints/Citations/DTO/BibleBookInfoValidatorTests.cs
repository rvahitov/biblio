using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class BibleBookInfoValidatorTests
{
    private readonly BibleBookInfoValidator _validator = new();

    [Fact]
    public void Should_BeValid_When_AllPropertiesAreValid()
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = BiblePart.OldTestament,
            OrderInBible = 1,
            OrderInPart = 1,
            Abbreviations = ["Gen", "Gn"],
            Translation = "Genesis",
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_BeInvalid_When_BiblePartIsUndefined()
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = (BiblePart)999, // Неопределенное значение
            OrderInBible = 1,
            OrderInPart = 1,
            Abbreviations = null,
            Translation = null,
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "BiblePart");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(81)]
    public void Should_BeInvalid_When_OrderInBibleIsOutOfRange(int invalidOrder)
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = BiblePart.OldTestament,
            OrderInBible = invalidOrder,
            OrderInPart = 1,
            Abbreviations = null,
            Translation = null,
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "OrderInBible");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(61)]
    public void Should_BeInvalid_When_OrderInPartIsOutOfRange(int invalidOrder)
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = BiblePart.OldTestament,
            OrderInBible = 1,
            OrderInPart = invalidOrder,
            Abbreviations = null,
            Translation = null,
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "OrderInPart");
    }

    [Fact]
    public void Should_BeInvalid_When_AbbreviationsContainEmptyOrShortStrings()
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = BiblePart.OldTestament,
            OrderInBible = 1,
            OrderInPart = 1,
            Abbreviations = ["", "G"],
            Translation = null,
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().AllSatisfy(e => e.PropertyName.Should().StartWith("Abbreviation"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void Should_BeInvalid_When_TranslationIsTooShort(string invalidTranslation)
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = BiblePart.OldTestament,
            OrderInBible = 1,
            OrderInPart = 1,
            Abbreviations = null,
            Translation = invalidTranslation,
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Translation");
    }

    [Fact]
    public void Should_BeValid_When_AbbreviationsAndTranslationAreNull()
    {
        // Arrange
        var dto = new BibleBookInfo
        {
            BiblePart = BiblePart.OldTestament,
            OrderInBible = 1,
            OrderInPart = 1,
            Abbreviations = null,
            Translation = null,
            IsApocryphal = false
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}