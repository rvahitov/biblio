using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Common.Monads;

using WorkflowRWS = RWST<IWorkflowEnvironment, Unit, WorkflowState, IO>;

/// <summary>
/// Represents a workflow computation that produces a value of type <typeparamref name="A"/>.
/// Internally this wraps an RWST monad stack (<c>Reader/Writer/State</c>) with an
/// <c>IWorkflowEnvironment</c> environment, unit writer, and <see cref="WorkflowState"/> state over <c>IO</c>.
/// </summary>
/// <typeparam name="A">The result type of the workflow computation.</typeparam>
public sealed class Workflow<A> : K<Workflow, A>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Workflow{A}"/> type from
    /// the underlying RWST representation.
    /// </summary>
    /// <param name="runWorkflow">The underlying RWST computation that implements the workflow.</param>
    public Workflow(K<WorkflowRWS, A> runWorkflow)
    {
        ArgumentNullException.ThrowIfNull(runWorkflow);
        RunWorkflow = runWorkflow;
    }

    /// <summary>
    /// Gets the underlying RWST representation of this workflow.
    /// </summary>
    public K<WorkflowRWS, A> RunWorkflow { get; }

    /// <summary>
    /// Lift an <see cref="IO{A}"/> computation into a <see cref="Workflow{A}"/>.
    /// </summary>
    /// <param name="io">The IO computation to lift.</param>
    /// <returns>A workflow that, when run, executes the provided IO and returns its result.</returns>
    public static Workflow<A> LiftIO(IO<A> io)
    {
        ArgumentNullException.ThrowIfNull(io);
        return new(RWST<IWorkflowEnvironment, Unit, WorkflowState, IO, A>.LiftIO(io));
    }
}
