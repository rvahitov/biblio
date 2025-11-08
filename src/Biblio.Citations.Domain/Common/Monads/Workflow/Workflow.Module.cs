using System;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using Biblio.Common.Extensions;

namespace Biblio.Citations.Domain.Common.Monads;

public partial class Workflow
{
    public static Workflow<A> Pure<A>(A value) => Applicative.pure<Workflow, A>(value).As();

    public static Workflow<A> Fail<A>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return Fallible.error<Workflow, A>(error).As();
    }
    public static Workflow<A> Asks<A>(Func<IWorkflowEnvironment, A> f) => Readable.asks<Workflow, IWorkflowEnvironment, A>(f).As();
    public static Workflow<A> Asks<E, A>(Func<E, A> f) where E : IWorkflowEnvironment => Asks(env => f((E)env));
    public static Workflow<A> AsksM<A>(Func<IWorkflowEnvironment, K<Workflow, A>> f) => Readable.asksM(f).As();

    public static Workflow<A> AsksM<E, A>(Func<E, K<Workflow, A>> f) where E : IWorkflowEnvironment =>
        AsksM(env => env is E e ? f(e) : Fail<A>(Error.New("Invalid environment type")));

    public static Workflow<A> Lift<A>(Option<A> option, Func<Error> onNone) =>
        option.IsJust(out var a) ? Pure<A>(a) : Fail<A>(onNone());

    public static Workflow<Unit> AddEvent<E>(E @event) where E : IDomainEvent =>
        Stateful.modify<Workflow, WorkflowState>(s => s.AddEvent(@event)).As();

    public static Workflow<Unit> AddEvents(Seq<IDomainEvent> events) =>
        Stateful.modify<Workflow, WorkflowState>(s => s.AddEvents(events)).As();

    public static Workflow<A> Iff<A>(K<Workflow, bool> mCond, K<Workflow, A> mThen, K<Workflow, A> mElse) =>
        mCond.Bind(cond => cond ? mThen : mElse).As();

    public static Workflow<A> Iff<A>(bool flag, K<Workflow, A> mThen, K<Workflow, A> mElse) =>
        flag ? mThen.As() : mElse.As();

    public static Workflow<Unit> When<A>(K<Workflow, bool> mCond, K<Workflow, A> mThen) =>
        mCond.Bind(cond => cond ? mThen.IgnoreF() : Pure(Unit.Default)).As();

    public static Workflow<Unit> When<A>(bool flag, K<Workflow, A> mThen) =>
        flag ? mThen.IgnoreF().As() : Pure(Unit.Default);
}
