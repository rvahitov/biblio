using System;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Common.Monads;

public static class WorkflowExtensions
{
    /// <summary>
    /// Extension helpers for working with the <see cref="Workflow{A}"/> kind.
    /// Includes LINQ-compatible operators (Select/SelectMany), runners (Run/Eval/Exec)
    /// and conveniences for converting between the generic <c>K&lt;Workflow, A&gt;</c> kind
    /// and the concrete <see cref="Workflow{A}"/> type.
    /// </summary>
    /// <summary>
    /// Convert a generic <c>K&lt;Workflow, A&gt;</c> kind to the concrete <see cref="Workflow{A}"/> type.
    /// </summary>
    /// <typeparam name="A">The result type of the workflow.</typeparam>
    /// <param name="kind">The kind to convert.</param>
    /// <returns>The concrete <see cref="Workflow{A}"/> instance.</returns>
    public static Workflow<A> As<A>(this K<Workflow, A> kind)
    {
        ArgumentNullException.ThrowIfNull(kind);
        return (Workflow<A>)kind;
    }

    /// <summary>
    /// Run a workflow with the provided environment and initial state.
    /// </summary>
    /// <typeparam name="A">Result type of the workflow.</typeparam>
    /// <param name="ma">The workflow to run.</param>
    /// <param name="env">The workflow environment.</param>
    /// <param name="state">Initial workflow state.</param>
    /// <returns>An <see cref="IO{T}"/> producing a tuple of the result value, writer output and final state.</returns>
    public static IO<(A Value, Unit Output, WorkflowState State)> Run<A>(
        this K<Workflow, A> ma,
        IWorkflowEnvironment env,
        WorkflowState state
    ) => ma.As().RunWorkflow.Run(env, Unit.Default, state).As();
    /// <summary>
    /// Evaluate a workflow and return its result value, using an empty initial state.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="ma">The workflow to evaluate.</param>
    /// <param name="env">The workflow environment.</param>
    /// <returns>An <see cref="IO{A}"/> producing the workflow's result value.</returns>
    public static IO<A> Eval<A>(this K<Workflow, A> ma, IWorkflowEnvironment env) =>
        ma.Run(env, WorkflowState.Empty).Map(t => t.Value);

    /// <summary>
    /// Execute a workflow and return its final <see cref="WorkflowState"/>, using an empty initial state.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="ma">The workflow to execute.</param>
    /// <param name="env">The workflow environment.</param>
    /// <returns>An <see cref="IO{WorkflowState}"/> producing the final workflow state.</returns>
    public static IO<WorkflowState> Exec<A>(this K<Workflow, A> ma, IWorkflowEnvironment env) =>
        ma.Run(env, WorkflowState.Empty).Map(t => t.State);

    /// <summary>
    /// LINQ Select projection for workflows.
    /// </summary>
    /// <typeparam name="A">Source result type.</typeparam>
    /// <typeparam name="B">Projected result type.</typeparam>
    /// <param name="ma">Source workflow.</param>
    /// <param name="f">Projection function.</param>
    /// <returns>A workflow producing projected values.</returns>
    public static Workflow<B> Select<A, B>(
        this K<Workflow, A> ma,
        Func<A, B> f
    ) => ma.Map(f).As();

    /// <summary>
    /// LINQ SelectMany (bind) overload that accepts a function returning a concrete <see cref="Workflow{B}"/>.
    /// </summary>
    /// <typeparam name="A">Source type.</typeparam>
    /// <typeparam name="B">Result type.</typeparam>
    /// <param name="ma">Source workflow.</param>
    /// <param name="f">Binder function.</param>
    /// <returns>A bound workflow.</returns>
    public static Workflow<B> SelectMany<A, B>(
        this K<Workflow, A> ma,
        Func<A, Workflow<B>> f
    ) => ma.Bind(f).As();

    /// <summary>
    /// LINQ SelectMany (bind) overload that accepts a binder returning the generic kind <c>K&lt;Workflow, B&gt;</c>.
    /// </summary>
    /// <typeparam name="A">Source type.</typeparam>
    /// <typeparam name="B">Result type.</typeparam>
    /// <param name="ma">Source workflow.</param>
    /// <param name="f">Binder function returning a workflow kind.</param>
    /// <returns>A bound workflow.</returns>
    public static Workflow<B> SelectMany<A, B>(
        this K<Workflow, A> ma,
        Func<A, K<Workflow, B>> f
    ) => ma.Bind(f).As();

    /// <summary>
    /// LINQ SelectMany overload for comprehension pattern with projection function.
    /// </summary>
    /// <typeparam name="A">Source type.</typeparam>
    /// <typeparam name="B">Intermediate type.</typeparam>
    /// <typeparam name="C">Projected result type.</typeparam>
    /// <param name="ma">Source workflow.</param>
    /// <param name="f">Binder producing an intermediate workflow.</param>
    /// <param name="g">Projection function combining source and intermediate values.</param>
    /// <returns>A workflow producing values of type <typeparamref name="C"/>.</returns>
    public static Workflow<C> SelectMany<A, B, C>(
        this K<Workflow, A> ma,
        Func<A, Workflow<B>> f,
        Func<A, B, C> g
    ) => ma.Bind(a => f(a).Map(b => g(a, b))).As();

    /// <summary>
    /// LINQ SelectMany overload for comprehension pattern when binder returns a generic workflow kind.
    /// </summary>
    /// <typeparam name="A">Source type.</typeparam>
    /// <typeparam name="B">Intermediate type.</typeparam>
    /// <typeparam name="C">Projected result type.</typeparam>
    /// <param name="ma">Source workflow.</param>
    /// <param name="f">Binder producing an intermediate workflow kind.</param>
    /// <param name="g">Projection function combining source and intermediate values.</param>
    /// <returns>A workflow producing values of type <typeparamref name="C"/>.</returns>
    public static Workflow<C> SelectMany<A, B, C>(
        this K<Workflow, A> ma,
        Func<A, K<Workflow, B>> f,
        Func<A, B, C> g
    ) => ma.Bind(a => f(a).Map(b => g(a, b))).As();

    /// <summary>
    /// LINQ SelectMany overload that supports binding with a <see cref="Guard{TFail, TOk}"/>
    /// to enable guarded computations that may fail with an <see cref="Error"/>.
    /// </summary>
    /// <typeparam name="A">Source type.</typeparam>
    /// <typeparam name="B">Result type.</typeparam>
    /// <param name="ma">Source workflow.</param>
    /// <param name="f">Function producing a guard indicating success or failure.</param>
    /// <param name="g">Projection function executed when the guard succeeds.</param>
    /// <returns>A workflow producing values of type <typeparamref name="B"/>, or a failure.</returns>
    public static Workflow<B> SelectMany<A, B>(
        this K<Workflow, A> ma,
        Func<A, Guard<Error, Unit>> f,
        Func<A, Unit, B> g
    ) => ma.Bind(a => f(a) switch
    {
        { Flag: true } => Workflow.Pure(g(a, default)),
        var guard => Workflow.Fail<B>(guard.OnFalse())
    }).As();
}