using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.CitationDomain.Models;

/// <summary>
/// A collection of <see cref="CitationMeta"/> items keyed by their string key.
/// This type wraps an immutable <see cref="HashMap{TKey,TValue}"/> to provide domain-specific helpers
/// for working with citation metadata.
/// </summary>
/// <param name="Items">Underlying immutable map of metadata items.</param>
public sealed record class CitationMetaCollection(HashMap<string, CitationMeta> Items)
{
    /// <summary>
    /// An empty <see cref="CitationMetaCollection"/> instance.
    /// </summary>
    public static CitationMetaCollection Empty { get; } = new(HashMap<string, CitationMeta>.Empty);

    /// <summary>
    /// Creates a <see cref="CitationMetaCollection"/> from a foldable sequence of <see cref="CitationMeta"/> items.
    /// </summary>
    /// <typeparam name="T">The foldable collection type (for example, <c>Seq</c> or other LanguageExt foldables).</typeparam>
    /// <param name="foldable">A foldable structure containing <see cref="CitationMeta"/> items to include in the collection.</param>
    /// <returns>A new <see cref="CitationMetaCollection"/> containing the provided items.</returns>
    /// <remarks>
    /// When multiple items share the same key, the last folded item will overwrite earlier values (consistent with <see cref="HashMap{TKey,TValue}.Add"/> behaviour).
    /// </remarks>
    public static CitationMetaCollection From<T>(K<T, CitationMeta> foldable) where T : Foldable<T>
    {
        var items = foldable.Fold(HashMap<string, CitationMeta>.Empty, (map, item) => map.Add(item.Key, item));
        return new CitationMetaCollection(items);
    }

    /// <summary>
    /// Returns a new collection with the provided metadata item added. If an item with the same key already exists,
    /// it will be replaced.
    /// </summary>
    /// <param name="meta">The metadata item to add.</param>
    /// <returns>A new <see cref="CitationMetaCollection"/> containing the added item.</returns>
    public CitationMetaCollection Add(CitationMeta meta) => new(Items.Add(meta.Key, meta));
    
    /// <summary>
    /// Returns the values of the collection as an <see cref="Iterable{T}"/> for iteration.
    /// </summary>
    /// <returns>An <see cref="Iterable{CitationMeta}"/> containing the metadata values.</returns>
    public Iterable<CitationMeta> ToIterable() => Items.Values;
}
