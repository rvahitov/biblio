using Biblio.Common.Messages;
using LanguageExt;
namespace Biblio.Common;

/// <summary>
/// Interface for processing commands in a functional style.
/// <typeparamref name="T"/>: The type implementing this interface.
/// <typeparamref name="TCommand"/>: The type of command to be processed, must implement <see cref="IFallibleCommand{TResult}"/>.
/// <typeparamref name="TEnvironment"/>: The type of the environment required for processing.
/// <typeparamref name="TEvent"/>: The type of events produced during processing.
/// <typeparamref name="TResult"/>: The type of the result produced after processing.
/// <remarks>
/// This interface defines a static abstract method for processing commands, allowing for a functional programming approach.
/// </remarks>
/// </summary>
public interface IProcessCommand<T, in TCommand, TEnvironment, TEvent, TResult>
    where T : IProcessCommand<T, TCommand, TEnvironment, TEvent, TResult>
    where TCommand : IFallibleCommand<TResult>
{
    /// <summary>
    /// Processes a command and returns an effectful computation that yields events and a result.
    /// <paramref name="self"/>: The instance of the type implementing this interface.
    /// <paramref name="command"/>: The command to be processed.
    /// <returns>>An effectful computation yielding a sequence of events and the result.</returns>
    /// <remarks>
    /// This method is static and abstract, allowing implementations to define their own processing logic.
    /// </remarks>
    /// </summary>
    public static abstract Eff<TEnvironment, (Seq<TEvent> Events, TResult Result)> ProcessCommand(
        T self,
        TCommand command
    );
}
