using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.BookDomain.Models;
/// <summary>
/// An immutable collection of <see cref="Chapter"/> instances keyed by <see cref="ChapterId"/>.
/// Internally backed by a <see cref="HashMap{TKey,TValue}"/> from LanguageExt to provide functional immutability and efficient lookups.
/// </summary>
/// <param name="Items">The underlying map of chapters.</param>
public sealed record class ChapterCollection(HashMap<ChapterId, Chapter> Items)
{
    /// <summary>
    /// An empty <see cref="ChapterCollection"/> instance.
    /// </summary>
    public static readonly ChapterCollection Empty = new([]);

    /// <summary>
    /// Creates a <see cref="ChapterCollection"/> from a foldable collection of chapters.
    /// </summary>
    /// <typeparam name="TFoldable">The foldable container type (e.g. List, Seq) used by LanguageExt. Must implement <see cref="Foldable{T}"/>.</typeparam>
    /// <param name="chapters">A foldable container of <see cref="Chapter"/> instances. Cannot be null.</param>
    /// <returns>A new <see cref="ChapterCollection"/> containing the provided chapters keyed by their <see cref="ChapterId"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="chapters"/> is null.</exception>
    public static ChapterCollection From<TFoldable>(K<TFoldable, Chapter> chapters)
        where TFoldable : Foldable<TFoldable>
    {
        ArgumentNullException.ThrowIfNull(chapters);
        var map = chapters.Fold(HashMap.empty<ChapterId, Chapter>(), (acc, chapter) => acc.Add(chapter.Id, chapter));
        return new ChapterCollection(map);
    }

    /// <summary>
    /// Returns a new <see cref="ChapterCollection"/> with the provided <paramref name="chapter"/> added or replaced.
    /// </summary>
    /// <param name="chapter">Chapter to add or replace in the collection.</param>
    /// <returns>A new collection instance containing the updated chapters.</returns>
    public ChapterCollection Add(Chapter chapter) => new(Items.Add(chapter.Id, chapter));

    /// <summary>
    /// Determines whether the collection contains a chapter with the given <paramref name="chapterId"/>.
    /// </summary>
    /// <param name="chapterId">The chapter identifier to check for.</param>
    /// <returns>True if a chapter with the specified id exists; otherwise false.</returns>
    public bool Contains(ChapterId chapterId) => Items.ContainsKey(chapterId);

    /// <summary>
    /// Finds a chapter by its identifier.
    /// </summary>
    /// <param name="chapterId">The identifier of the chapter to find.</param>
    /// <returns>An <see cref="Option{T}"/> containing the chapter when found, or <c>None</c> when not present.</returns>
    public Option<Chapter> Find(ChapterId chapterId) => Items.Find(chapterId);

    /// <summary>
    /// Returns an iterable sequence of chapters contained in the collection.
    /// </summary>
    /// <returns>An <see cref="Iterable{T}"/> of <see cref="Chapter"/> values.</returns>
    public Iterable<Chapter> ToIterable() => Items.Values;

    /// <summary>
    /// Gets the number of chapters in the collection.
    /// </summary>
    public int Count => Items.Count;

    /// <summary>
    /// Attempts to update an existing chapter by applying <paramref name="updateFunc"/> to the found value.
    /// If the chapter does not exist, the collection is returned unchanged.
    /// </summary>
    /// <param name="id">The identifier of the chapter to update.</param>
    /// <param name="updateFunc">A function that receives the existing <see cref="Chapter"/> and returns the updated one.</param>
    /// <returns>A new <see cref="ChapterCollection"/> with the updated chapter, or the original collection if the id was not found.</returns>
    public ChapterCollection TryUpdateChapter(ChapterId id, Func<Chapter, Chapter> updateFunc)
    {
        var map = Items.TrySetItem(id, updateFunc);
        return new ChapterCollection(map);
    }
}
