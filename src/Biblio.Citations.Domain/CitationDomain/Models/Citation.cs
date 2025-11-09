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
);
