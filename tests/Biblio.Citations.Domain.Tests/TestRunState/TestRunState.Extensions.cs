using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Tests;

/// <summary>
/// Extension methods for working with <see cref="K{TestRunState, A}"/>/ <see cref="TestRunState{A}"/> kinds.
/// </summary>
public static class TestRunStateExtensions
{
    /// <summary>
    /// Casts a <see cref="K{TestRunState, A}"/> (kind) to the concrete <see cref="TestRunState{A}"/> type.
    /// </summary>
    /// <typeparam name="A">The value type carried by the kind.</typeparam>
    /// <param name="kind">The kind instance to cast.</param>
    /// <returns>The <see cref="TestRunState{A}"/> instance represented by <paramref name="kind"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="kind"/> is <c>null</c>.</exception>
    public static TestRunState<A> As<A>(this K<TestRunState, A> kind)
    {
        ArgumentNullException.ThrowIfNull(kind);
        return (TestRunState<A>)kind;
    }
    /// <summary>
    /// Projects each element of the <see cref="K{TestRunState, A}"/> into a new form.
    /// This is the LINQ-compatible <c>Select</c> operator.
    /// </summary>
    /// <typeparam name="A">Source element type.</typeparam>
    /// <typeparam name="B">Result element type.</typeparam>
    /// <param name="ma">The source computation.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A <see cref="TestRunState{B}"/> representing the transformed computation.</returns>
    public static TestRunState<B> Select<A, B>(
        this K<TestRunState, A> ma,
        Func<A, B> selector
    ) => ma.Map(selector).As();
    /// <summary>
    /// LINQ-compatible SelectMany operator that maps each element to a <see cref="TestRunState{B}"/> and flattens the result.
    /// </summary>
    /// <typeparam name="A">Source element type.</typeparam>
    /// <typeparam name="B">Result element type.</typeparam>
    /// <param name="ma">The source computation.</param>
    /// <param name="selector">A function mapping the source value to a <see cref="TestRunState{B}"/>.</param>
    /// <returns>A flattened <see cref="TestRunState{B}"/> computation.</returns>
    public static TestRunState<B> SelectMany<A, B>(
        this K<TestRunState, A> ma,
        Func<A, TestRunState<B>> selector
    ) => ma.Bind(selector).As();

    /// <summary>
    /// LINQ-compatible SelectMany operator that maps each element to a <see cref="K{TestRunState, B}"/> and flattens the result.
    /// </summary>
    /// <typeparam name="A">Source element type.</typeparam>
    /// <typeparam name="B">Result element type wrapped in a kind.</typeparam>
    /// <param name="ma">The source computation.</param>
    /// <param name="selector">A function mapping the source value to a <see cref="K{TestRunState, B}"/>.</param>
    /// <returns>A flattened <see cref="TestRunState{B}"/> computation.</returns>
    public static TestRunState<B> SelectMany<A, B>(
        this K<TestRunState, A> ma,
        Func<A, K<TestRunState, B>> selector
    ) => ma.Bind(selector).As();

    /// <summary>
    /// LINQ SelectMany overload with projection: binds using <paramref name="selector"/>, then projects the pair
    /// using <paramref name="project"/>.
    /// </summary>
    /// <typeparam name="A">Source element type.</typeparam>
    /// <typeparam name="B">Intermediate result type.</typeparam>
    /// <typeparam name="C">Final projected result type.</typeparam>
    /// <param name="ma">The source computation.</param>
    /// <param name="selector">Function mapping source to an intermediate <see cref="TestRunState{B}"/>.</param>
    /// <param name="project">Projection function combining source and intermediate values into <c>C</c>.</param>
    /// <returns>A <see cref="TestRunState{C}"/> representing the projected computation.</returns>
    public static TestRunState<C> SelectMany<A, B, C>(
        this K<TestRunState, A> ma,
        Func<A, TestRunState<B>> selector,
        Func<A, B, C> project
    ) => ma.Bind(a => selector(a).Map(b => project(a, b))).As();

    /// <summary>
    /// LINQ SelectMany overload with projection for selector returning a kind-level computation.
    /// Binds using <paramref name="selector"/>, then projects the pair using <paramref name="project"/>.
    /// </summary>
    /// <typeparam name="A">Source element type.</typeparam>
    /// <typeparam name="B">Intermediate result type wrapped in a kind.</typeparam>
    /// <typeparam name="C">Final projected result type.</typeparam>
    /// <param name="ma">The source computation.</param>
    /// <param name="selector">Function mapping source to a <see cref="K{TestRunState, B}"/>.</param>
    /// <param name="project">Projection function combining source and intermediate values into <c>C</c>.</param>
    /// <returns>A <see cref="TestRunState{C}"/> representing the projected computation.</returns>
    public static TestRunState<C> SelectMany<A, B, C>(
        this K<TestRunState, A> ma,
        Func<A, K<TestRunState, B>> selector,
        Func<A, B, C> project
    ) => ma.Bind(a => selector(a).Map(b => project(a, b))).As();
}