namespace Biblio.Common;

/// <summary>
/// Represents a type that can apply domain events to produce a new instance.
/// This is a small, type-safe contract intended for event-sourced aggregates or entities.
/// </summary>
/// <typeparam name="T">The concrete type implementing the interface (self type).</typeparam>
/// <typeparam name="TEvent">The event type that can be applied to the aggregate or entity.</typeparam>
public interface IApplyEvent<T, TEvent> where T : IApplyEvent<T, TEvent>
{
    /// <summary>
    /// Applies the specified event to the current instance and returns the updated instance.
    /// Implementations should prefer immutable updates (return a new instance) and avoid
    /// performing external side-effects (persistence, messaging) inside this method.
    /// </summary>
    /// <param name="event">The domain event to apply.</param>
    /// <returns>The updated instance of <typeparamref name="T"/> after applying the event.</returns>
    /// <remarks>
    /// Use this method when rebuilding aggregate state from a stream of events or when
    /// applying a single domain transition. The method follows the principle that
    /// domain model mutation is local and pure; side-effects should be handled by
    /// higher-level services.
    /// </remarks>
    T ApplyEvent(TEvent @event);
}
