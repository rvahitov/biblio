using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Common.Monads;

using WorkflowRWS = RWST<IWorkflowEnvironment, Unit, WorkflowState, IO>;

public sealed class Workflow<A> : K<Workflow, A>
{
    public Workflow(K<WorkflowRWS, A> runWorkflow)
    {
        ArgumentNullException.ThrowIfNull(runWorkflow);
        RunWorkflow = runWorkflow;
    }

    public K<WorkflowRWS, A> RunWorkflow { get; }

    public static Workflow<A> LiftIO(IO<A> io)
    {
        ArgumentNullException.ThrowIfNull(io);
        return new(RWST<IWorkflowEnvironment, Unit, WorkflowState, IO, A>.LiftIO(io));
    }
}
