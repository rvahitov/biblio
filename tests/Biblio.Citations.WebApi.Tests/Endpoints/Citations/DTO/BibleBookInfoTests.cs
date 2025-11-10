using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class BibleBookInfoTests
{
    [Fact]
    public void Should_CreateBibleBookInfo_When_AllRequiredPropertiesProvided()
    {
        // Arrange
        var biblePart = BiblePart.OldTestament;
        var orderInBible = 1;
        var orderInPart = 1;
        string[] abbreviations = ["Gen", "Gn"];
        var translation = "Genesis";
        var isApocryphal = false;

        // Act
        var dto = new BibleBookInfo
        {
            BiblePart = biblePart,
            OrderInBible = orderInBible,
            OrderInPart = orderInPart,
            Abbreviations = abbreviations,
            Translation = translation,
            IsApocryphal = isApocryphal
        };

        // Assert
        dto.BiblePart.Should().Be(biblePart);
        dto.OrderInBible.Should().Be(orderInBible);
        dto.OrderInPart.Should().Be(orderInPart);
        dto.Abbreviations.Should().BeEquivalentTo(abbreviations);
        dto.Translation.Should().Be(translation);
        dto.IsApocryphal.Should().Be(isApocryphal);
    }
}
