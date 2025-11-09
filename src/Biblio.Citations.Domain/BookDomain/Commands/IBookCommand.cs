using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Common.Messages;

namespace Biblio.Citations.Domain.BookDomain.Commands;

/// <summary>
/// Represents a command which targets a book within the Book domain.
/// Implementations of this interface operate on a specific <see cref="BookId"/>.
/// </summary>
public interface IBookCommand : ICommand
{
    /// <summary>
    /// Gets the identifier of the book that this command operates on.
    /// </summary>
    BookId BookId { get; }
}
