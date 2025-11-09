using System;
using Biblio.Citations.Domain.BookDomain.Models;

namespace Biblio.Citations.Domain.BookDomain.Events;

/// <summary>
/// Represents the event of an author being added to a book in the citation domain.
/// </summary>
public class BookAuthorAddedEvent : IBookEvent
{
    /// <summary>
    /// Gets the date and time at which the event occurred.
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }

    /// <summary>
    /// Gets the unique identifier of the book to which the author was added.
    /// </summary>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the author that was added to the book.
    /// </summary>
    public required string Author { get; init; }
}