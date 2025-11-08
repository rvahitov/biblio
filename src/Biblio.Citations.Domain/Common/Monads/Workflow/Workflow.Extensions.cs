using System;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Common.Monads;

public static class WorkflowExtensions
{
    public static Workflow<A> As<A>(this K<Workflow, A> kind)
    {
        ArgumentNullException.ThrowIfNull(kind);
        return (Workflow<A>)kind;
    }
    public static IO<(A Value, Unit Output, WorkflowState State)> Run<A>(
        this K<Workflow, A> ma,
        IWorkflowEnvironment env,
        WorkflowState state
    ) => ma.As().RunWorkflow.Run(env, Unit.Default, state).As();
    public static IO<A> Eval<A>(this K<Workflow, A> ma, IWorkflowEnvironment env) =>
        ma.Run(env, WorkflowState.Empty).Map(t => t.Value);

    public static IO<WorkflowState> Exec<A>(this K<Workflow, A> ma, IWorkflowEnvironment env) =>
        ma.Run(env, WorkflowState.Empty).Map(t => t.State);

    public static Workflow<B> Select<A, B>(
        this K<Workflow, A> ma,
        Func<A, B> f
    ) => ma.Map(f).As();

    public static Workflow<B> SelectMany<A, B>(
        this K<Workflow, A> ma,
        Func<A, Workflow<B>> f
    ) => ma.Bind(f).As();

    public static Workflow<B> SelectMany<A, B>(
        this K<Workflow, A> ma,
        Func<A, K<Workflow, B>> f
    ) => ma.Bind(f).As();

    public static Workflow<C> SelectMany<A, B, C>(
        this K<Workflow, A> ma,
        Func<A, Workflow<B>> f,
        Func<A, B, C> g
    ) => ma.Bind(a => f(a).Map(b => g(a, b))).As();

    public static Workflow<C> SelectMany<A, B, C>(
        this K<Workflow, A> ma,
        Func<A, K<Workflow, B>> f,
        Func<A, B, C> g
    ) => ma.Bind(a => f(a).Map(b => g(a, b))).As();

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