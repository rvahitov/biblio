using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Common.Messages;

namespace Biblio.Citations.Domain.BookDomain.Queries;

/// <summary>
/// Represents a fallible query to retrieve a book by its identifier.
/// This query may succeed with a <see cref="Book"/> or fail with an error.
/// </summary>
public sealed class GetBookQuery : IBookQuery, IFallibleQuery<Book>
{
    /// <summary>
    /// Gets the unique identifier of the book to retrieve.
    /// </summary>
    public required BookId BookId { get; init; }
}