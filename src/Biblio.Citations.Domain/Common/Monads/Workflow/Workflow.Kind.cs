using System;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Common.Monads;

using WorkflowRWS = RWST<IWorkflowEnvironment, Unit, WorkflowState, IO>;

public abstract partial class Workflow :
    Deriving<Workflow, WorkflowRWS>,
    Deriving.MonadT<Workflow, WorkflowRWS, IO>,
    Deriving.Readable<Workflow, IWorkflowEnvironment, WorkflowRWS>,
    Deriving.Stateful<Workflow, WorkflowRWS, WorkflowState>,
    Deriving.Choice<Workflow, WorkflowRWS>,
    MonadIO<Workflow>,
    Fallible<Workflow>
{
    static K<Workflow, A> CoNatural<Workflow, WorkflowRWS>.CoTransform<A>(K<WorkflowRWS, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return new Workflow<A>(fa);
    }

    static K<WorkflowRWS, A> Natural<Workflow, WorkflowRWS>.Transform<A>(K<Workflow, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return fa.As().RunWorkflow;
    }

    static K<Workflow, A> MonadIO<Workflow>.LiftIO<A>(IO<A> ma)
    {
        ArgumentNullException.ThrowIfNull(ma);
        return Workflow<A>.LiftIO(ma);
    }

    static K<Workflow, A> Fallible<Error, Workflow>.Fail<A>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        var io = IO.fail<A>(error);
        return Workflow<A>.LiftIO(io);
    }

    static K<Workflow, A> Fallible<Error, Workflow>.Catch<A>(K<Workflow, A> fa, Func<Error, bool> Predicate, Func<Error, K<Workflow, A>> Fail)
    {
        var rws = new RWST<IWorkflowEnvironment, Unit, WorkflowState, IO, A>(t =>
            fa.Run(t.Env, t.State).Catch(Predicate, err => Fail(err).Run(t.Env, t.State))
        );
        return new Workflow<A>(rws);
    }
}
