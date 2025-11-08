using Biblio.Common.Messages;
using LanguageExt;
namespace Biblio.Common;

/// <summary>
/// Interface for processing commands in a functional style.
/// </summary>
public interface IProcessCommand<T, in TCommand, TEnvironment, TEvent, TResult>
    where T : IProcessCommand<T, TCommand, TEnvironment, TEvent, TResult>
    where TCommand : IFallibleCommand<TResult>
{
    /// <summary>
    /// Processes a command and returns an effectful computation that yields events and a result.
    /// </summary>
    public static abstract Eff<TEnvironment, (Seq<TEvent> Events, TResult Result)> ProcessCommand(
        T self,
        TCommand command
    );
}
