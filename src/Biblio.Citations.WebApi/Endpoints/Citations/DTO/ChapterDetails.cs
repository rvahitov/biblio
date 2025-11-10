
using FluentValidation;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Represents detailed information about a chapter, including its core info and paragraph indices.
/// </summary>
/// <remarks>
/// - <see cref="ChapterInfo"/> contains metadata about the chapter (number, optional volume and title).
/// - <see cref="Paragraphs"/> contains a list of paragraph numbers (1-based) that belong to the chapter.
/// </remarks>
public sealed class ChapterDetails
{
    /// <summary>
    /// Core information for the chapter. Must be provided.
    /// </summary>
    public required ChapterInfo ChapterInfo { get; init; }

    /// <summary>
    /// Array of paragraph indices belonging to the chapter. Each value should be greater than 0.
    /// </summary>
    public required int[] Paragraphs { get; init; }
}


/// <summary>
/// Validator for <see cref="ChapterDetails"/>.
/// </summary>
/// <remarks>
/// Validation rules:
/// - ChapterInfo is validated using <see cref="ChapterInfoValidator"/>.
/// - Paragraphs must not be null and each paragraph number must be greater than 0.
/// </remarks>
public sealed class ChapterDetailsValidator : AbstractValidator<ChapterDetails>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChapterDetailsValidator"/> class
    /// and configures validation rules for <see cref="ChapterDetails"/>.
    /// </summary>
    public ChapterDetailsValidator()
    {
        // Validate nested ChapterInfo with its own validator
        RuleFor(x => x.ChapterInfo).SetValidator(new ChapterInfoValidator());

        // Paragraphs must be provided and each value must be > 0
        RuleFor(x => x.Paragraphs).NotNull();
        RuleForEach(x => x.Paragraphs).GreaterThan(0).LessThanOrEqualTo(1000);
    }
}