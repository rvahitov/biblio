using Biblio.Citations.Domain.BookDomain.Models;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Commands;

/// <summary>
/// Represents a command to add a new book to the system.
/// </summary>
public sealed class AddBookCommand : IBookCommand
{
    /// <inheritdoc/>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the title of the book.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the authors of the book.
    /// </summary>
    public required HashSet<string> Authors { get; init; }

    /// <summary>
    /// Gets the optional Bible information for the book.
    /// </summary>
    public required Option<BibleInfo> BibleInfo { get; init; }
}