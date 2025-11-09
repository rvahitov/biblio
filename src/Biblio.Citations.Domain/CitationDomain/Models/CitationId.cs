using System.Text;
using Biblio.Citations.Domain.BookDomain.Models;

namespace Biblio.Citations.Domain.CitationDomain.Models;

/// <summary>
/// Represents an immutable identifier for a citation composed of a book, a chapter, and a paragraph number.
/// </summary>
/// <param name="BookId">The identifier of the book.</param>
/// <param name="ChapterId">The identifier of the chapter. ChapterId contains chapter number and optional volume information.</param>
/// <param name="ParagraphNumber">The paragraph number inside the chapter (1-based).</param>
/// <remarks>
/// This record is immutable and intended to provide a strongly-typed identifier for citations within the domain.
/// Use this type instead of raw primitives to avoid mixing identifiers and to enable domain-specific helpers such as
/// <see cref="ToPersistentId"/>.
/// </remarks>
public sealed record CitationId(BookId BookId, ChapterId ChapterId, int ParagraphNumber)
{
    /// <summary>
    /// Returns a stable, storage-friendly identifier string for this citation.
    /// </summary>
    /// <returns>
    /// A string that uniquely identifies the citation in persistent storage. The format is:
    /// <c>citation-{bookGuid}-{volume:D2}-{chapter:D3}-{paragraph:D2}</c> where <c>volume</c> falls back to 0
    /// when absent.
    /// </returns>
    /// <remarks>
    /// Implementation notes:
    /// - Uses <see cref="BookId.Value"/> for the book part.
    /// - Uses <see cref="ChapterId.Volume"/> and <see cref="ChapterId.Number"/> for chapter information; when
    ///   <see cref="ChapterId.Volume"/> is empty the code falls back to 0 via <c>IfNone(0)</c>.
    /// - This method delegates to the internal <see cref="MakePersistentId(BookId,ChapterId,int)"/> helper.
    /// - Intended for use as a deterministic key in storage layers; it is not a replacement for a database primary key.
    /// </remarks>
    public string ToPersistentId() => MakePersistentId(BookId, ChapterId, ParagraphNumber);

    /// <summary>
    /// Builds the persistent identifier string for a citation from its components.
    /// </summary>
    /// <param name="bookId">The book identifier (used as GUID part).</param>
    /// <param name="chapterId">The chapter identifier containing number and optional volume.</param>
    /// <param name="paragraphNumber">The paragraph number (1-based).</param>
    /// <returns>
    /// Formatted identifier string in the form <c>citation_{bookGuid}-{volume:D2}-{chapter:D3}-{paragraph:D3}</c>.
    /// </returns>
    /// <remarks>
    /// This helper centralizes formatting rules so that the returned string is deterministic and suitable for
    /// use as a storage key or human-readable identifier. Caller is responsible for ensuring values are valid
    /// (e.g. paragraphNumber &gt;= 1).
    /// </remarks>
    private static string MakePersistentId(BookId bookId, ChapterId chapterId, int paragraphNumber)
    {
        var builder = new StringBuilder("citation_");
        builder.Append($"{bookId.Value}-");
        builder.Append($"{chapterId.Volume.IfNone(0):00}-");
        builder.Append($"{chapterId.Number:000}-");
        builder.Append($"{paragraphNumber:000}");
        return builder.ToString();
    }
}