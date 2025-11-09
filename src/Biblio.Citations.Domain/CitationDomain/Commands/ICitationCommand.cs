using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Common.Messages;

namespace Biblio.Citations.Domain.CitationDomain.Commands;

/// <summary>
/// Represents a command that targets a citation aggregate.
/// All citation commands include the identifier of the targeted citation.
/// </summary>
public interface ICitationCommand : ICommand
{
    /// <summary>
    /// Identifier of the citation this command targets.
    /// </summary>
    CitationId CitationId { get; }
}
