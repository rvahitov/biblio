namespace Biblio.Citations.Domain.Common.Monads;

/// <summary>
/// Represents the environment available to workflows at runtime.
/// Implementations provide services, configuration, or other contextual
/// data that workflow computations may read from during execution.
/// </summary>
public interface IWorkflowEnvironment;
