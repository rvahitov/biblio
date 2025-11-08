using LanguageExt;

namespace Biblio.Common.Messages;

/// <summary>
/// Marker interface for messages that expect a response of type TResponse. 
/// </summary>
public interface IMessage<TResponse>;

/// <summary>
/// Marker interface for messages that expect a fallible response of type TResponse.
/// </summary>
public interface IFallibleMessage<TResponse> : IMessage<Fin<TResponse>>;
