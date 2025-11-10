using Biblio.Citations.Domain.CitationDomain.Models;
using FluentAssertions;
using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Biblio.Citations.Domain.Tests.CitationDomain.Models;

public class CitationMetaCollectionTests
{
    [Fact]
    public void Should_HaveEmptyCollection_WhenUsingEmptyProperty()
    {
        // Act
        var collection = CitationMetaCollection.Empty;

        // Assert
        collection.Items.Count.Should().Be(0);
    }

    [Fact]
    public void Should_CreateFromFoldable_WhenProvidedSeq()
    {
        // Arrange
        var meta1 = new CitationMeta("key1", "value1");
        var meta2 = new CitationMeta("key2", "value2");
        var seq = Seq(meta1, meta2);

        // Act
        var collection = CitationMetaCollection.From(seq);

        // Assert
        collection.Items.Count.Should().Be(2);
        collection.Items.ContainsKey("key1").Should().BeTrue();
        collection.Items["key1"].Should().Be(meta1);
        collection.Items.ContainsKey("key2").Should().BeTrue();
        collection.Items["key2"].Should().Be(meta2);
    }

    [Fact]
    public void Should_AddMeta_WhenAddingToCollection()
    {
        // Arrange
        var initialCollection = CitationMetaCollection.Empty;
        var meta = new CitationMeta("newKey", "newValue");

        // Act
        var updatedCollection = initialCollection.Add(meta);

        // Assert
        updatedCollection.Items.Count.Should().Be(1);
        updatedCollection.Items.ContainsKey("newKey").Should().BeTrue();
        updatedCollection.Items["newKey"].Should().Be(meta);
        initialCollection.Items.Count.Should().Be(0); // Original unchanged
    }

    [Fact]
    public void Should_ThrowArgumentException_WhenAddingWithSameKey()
    {
        // Arrange
        var meta1 = new CitationMeta("key", "value1");
        var meta2 = new CitationMeta("key", "value2");
        var collection = CitationMetaCollection.Empty.Add(meta1);

        // Act & Assert
        var action = () => collection.Add(meta2);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_ReturnIterableWithValues_WhenCallingToIterable()
    {
        // Arrange
        var meta1 = new CitationMeta("key1", "value1");
        var meta2 = new CitationMeta("key2", "value2");
        var collection = CitationMetaCollection.Empty.Add(meta1).Add(meta2);

        // Act
        var iterable = collection.ToIterable();

        // Assert
        var seq = iterable.ToSeq();
        seq.Count.Should().Be(2);
        seq.Contains(meta1).Should().BeTrue();
        seq.Contains(meta2).Should().BeTrue();
    }
}