using Biblio.Common.Messages;
using LanguageExt;

namespace Biblio.Common;

/// <summary>
/// Interface for processing queries in a functional manner.
/// <typeparamref name="T"/>: The type implementing this interface.
/// <typeparamref name="TEnvironment"/>: The type of the environment required for processing.
/// <typeparamref name="TQuery"/>: The type of the query to be processed, must implement <see cref="IFallibleQuery{TResult}"/>.
/// <typeparamref name="TResult"/>: The type of the result produced by processing the query.
/// <remarks>
/// This interface defines a contract for processing queries in a way that encapsulates side effects.
/// </remarks>
/// </summary>
public interface IProcessQuery<T, TEnvironment, TQuery, TResult>
    where T : IProcessQuery<T, TEnvironment, TQuery, TResult>
    where TQuery : IFallibleQuery<TResult>
{
    /// <summary>
    /// Processes the given query and returns an effectful computation yielding the result.
    /// <param name="self">The instance of the type implementing this interface.</param>
    /// <param name="query">The query to be processed.</param>
    /// <returns>An effectful computation that produces the result of the query.</returns>
    /// <remarks>
    /// This method encapsulates the logic for processing the query and handling any side effects.
    /// </remarks>
    /// </summary>
    public static abstract Eff<TEnvironment, TResult> ProcessQuery(T self, TQuery query);
}
