using System;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Common.Monads;

using WorkflowRWS = RWST<IWorkflowEnvironment, Unit, WorkflowState, IO>;

/// <summary>
/// Core workflow kind declaration. This partial declaration contains the
/// kind/monad plumbing that allows <see cref="Workflow{A}"/> to interact
/// with LanguageExt's deriving traits (Readable, Stateful, Choice, etc.).
/// </summary>
/// <remarks>
/// The class itself is a marker for the higher-kinded type used throughout
/// the functional abstractions in this module.
/// </remarks>
public abstract partial class Workflow :
    Deriving<Workflow, WorkflowRWS>,
    Deriving.MonadT<Workflow, WorkflowRWS, IO>,
    Deriving.Readable<Workflow, IWorkflowEnvironment, WorkflowRWS>,
    Deriving.Stateful<Workflow, WorkflowRWS, WorkflowState>,
    Deriving.Choice<Workflow, WorkflowRWS>,
    MonadIO<Workflow>,
    Fallible<Workflow>
{
    /// <summary>
    /// Co-transform an underlying <c>WorkflowRWS</c> computation into the
    /// <see cref="Workflow{A}"/> kind.
    /// </summary>
    /// <param name="fa">The underlying RWS computation.</param>
    /// <typeparam name="A">Result type of the computation.</typeparam>
    /// <returns>The computation wrapped as a <see cref="Workflow{A}"/> kind.</returns>
    static K<Workflow, A> CoNatural<Workflow, WorkflowRWS>.CoTransform<A>(K<WorkflowRWS, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return new Workflow<A>(fa);
    }

    /// <summary>
    /// Transform a <see cref="Workflow{A}"/> kind into its underlying
    /// <c>WorkflowRWS</c> representation.
    /// </summary>
    /// <param name="fa">The workflow kind to transform.</param>
    /// <typeparam name="A">Result type.</typeparam>
    /// <returns>The underlying RWS computation.</returns>
    static K<WorkflowRWS, A> Natural<Workflow, WorkflowRWS>.Transform<A>(K<Workflow, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return fa.As().RunWorkflow;
    }

    /// <summary>
    /// Lift an <see cref="IO{A}"/> computation into the <see cref="Workflow{A}"/> kind.
    /// </summary>
    /// <param name="ma">The IO computation to lift.</param>
    /// <typeparam name="A">Result type.</typeparam>
    /// <returns>The lifted workflow kind.</returns>
    static K<Workflow, A> MonadIO<Workflow>.LiftIO<A>(IO<A> ma)
    {
        ArgumentNullException.ThrowIfNull(ma);
        return Workflow<A>.LiftIO(ma);
    }

    /// <summary>
    /// Create a failed workflow from an <see cref="Error"/>.
    /// </summary>
    /// <param name="error">The error to fail with.</param>
    /// <typeparam name="A">Result type parameter.</typeparam>
    /// <returns>A workflow kind that represents a failure.</returns>
    static K<Workflow, A> Fallible<Error, Workflow>.Fail<A>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        var io = IO.fail<A>(error);
        return Workflow<A>.LiftIO(io);
    }

    /// <summary>
    /// Catch errors produced by a workflow and delegate to a recovery function.
    /// </summary>
    /// <param name="fa">The original workflow kind.</param>
    /// <param name="Predicate">Predicate that determines which errors to catch.</param>
    /// <param name="Fail">Recovery function mapping an <see cref="Error"/> to an alternative workflow kind.</param>
    /// <typeparam name="A">Result type.</typeparam>
    /// <returns>A workflow kind that applies the catch semantics.</returns>
    static K<Workflow, A> Fallible<Error, Workflow>.Catch<A>(K<Workflow, A> fa, Func<Error, bool> Predicate, Func<Error, K<Workflow, A>> Fail)
    {
        var rws = new RWST<IWorkflowEnvironment, Unit, WorkflowState, IO, A>(t =>
            fa.Run(t.Env, t.State).Catch(Predicate, err => Fail(err).Run(t.Env, t.State))
        );
        return new Workflow<A>(rws);
    }
}
