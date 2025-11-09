using Biblio.Citations.Domain.CitationDomain.Models;

namespace Biblio.Citations.Domain.CitationDomain.Commands;

/// <summary>
/// Command to add a new citation to the system.
/// Contains the citation identifier, text content, associated metadata and tags.
/// </summary>
public sealed class AddCitationCommand : ICitationCommand
{
    /// <summary>
    /// Identifier of the citation to add.
    /// </summary>
    public required CitationId CitationId { get; init; }

    /// <summary>
    /// Text content of the citation.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Collection of metadata associated with the citation (authors, source, etc.).
    /// </summary>
    public required CitationMetaCollection Meta { get; init; }

    /// <summary>
    /// Collection of tags applied to the citation.
    /// </summary>
    public required CitationTagCollection Tags { get; init; }
}