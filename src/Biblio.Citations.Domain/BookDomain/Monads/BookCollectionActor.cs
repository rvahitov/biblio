using Biblio.Citations.Domain.BookDomain.Actors;
using Biblio.Citations.Domain.BookDomain.Commands;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.BookDomain.Queries;
using Biblio.Common.Akka;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.BookDomain.Monads;

/// <summary>
/// Monad-level helpers for interacting with the <see cref="BookCollectionActor"/>.
/// Provides convenience methods to execute commands and queries against the actor from a generic monadic context <c>M</c>.
/// </summary>
/// <typeparam name="M">The monad type which must provide an <see cref="IActorProvider"/> and support IO operations.</typeparam>
public static class BookCollectionActor<M> where M : Has<M, IActorProvider>, MonadIO<M>
{
    /// <summary>
    /// Executes a domain command against the <see cref="BookCollectionActor"/> and returns a monadic computation producing <see cref="LanguageExt.Unit"/>.
    /// </summary>
    /// <param name="command">The book command to execute.</param>
    /// <returns>A monadic computation of type <c>K&lt;M, Unit&gt;</c> representing the command execution result.</returns>
    public static K<M, Unit> ExecuteCommand(IBookCommand command) =>
        Actor<BookCollectionActor, M>.Ask(command);

    /// <summary>
    /// Retrieves a book by its identifier via the <see cref="BookCollectionActor"/>.
    /// </summary>
    /// <param name="bookId">The identifier of the requested book.</param>
    /// <returns>A monadic computation producing the requested <see cref="Book"/>.</returns>
    public static K<M, Book> GetBook(BookId bookId) =>
        Actor<BookCollectionActor, M>.Ask(new GetBookQuery { BookId = bookId });
}
