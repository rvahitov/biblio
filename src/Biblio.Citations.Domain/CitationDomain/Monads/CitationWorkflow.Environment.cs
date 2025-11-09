using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.Common.Monads;
using LanguageExt;

namespace Biblio.Citations.Domain.CitationDomain.Monads;

/// <summary>
/// Environment available to citation workflows.
/// Provides access to contextual data the workflow may need, such as an optional
/// current <see cref="Citation"/> instance.
/// </summary>
public interface ICitationWorkflowEnvironment : IWorkflowEnvironment
{
    /// <summary>
    /// Optionally contains the current citation that the workflow operates on.
    /// If this option is <c>None</c>, no citation is present in the environment.
    /// </summary>
    Option<Citation> Citation { get; }
}
