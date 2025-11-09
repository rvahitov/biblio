using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.Common;

namespace Biblio.Citations.Domain.BookDomain.Events;

/// <summary>
/// Represents a domain event related to a book in the citation domain.
/// Implementations describe something that happened to a book (identified by <see cref="BookId"/>)
/// and are used for event sourcing and domain notifications.
/// </summary>
public interface IBookEvent : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the book associated with this event.
    /// </summary>
    BookId BookId { get; }
}
