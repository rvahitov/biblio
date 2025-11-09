using Biblio.Citations.Domain.CitationDomain.Models;
using FluentAssertions;

namespace Biblio.Citations.Domain.Tests.CitationDomain;

public class CitationMetaTests
{
    [Fact]
    public void Should_CreateCitationMeta_WithValidParameters()
    {
        // Arrange
        var key = "author";
        var value = "John Doe";

        // Act
        var meta = new CitationMeta(key, value);

        // Assert
        meta.Key.Should().Be(key);
        meta.Value.Should().Be(value);
    }

    [Fact]
    public void Should_BeImmutable_WhenCreated()
    {
        // Arrange
        var meta1 = new CitationMeta("key1", "value1");
        var meta2 = new CitationMeta("key1", "value1");

        // Assert
        meta1.Should().Be(meta2);
        meta1.Should().NotBeSameAs(meta2);
    }
}