using LanguageExt;

namespace Biblio.Common.Messages;

/// <summary>
/// Marker interface for commands that expect a response of type TResponse.
/// </summary>
public interface ICommand<TResponse> : IMessage<TResponse>;

/// <summary>
/// Marker interface for commands that expect a fallible response of type TResponse.
/// </summary>
public interface IFallibleCommand<TResponse> : ICommand<TResponse>, IFallibleMessage<TResponse>;

/// <summary>
/// Marker interface for commands that do not expect a response.
/// </summary>
public interface ICommand : ICommand<Unit>;
