using Biblio.Citations.Domain.BookDomain.Models;

namespace Biblio.Citations.Domain.BookDomain.Commands;

/// <summary>
/// Represents a command to add a new paragraph to an existing chapter.
/// </summary>
public sealed class AddParagraphCommand : IBookCommand
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the identifier of the chapter to add the paragraph to.
    /// </summary>
    public required ChapterId ChapterId { get; init; }

    /// <summary>
    /// Gets the number of the paragraph to add.
    /// </summary>
    public required int ParagraphNumber { get; init; }
}