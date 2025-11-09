namespace Biblio.Citations.Domain.CitationDomain.Models;

/// <summary>
/// Represents a single key/value metadata entry attached to a citation.
/// </summary>
/// <param name="Key">The metadata key. Keys should be treated as case-sensitive domain identifiers.</param>
/// <param name="Value">The metadata value as a string.</param>
/// <remarks>
/// This simple record is intentionally immutable and lightweight. Use <see cref="CitationMetaCollection"/>
/// to aggregate multiple metadata entries.
/// </remarks>
public sealed record CitationMeta(string Key, string Value);
