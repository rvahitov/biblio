using System;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using Biblio.Common.Extensions;

namespace Biblio.Citations.Domain.Common.Monads;

public partial class Workflow
{
    /// <summary>
    /// Create a workflow that returns the specified value.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="value">Value to return.</param>
    /// <returns>A workflow that yields <paramref name="value"/>.</returns>
    public static Workflow<A> Pure<A>(A value) => Applicative.pure<Workflow, A>(value).As();

    /// <summary>
    /// Lift an <see cref="IO{A}"/> into a <see cref="Workflow{A}"/>.
    /// The provided <paramref name="io"/> will be executed when the workflow is run
    /// and its result will be returned by the workflow.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="io">The IO action to lift into the workflow.</param>
    /// <returns>A <see cref="Workflow{A}"/> that, when executed, runs the IO and yields its result.</returns>
    public static Workflow<A> LiftIO<A>(IO<A> io) => Workflow<A>.LiftIO(io);

    /// <summary>
    /// Create a failing workflow from an <see cref="Error"/>.
    /// </summary>
    /// <typeparam name="A">Result type parameter.</typeparam>
    /// <param name="error">Error describing the failure.</param>
    /// <returns>A workflow that represents a failure.</returns>
    public static Workflow<A> Fail<A>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return Fallible.error<Workflow, A>(error).As();
    }

    /// <summary>
    /// Read a value from the workflow environment.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="f">Function mapping the environment to a value.</param>
    /// <returns>A workflow that returns the mapped value.</returns>
    public static Workflow<A> Asks<A>(Func<IWorkflowEnvironment, A> f) => Readable.asks<Workflow, IWorkflowEnvironment, A>(f).As();

    /// <summary>
    /// Read a value from the workflow environment with a specific environment type.
    /// </summary>
    /// <typeparam name="E">Specific environment type.</typeparam>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="f">Function mapping the typed environment to a value.</param>
    /// <returns>A workflow that returns the mapped value.</returns>
    public static Workflow<A> Asks<E, A>(Func<E, A> f) where E : IWorkflowEnvironment => Asks(env => f((E)env));

    /// <summary>
    /// Read a workflow-producing function from the environment.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="f">Function mapping the environment to a workflow kind.</param>
    /// <returns>A workflow produced by the environment function.</returns>
    public static Workflow<A> AsksM<A>(Func<IWorkflowEnvironment, K<Workflow, A>> f) => Readable.asksM(f).As();

    /// <summary>
    /// Read a workflow-producing function from the environment with a specific environment type.
    /// </summary>
    /// <typeparam name="E">Specific environment type.</typeparam>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="f">Function mapping the typed environment to a workflow kind.</param>
    /// <returns>A workflow produced by the environment function, or a failure if the environment type is invalid.</returns>
    public static Workflow<A> AsksM<E, A>(Func<E, K<Workflow, A>> f) where E : IWorkflowEnvironment =>
        AsksM(env => env is E e ? f(e) : Fail<A>(Error.New("Invalid environment type")));

    /// <summary>
    /// Lift an <see cref="Option{A}"/> into a workflow, failing with <paramref name="onNone"/> when empty.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="option">Option to lift.</param>
    /// <param name="onNone">Factory for the error when option is empty.</param>
    /// <returns>A workflow that yields the option's value or fails.</returns>
    public static Workflow<A> Lift<A>(Option<A> option, Func<Error> onNone) =>
        option.IsJust(out var a) ? Pure<A>(a) : Fail<A>(onNone());

    /// <summary>
    /// Append a domain event to the workflow state.
    /// </summary>
    /// <typeparam name="E">Type of the domain event.</typeparam>
    /// <param name="event">The domain event to append.</param>
    /// <returns>A workflow that appends the event and returns <see cref="Unit"/>.</returns>
    public static Workflow<Unit> AddEvent<E>(E @event) where E : IDomainEvent =>
        Stateful.modify<Workflow, WorkflowState>(s => s.AddEvent(@event)).As();

    /// <summary>
    /// Append multiple domain events to the workflow state.
    /// </summary>
    /// <param name="events">Sequence of domain events to append.</param>
    /// <returns>A workflow that appends the events and returns <see cref="Unit"/>.</returns>
    public static Workflow<Unit> AddEvents(Seq<IDomainEvent> events) =>
        Stateful.modify<Workflow, WorkflowState>(s => s.AddEvents(events)).As();

    /// <summary>
    /// Conditional choice: if <paramref name="mCond"/> evaluates to true then <paramref name="mThen"/>, else <paramref name="mElse"/>.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="mCond">Workflow producing the condition.</param>
    /// <param name="mThen">Workflow to run when condition is true.</param>
    /// <param name="mElse">Workflow to run when condition is false.</param>
    /// <returns>A workflow that selects between <paramref name="mThen"/> and <paramref name="mElse"/>.</returns>
    public static Workflow<A> Iff<A>(K<Workflow, bool> mCond, K<Workflow, A> mThen, K<Workflow, A> mElse) =>
        mCond.Bind(cond => cond ? mThen : mElse).As();

    /// <summary>
    /// Conditional choice: if <paramref name="flag"/> is true then <paramref name="mThen"/>, else <paramref name="mElse"/>.
    /// </summary>
    /// <typeparam name="A">Result type.</typeparam>
    /// <param name="flag">Boolean condition.</param>
    /// <param name="mThen">Workflow to run when condition is true.</param>
    /// <param name="mElse">Workflow to run when condition is false.</param>
    /// <returns>A workflow that selects between <paramref name="mThen"/> and <paramref name="mElse"/>.</returns>
    public static Workflow<A> Iff<A>(bool flag, K<Workflow, A> mThen, K<Workflow, A> mElse) =>
        flag ? mThen.As() : mElse.As();

    /// <summary>
    /// Execute <paramref name="mThen"/> when <paramref name="mCond"/> evaluates to true; otherwise do nothing.
    /// </summary>
    /// <typeparam name="A">Type parameter for the conditional workflow.</typeparam>
    /// <param name="mCond">Workflow producing the condition.</param>
    /// <param name="mThen">Workflow to execute when condition is true.</param>
    /// <returns>A workflow returning <see cref="Unit"/>.</returns>
    public static Workflow<Unit> When<A>(K<Workflow, bool> mCond, K<Workflow, A> mThen) =>
        mCond.Bind(cond => cond ? mThen.IgnoreF() : Pure(Unit.Default)).As();

    /// <summary>
    /// Execute <paramref name="mThen"/> when <paramref name="flag"/> is true; otherwise do nothing.
    /// </summary>
    /// <typeparam name="A">Type parameter for the conditional workflow.</typeparam>
    /// <param name="flag">Boolean flag to control execution.</param>
    /// <param name="mThen">Workflow to execute when flag is true.</param>
    /// <returns>A workflow returning <see cref="Unit"/>.</returns>
    public static Workflow<Unit> When<A>(bool flag, K<Workflow, A> mThen) =>
        flag ? mThen.IgnoreF().As() : Pure(Unit.Default);
}
