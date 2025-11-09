using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.BookDomain.Models;

/// <summary>
/// Represents an immutable identifier for a book in the domain.
/// This record wraps a string value to provide stronger type-safety than using a raw <see cref="string"/>.
/// </summary>
/// <param name="Value">The identifier value as a string. Should not be null, empty or whitespace.</param>
public sealed record class BookId(string Value)
{
    /// <summary>
    /// Attempts to create a <see cref="BookId"/> from the provided string value and returns the result
    /// inside an applicative/fallible context (<c>K&lt;M,BookId&gt;</c>).
    /// If <paramref name="value"/> is null, empty or only whitespace, a failure is returned containing an <see cref="Error"/>.
    /// Otherwise a successful <see cref="BookId"/> is returned.
    /// </summary>
    /// <typeparam name="M">The applicative/fallible context type used by LanguageExt. Must implement <see cref="Applicative{T}"/> and <see cref="Fallible{T}"/>.</typeparam>
    /// <param name="value">The raw string value to validate and wrap as a <see cref="BookId"/>.</param>
    /// <returns>
    /// A <c>K&lt;M,BookId&gt;</c> representing either a successful <see cref="BookId"/> or a failure with an <see cref="Error"/> describing the validation problem.
    /// </returns>
    /// <remarks>
    /// Validation performed: the value must not be null, empty or whitespace. The returned failure contains the message "BookId cannot be null or empty" when validation fails.
    /// </remarks>
    public static K<M, BookId> From<M>(string value) where M : Applicative<M>, Fallible<M> =>
        string.IsNullOrWhiteSpace(value)
            ? Fallible.error<M, BookId>(Error.New("BookId cannot be null or empty"))
            : Applicative.pure<M, BookId>(new BookId(value));
}
