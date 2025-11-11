using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Results;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Request DTO to add a citation for a specific book/chapter/paragraph.
/// </summary>
/// <remarks>
/// Use <see cref="AddCitationRequestValidator"/> to validate instances programmatically.
/// Validation rules are also expressed via Data Annotations for automatic framework integration.
/// </remarks>
public sealed class AddCitationRequest
{
    /// <summary>
    /// Short book metadata required to identify the book being cited.
    /// </summary>
    [Required]
    public required BookInfo Book { get; init; }

    /// <summary>
    /// Chapter information for the citation. Required.
    /// </summary>
    [Required]
    public required ChapterInfo Chapter { get; init; }

    /// <summary>
    /// One-based paragraph index within the chapter. Must be between 1 and 1000.
    /// </summary>
    [Required, Range(1, 1000)]
    public required int Paragraph { get; init; }

    /// <summary>
    /// The citation text. Required and must have at least 5 characters.
    /// </summary>
    [Required, MinLength(5)]
    public required string Text { get; init; }

    /// <summary>
    /// Optional metadata map associated with the citation. Values must be non-empty when provided.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; init; }

    /// <summary>
    /// Optional tags for the citation. When provided, individual tags must be non-empty.
    /// </summary>
    public string[]? Tags { get; init; }
}


/// <summary>
/// Validator for <see cref="AddCitationRequest"/> using FluentValidation.
/// </summary>
/// <remarks>
/// Validation rules:
/// - <see cref="AddCitationRequest.Book"/> and <see cref="AddCitationRequest.Chapter"/> are validated by their respective validators.
/// - <see cref="AddCitationRequest.Paragraph"/> must be in the range 1..1000.
/// - <see cref="AddCitationRequest.Text"/> must be non-empty and at least 5 characters long.
/// - <see cref="AddCitationRequest.Tags"/>, if present, must not contain empty strings.
/// - <see cref="AddCitationRequest.Metadata"/>, if present, must not contain empty values.
/// </remarks>
public sealed class AddCitationRequestValidator : AbstractValidator<AddCitationRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddCitationRequestValidator"/> class
    /// and configures validation rules for <see cref="AddCitationRequest"/>.
    /// </summary>
    public AddCitationRequestValidator()
    {
        RuleFor(x => x.Book).NotNull().SetValidator(new BookInfoValidator());
        RuleFor(x => x.Chapter).NotNull().SetValidator(new ChapterInfoValidator());
        RuleFor(x => x.Paragraph).GreaterThan(0).LessThanOrEqualTo(1000);
        RuleFor(x => x.Text).NotEmpty().MinimumLength(5);
        RuleForEach(x => x.Tags).NotEmpty().When(x => x.Tags is not null);
        RuleForEach(x => x.Metadata!).Custom((keyvaluePair, ctx) =>
        {
            if (!string.IsNullOrWhiteSpace(keyvaluePair.Value)) return;
            var failure = new ValidationFailure
            {
                PropertyName = $"Metadata[{keyvaluePair.Key}]",
                ErrorMessage = "Metadata values must be non-empty.",
                ErrorCode = "NotEmpty"
            };
            ctx.AddFailure(failure);
        })
        .When(x => x.Metadata is not null);
    }
}
