namespace Biblio.Common.Messages;

/// <summary>
/// Marker interface for queries that expect a response of type TResponse.
/// </summary>
public interface IQuery<TResponse> : IMessage<TResponse>;

/// <summary>
/// Marker interface for queries that expect a fallible response of type TResponse.
/// </summary>
public interface IFallibleQuery<TResponse> : IQuery<TResponse>, IFallibleMessage<TResponse>;
