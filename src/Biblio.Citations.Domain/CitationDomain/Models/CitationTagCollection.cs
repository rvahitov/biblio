using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.CitationDomain.Models;

/// <summary>
/// Represents an immutable collection of tags attached to a citation.
/// The underlying storage is a <see cref="HashSet{T}"/> from LanguageExt which provides immutability and set semantics.
/// </summary>
/// <param name="Items">The underlying set of tag strings.</param>
public sealed record CitationTagCollection(HashSet<string> Items)
{
    /// <summary>
    /// An empty <see cref="CitationTagCollection"/> instance.
    /// </summary>
    public static CitationTagCollection Empty { get; } = new(HashSet<string>.Empty);

    /// <summary>
    /// Builds a <see cref="CitationTagCollection"/> from a foldable sequence of tag strings.
    /// </summary>
    /// <typeparam name="T">The foldable collection type (for example, <c>Seq</c> or other LanguageExt foldables).</typeparam>
    /// <param name="foldable">A foldable structure containing tag strings to include in the collection.</param>
    /// <returns>A new <see cref="CitationTagCollection"/> containing the provided tags.</returns>
    /// <remarks>
    /// Duplicate tag values in the source are ignored because the underlying structure is a set.
    /// </remarks>
    public static CitationTagCollection From<T>(K<T, string> foldable) where T : Foldable<T>
    {
        var set = foldable.Fold(HashSet<string>.Empty, (acc, item) => acc.TryAdd(item));
        return new CitationTagCollection(set);
    }

    /// <summary>
    /// Returns a new collection with the provided tag added.
    /// </summary>
    /// <param name="tag">Tag to add. If the tag already exists the operation is idempotent (the set remains unchanged).</param>
    /// <returns>A new <see cref="CitationTagCollection"/> with the tag included.</returns>
    public CitationTagCollection Add(string tag) => new(Items.TryAdd(tag));

    /// <summary>
    /// Returns the tags as an iterable sequence suitable for enumeration.
    /// </summary>
    /// <returns>An <see cref="Iterable{String}"/> containing the tags.</returns>
    public Iterable<string> ToIterable() => Items.AsIterable();

}
