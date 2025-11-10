using System;

namespace Biblio.Citations.Domain.CitationDomain.Models;

/// <summary>
/// Represents a citation within a book: its identifier, the quoted text, metadata and tags.
/// </summary>
/// <param name="Id">The strongly-typed identifier of the citation (<see cref="CitationId"/>).</param>
/// <param name="Text">The citation text content.</param>
/// <param name="Meta">A collection of metadata entries associated with the citation.</param>
/// <param name="Tags">A collection of tags assigned to the citation.</param>
/// <remarks>
/// This record is immutable. Use the provided domain types (<see cref="CitationMetaCollection"/>, <see cref="CitationTagCollection"/>)
/// to work with metadata and tags in a functional, immutable style.
/// </remarks>
public sealed record Citation(
    CitationId Id,
    string Text,
    CitationMetaCollection Meta,
    CitationTagCollection Tags
)
{
    /// <summary>
    /// Returns a new <see cref="Citation"/> with the specified tag added to the <see cref="CitationTagCollection"/>.
    /// </summary>
    /// <param name="tag">The tag to add. Must not be null, empty or whitespace.</param>
    /// <returns>A new <see cref="Citation"/> instance that contains the added tag.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="tag"/> is null, empty or consists only of whitespace.</exception>
    public Citation AddTag(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        return this with { Tags = Tags.Add(tag) };
    }

    /// <summary>
    /// Returns a new <see cref="Citation"/> with the specified metadata entry added to the
    /// <see cref="CitationMetaCollection"/>.
    /// </summary>
    /// <param name="meta">The metadata entry to add. Must not be null.</param>
    /// <returns>A new <see cref="Citation"/> instance that contains the added metadata entry.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="meta"/> is null.</exception>
    public Citation AddMeta(CitationMeta meta)
    {
        ArgumentNullException.ThrowIfNull(meta);
        return this with { Meta = Meta.Add(meta) };
    }
}
