
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Represents chapter information for a citation.
/// </summary>
/// <remarks>
/// The <see cref="Number"/> is required and must be between 1 and 500.
/// <see cref="Volume"/> is optional; when provided it must be between 1 and 100.
/// <see cref="Title"/> is optional; when provided it must be non-empty and at least 5 characters long.
/// </remarks>
public sealed class ChapterInfo
{
    /// <summary>
    /// The chapter number. Must be between 1 and 500.
    /// </summary>
    [Range(1, 500)]
    public required int Number { get; init; }

    /// <summary>
    /// Optional volume number for the chapter. When provided, must be between 1 and 100.
    /// </summary>
    public required int? Volume { get; init; }

    /// <summary>
    /// Optional chapter title. When provided, it must be non-empty and at least 5 characters long.
    /// </summary>
    public required string? Title { get; init; }
}

/// <summary>
/// Validator for <see cref="ChapterInfo"/> using FluentValidation.
/// </summary>
/// <remarks>
/// Validation rules configured in this validator:
/// - Number: greater than 0 and less than or equal to 500.
/// - Volume: when present, greater than 0 and less than or equal to 100.
/// - Title: when present, not empty and minimum length of 5 characters.
/// These rules complement the data annotations on the DTO.
/// </remarks>
public sealed class ChapterInfoValidator : AbstractValidator<ChapterInfo>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChapterInfoValidator"/> class
    /// and configures validation rules for <see cref="ChapterInfo"/>.
    /// </summary>
    public ChapterInfoValidator()
    {
        // Redundant due to [Range], but added for consistency
        RuleFor(x => x.Number).GreaterThan(0).LessThanOrEqualTo(500);

        // Volume can be null, but if provided, it should be between 1 and 100
        RuleFor(x => x.Volume).GreaterThan(0).LessThanOrEqualTo(100).When(x => x.Volume.HasValue);

        // Title can be null, but if provided, it should not be empty and have a minimum length of 5
        RuleFor(x => x.Title).NotEmpty().MinimumLength(5).When(x => x.Title is not null);
    }
}
