using System;
using Biblio.Citations.Domain.BookDomain.Models;

namespace Biblio.Citations.Domain.BookDomain.Events;

/// <summary>
/// Represents the event of a paragraph being added to a chapter in the citation domain.
/// </summary>
public class ParagraphAddedEvent : IBookEvent
{
    /// <summary>
    /// Gets the date and time at which the event occurred.
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }

    /// <summary>
    /// Gets the unique identifier of the book to which the paragraph was added.
    /// </summary>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the chapter to which the paragraph was added.
    /// </summary>
    public required ChapterId ChapterId { get; init; }

    /// <summary>
    /// Gets the number of the added paragraph.
    /// </summary>
    public required int ParagraphNumber { get; init; }
}