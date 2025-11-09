using LanguageExt;

namespace Biblio.Citations.Domain.BookDomain.Models;

/// <summary>
/// Represents metadata about a book of the Bible.
/// This record contains information about which part of the Bible the book belongs to,
/// ordering information, common abbreviations, an optional translation name, and whether the book is apocryphal.
/// </summary>
/// <param name="BiblePart">The part of the Bible this book belongs to (OldTestament or NewTestament).</param>
/// <param name="OrderInBible">The overall order of the book inside the whole Bible (1-based index).</param>
/// <param name="OrderInPart">The order of the book within its part (Old or New Testament) (1-based index).</param>
/// <param name="Abbreviations">A set of common abbreviations for the book (may be empty). Never null; use an empty set when none are known.</param>
/// <param name="Translation">Optional translation identifier or name (for example, "NIV", "KJV"). Use <see cref="Option{T}"/> to represent presence/absence.</param>
/// <param name="IsApocryphal">True when the book is considered apocryphal in the given tradition/translation; otherwise false.</param>
public sealed record class BibleInfo(
    BiblePart BiblePart,
    int OrderInBible,
    int OrderInPart,
    HashSet<string> Abbreviations,
    Option<string> Translation,
    bool IsApocryphal
);