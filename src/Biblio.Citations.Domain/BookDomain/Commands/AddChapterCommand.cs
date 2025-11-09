using Biblio.Citations.Domain.BookDomain.Models;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Commands;

/// <summary>
/// Represents a command to add a new chapter to an existing book.
/// </summary>
public sealed class AddChapterCommand : IBookCommand
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the number of the chapter to add.
    /// </summary>
    public required int ChapterNumber { get; init; }

    /// <summary>
    /// Gets the optional volume number of the chapter.
    /// </summary>
    public required Option<int> VolumeNumber { get; init; }

    /// <summary>
    /// Gets the optional title of the chapter.
    /// </summary>
    public required Option<string> Title { get; init; }
}