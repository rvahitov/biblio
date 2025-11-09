namespace Biblio.Citations.Domain.BookDomain.Queries;

/// <summary>
/// Marker interface for identifying queries related to books within the Book domain.
/// Implementations may use this interface for type safety and query dispatching.
/// 
/// <example>
/// This interface can be used with a query dispatcher or mediator pattern. For example:
/// <code>
/// public class GetBookByIdQuery : IBookQuery
/// {
///     public Guid BookId { get; }
///     public GetBookByIdQuery(Guid bookId) => BookId = bookId;
/// }
/// 
/// // In a dispatcher or handler:
/// public Task&lt;Book&gt; Handle(IBookQuery query) { ... }
/// </code>
/// </example>
/// </summary>
public interface IBookQuery;