using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class BookInfoTests
{
    [Fact]
    public void Should_CreateBookInfo_When_AllRequiredPropertiesProvided()
    {
        // Arrange
        var id = "B001";
        var title = "T001";
        var authors = new[] { "Author1", "Author2" };

        // Act
        var dto = new BookInfo
        {
            Id = id,
            Title = title,
            Authors = authors,
            BibleInfo = null
        };

        // Assert
        dto.Id.Should().Be(id);
        dto.Title.Should().Be(title);
        dto.Authors.Should().BeEquivalentTo(authors);
        dto.BibleInfo.Should().BeNull();
    }

    [Fact]
    public void Should_CreateBookInfo_When_BibleInfoIsNotProvided()
    {
        // Arrange
        var dto = new BookInfo
        {
            Id = "X",
            Title = "Y",
            Authors = ["A"]
            // BibleInfo omitted
        };

        // Assert
        dto.BibleInfo.Should().BeNull();
    }
}