using System;
using Biblio.Citations.Domain.BookDomain.Models;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Events;

/// <summary>
/// Represents the event of a chapter being added to a book in the citation domain.
/// </summary>
public class ChapterAddedEvent : IBookEvent
{
    /// <summary>
    /// Gets the date and time at which the event occurred.
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }

    /// <summary>
    /// Gets the unique identifier of the book to which the chapter was added.
    /// </summary>
    public required BookId BookId { get; init; }

    /// <summary>
    /// Gets the number of the added chapter.
    /// </summary>
    public required int ChapterNumber { get; init; }

    /// <summary>
    /// Gets the optional volume number of the added chapter.
    /// </summary>
    public required Option<int> VolumeNumber { get; init; }

    /// <summary>
    /// Gets the optional title of the added chapter.
    /// </summary>
    public required Option<string> Title { get; init; }
}