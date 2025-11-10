using System.Collections.Generic;
using Biblio.Citations.Endpoints.Citations.DTO;
using FluentAssertions;

namespace Biblio.Citations.WebApi.Tests.Endpoints.Citations.DTO;

public class AddCitationRequestTests
{
    [Fact]
    public void Should_CreateAddCitationRequest_When_AllPropertiesProvided()
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "B1", Title = "T1", Authors = new[] { "A1" } },
            Chapter = new ChapterInfo { Number = 1, Volume = 1, Title = "Ch1" },
            Paragraph = 2,
            Text = "This is a citation text.",
            Metadata = new Dictionary<string, string> { ["source"] = "archive" },
            Tags = new[] { "tag1", "tag2" }
        };

        // Assert
        dto.Book.Should().NotBeNull();
        dto.Chapter.Should().NotBeNull();
        dto.Paragraph.Should().Be(2);
        dto.Text.Should().StartWith("This is");
        dto.Metadata.Should().ContainKey("source");
        dto.Tags.Should().Contain("tag1");
    }

    [Fact]
    public void Should_CreateAddCitationRequest_When_MetadataAndTagsNull()
    {
        // Arrange
        var dto = new AddCitationRequest
        {
            Book = new BookInfo { Id = "B2", Title = "T2", Authors = new[] { "A" } },
            Chapter = new ChapterInfo { Number = 3, Volume = null, Title = null },
            Paragraph = 1,
            Text = "Text here",
            Metadata = null,
            Tags = null
        };

        // Assert
        dto.Metadata.Should().BeNull();
        dto.Tags.Should().BeNull();
    }
}