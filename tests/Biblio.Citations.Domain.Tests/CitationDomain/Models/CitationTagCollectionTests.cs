using Biblio.Citations.Domain.CitationDomain.Models;
using FluentAssertions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Biblio.Citations.Domain.Tests.CitationDomain.Models;

public class CitationTagCollectionTests
{
    [Fact]
    public void Should_HaveEmptyCollection_WhenUsingEmptyProperty()
    {
        // Act
        var collection = CitationTagCollection.Empty;

        // Assert
        collection.Items.Count.Should().Be(0);
    }

    [Fact]
    public void Should_CreateFromFoldable_WhenProvidedSeq()
    {
        // Arrange
        var seq = Seq("tag1", "tag2", "tag1"); // duplicate

        // Act
        var collection = CitationTagCollection.From(seq);

        // Assert
        collection.Items.Count.Should().Be(2);
        collection.Items.Contains("tag1").Should().BeTrue();
        collection.Items.Contains("tag2").Should().BeTrue();
    }

    [Fact]
    public void Should_AddTag_WhenAddingToCollection()
    {
        // Arrange
        var initialCollection = CitationTagCollection.Empty;
        var tag = "newTag";

        // Act
        var updatedCollection = initialCollection.Add(tag);

        // Assert
        updatedCollection.Items.Count.Should().Be(1);
        updatedCollection.Items.Contains(tag).Should().BeTrue();
        initialCollection.Items.Count.Should().Be(0); // Original unchanged
    }

    [Fact]
    public void Should_BeIdempotent_WhenAddingExistingTag()
    {
        // Arrange
        var collection = CitationTagCollection.Empty.Add("existing");

        // Act
        var updatedCollection = collection.Add("existing");

        // Assert
        updatedCollection.Items.Count.Should().Be(1);
        updatedCollection.Items.Contains("existing").Should().BeTrue();
    }

    [Fact]
    public void Should_ReturnIterableWithValues_WhenCallingToIterable()
    {
        // Arrange
        var collection = CitationTagCollection.Empty.Add("tag1").Add("tag2");

        // Act
        var iterable = collection.ToIterable();

        // Assert
        var seq = iterable.ToSeq();
        seq.Count.Should().Be(2);
        seq.Contains("tag1").Should().BeTrue();
        seq.Contains("tag2").Should().BeTrue();
    }
}