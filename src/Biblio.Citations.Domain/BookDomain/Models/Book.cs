using System;
using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Models;

public sealed record class Book(
    BookId Id,
    string Title,
    HashSet<string> Authors,
    ChapterCollection Chapters,
    Option<BibleInfo> BibleInfo
)
{
    /// <summary>
    /// Adds <paramref name="chapter"/> to the book and returns a new <see cref="Book"/> instance with the updated chapters.
    /// This method preserves immutability by returning a copy of the current book with the chapter added or replaced.
    /// </summary>
    /// <param name="chapter">Chapter to add to the book.</param>
    /// <returns>A new <see cref="Book"/> instance containing the added chapter.</returns>
    public Book AddChapter(Chapter chapter)
    {
        return this with { Chapters = Chapters.Add(chapter) };
    }

    /// <summary>
    /// Attempts to update an existing chapter identified by <paramref name="id"/> by applying <paramref name="updateFunc"/>.
    /// If the chapter is present, returns a new <see cref="Book"/> with the updated chapter; otherwise returns the original book unchanged.
    /// </summary>
    /// <param name="id">Identifier of the chapter to update.</param>
    /// <param name="updateFunc">Function that receives the existing <see cref="Chapter"/> and returns the updated one.</param>
    /// <returns>A new <see cref="Book"/> with the chapter updated, or the original book if the chapter was not found.</returns>
    public Book TryUpdateChapter(ChapterId id, Func<Chapter, Chapter> updateFunc)
    {
        return this with { Chapters = Chapters.TryUpdateChapter(id, updateFunc) };
    }
}
