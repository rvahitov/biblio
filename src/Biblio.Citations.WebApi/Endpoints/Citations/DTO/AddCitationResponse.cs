using System.ComponentModel.DataAnnotations;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Response returned after a citation has been created.
/// </summary>
/// <remarks>
/// Contains the identifier of the newly-created citation which can be used
/// for subsequent retrieval or linking operations.
/// </remarks>
public sealed class AddCitationResponse
{
    /// <summary>
    /// Identifier of the created citation. Required.
    /// </summary>
    [Required]
    public required string CitationId { get; init; }
}
