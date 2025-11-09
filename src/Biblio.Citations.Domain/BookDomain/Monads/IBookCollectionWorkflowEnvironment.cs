using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.Common.Monads;

namespace Biblio.Citations.Domain.BookDomain.Monads;

/// <summary>
/// Defines a workflow environment that provides access to a book collection for functional workflows.
/// This interface extends <see cref="IWorkflowEnvironment"/> to include book collection operations within the RWST monad stack.
/// </summary>
public interface IBookCollectionWorkflowEnvironment : IWorkflowEnvironment
{
    /// <summary>
    /// Gets the immutable book collection used in workflow computations.
    /// </summary>
    /// <returns>An instance of <see cref="BookCollection"/> containing the books.</returns>
    BookCollection BookCollection { get; }
}