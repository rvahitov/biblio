using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Models;

/// <summary>
/// Represents a chapter within a book.
/// Contains an identifier, an optional title and a set of paragraph numbers that belong to the chapter.
/// </summary>
/// <param name="Id">The chapter identifier (see <see cref="ChapterId"/>).</param>
/// <param name="Title">Optional chapter title. Use <see cref="Option{T}"/> to represent presence/absence.</param>
/// <param name="Paragraphs">A set of paragraph numbers contained in the chapter. Use an empty set when there are none.</param>
public sealed record class Chapter(ChapterId Id, Option<string> Title, HashSet<int> Paragraphs)
{
    /// <summary>
    /// Returns a new <see cref="Chapter"/> with the specified paragraph number added to the <see cref="Paragraphs"/> set.
    /// This method preserves immutability by returning a copy with the updated set.
    /// </summary>
    /// <param name="paragraphNumber">The paragraph number to add. It is the caller's responsibility to ensure the number is valid.</param>
    /// <returns>A new <see cref="Chapter"/> instance with the paragraph added to <see cref="Paragraphs"/>.</returns>
    public Chapter AddParagraph(int paragraphNumber) =>
        this with { Paragraphs = Paragraphs.Add(paragraphNumber) };
}
