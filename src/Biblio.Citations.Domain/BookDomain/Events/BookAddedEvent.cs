using System;
using Biblio.Citations.Domain.BookDomain.Models;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Events;

/// <summary>
/// Represents the event of a book being added to the citation domain.
/// </summary>
public class BookAddedEvent : IBookEvent
{
    /// <summary>
    /// Gets the unique identifier of the added book.
    /// </summary>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the title of the added book.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the optional Bible information for the added book.
    /// </summary>
    public Option<BibleInfo> BibleInfo { get; init; }

    ///<inheritdoc/>
    public required DateTimeOffset OccurredOn { get; init; }
}