namespace Biblio.Common;

/// <summary>
/// Interface for applying events to an aggregate or entity.
/// </summary>    
public interface IApplyEvent<T, TEvent> where T : IApplyEvent<T, TEvent>
{
    /// <summary>
    /// Applies the given event to the current instance and returns the updated instance.
    /// </summary>
    T ApplyEvent(TEvent @event);
}
