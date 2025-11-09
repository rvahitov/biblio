using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.BookDomain.Models;
/// <summary>
/// Represents an identifier for a chapter within a book. Optionally includes a volume number
/// when the work is multi-volume.
/// </summary>
/// <param name="Number">The chapter number (1-based).</param>
/// <param name="Volume">Optional volume number when the book is split into volumes.</param>
public sealed record class ChapterId(int Number, Option<int> Volume)
{
    /// <summary>
    /// Validates and creates a <see cref="ChapterId"/> within an applicative/fallible context.
    /// Returns a failure when the provided numbers are invalid.
    /// </summary>
    /// <typeparam name="M">The applicative/fallible context type used by LanguageExt. Must implement <see cref="Applicative{T}"/> and <see cref="Fallible{T}"/>.</typeparam>
    /// <param name="number">The chapter number. Must be greater than zero.</param>
    /// <param name="volume">Optional volume number. When present, must be greater than zero.</param>
    /// <returns>
    /// A <c>K&lt;M,ChapterId&gt;</c> containing either a successful <see cref="ChapterId"/> or a failure with an <see cref="Error"/> describing the validation problem.
    /// </returns>
    /// <remarks>
    /// Validation performed:
    /// - <paramref name="number"/> must be &gt; 0. Otherwise returns an error with message "Chapter number must be greater than zero".
    /// - When <paramref name="volume"/> is present, its value must be &gt; 0. Otherwise returns an error with message "Volume number must be greater than zero when specified".
    /// </remarks>
    public static K<M, ChapterId> From<M>(int number, Option<int> volume)
        where M : Applicative<M>, Fallible<M>
    {
        if (number <= 0)
            return Fallible.error<M, ChapterId>(Error.New("Chapter number must be greater than zero"));
        if (volume.Map(v => v <= 0).IfNone(false))
            return Fallible.error<M, ChapterId>(Error.New("Volume number must be greater than zero when specified"));
        return Applicative.pure<M, ChapterId>(new ChapterId(number, volume));
    }
}
