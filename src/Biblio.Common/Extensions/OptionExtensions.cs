using System;
using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace Biblio.Common.Extensions;

/// <summary>
/// Extension methods for LanguageExt <see cref="Option{A}"/>.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Attempts to extract the value from an <see cref="Option{A}"/>.
    /// Returns <c>true</c> when the <paramref name="option"/> is <c>Some</c> and sets
    /// the <paramref name="value"/> out parameter to the contained value. When the option is
    /// <c>None</c> the method returns <c>false</c> and sets <paramref name="value"/> to the default value.
    /// </summary>
    /// <typeparam name="A">The value type contained in the option.</typeparam>
    /// <param name="option">The option instance to inspect.</param>
    /// <param name="value">When this method returns, contains the value if the option was <c>Some</c>; otherwise the default value for <typeparamref name="A"/>.</param>
    /// <returns><c>true</c> if the <paramref name="option"/> is <c>Some</c>; otherwise <c>false</c>.</returns>
    /// <remarks>
    /// This helper obtains the contained value with minimal allocations by using the unsafe access helpers
    /// from <c>LanguageExt.UnsafeValueAccess</c> after first checking <see cref="Option{A}.IsNone"/> to avoid unsafe access.
    /// </remarks>
    /// <example>
    /// <code>
    /// if (myOption.IsJust(out var value))
    /// {
    ///     // use value
    /// }
    /// </code>
    /// </example>
    public static bool IsJust<A>(this Option<A> option, [MaybeNullWhen(false)] out A value)
    {
        value = default;
        if (option.IsNone) return false;
        value = option.ValueUnsafe()!;
        return true;
    }
}
