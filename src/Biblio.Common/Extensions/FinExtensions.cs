using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using LanguageExt.Common;

namespace Biblio.Common.Extensions;

/// <summary>
/// Extension methods for LanguageExt Fin&lt;T&gt;.
/// </summary>
public static class FinExtensions
{
    /// <summary>
    /// Attempts to extract the success value or failure <see cref="Error"/> from a <see cref="Fin{A}"/>.
    /// Returns <c>true</c> when the <paramref name="fin"/> is in the success state and sets
    /// <paramref name="value"/> to the contained value. When the fin represents failure the method
    /// returns <c>false</c> and sets <paramref name="error"/> to the contained <see cref="Error"/>.
    /// </summary>
    /// <typeparam name="A">The value type contained in the Fin.</typeparam>
    /// <param name="fin">The Fin instance to inspect.</param>
    /// <param name="value">When this method returns, contains the success value if the Fin was successful; otherwise the default value for <typeparamref name="A"/>.</param>
    /// <param name="error">When this method returns, contains the <see cref="Error"/> if the Fin was a failure; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="fin"/> is successful; otherwise <c>false</c>.</returns>
    /// <remarks>
    /// This helper uses <see cref="Fin{A}.IsSucc"/>, <see cref="Fin{A}.SuccSpan"/> and <see cref="Fin{A}.FailSpan"/>
    /// to obtain the contained value or error with minimal allocations.
    /// </remarks>
    public static bool IsSuccess<A>(this Fin<A> fin, [MaybeNullWhen(false)] out A value, [MaybeNullWhen(true)] out Error error)
    {
        value = default;
        error = null;
        if (fin.IsSucc)
        {
            value = fin.SuccSpan()[0];
            return true;
        }
        error = fin.FailSpan()[0];
        return false;
    }

    /// <summary>
    /// Attempts to extract the failure <see cref="Error"/> from a <see cref="Fin{A}"/>.
    /// Returns <c>true</c> when the <paramref name="fin"/> represents a failure and sets
    /// <paramref name="error"/> to the contained <see cref="Error"/>. When the fin is successful
    /// the method returns <c>false</c> and <paramref name="error"/> is set to <c>null</c>.
    /// </summary>
    /// <typeparam name="A">The value type contained in the Fin.</typeparam>
    /// <param name="fin">The Fin instance to inspect.</param>
    /// <param name="error">When this method returns, contains the <see cref="Error"/> if the Fin was a failure; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the <paramref name="fin"/> is a failure; otherwise <c>false</c>.</returns>
    /// <remarks>
    /// This helper reads the fin payload via <see cref="Fin{A}.FailSpan"/> to avoid allocations when possible.
    /// </remarks>
    public static bool IsFailure<A>(this Fin<A> fin, [MaybeNullWhen(false)] out Error error)
    {
        error = null;
        if (fin.IsSucc)
        {
            return false;
        }
        error = fin.FailSpan()[0];
        return true;
    }
}
