using System.ComponentModel.DataAnnotations;
using Biblio.Citations.Domain.BookDomain.Models;
using FluentValidation;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Describes metadata for a single book of the Bible used by the citations endpoints.
/// </summary>
/// <remarks>
/// This DTO carries basic structural information (part, order) and human-readable
/// fields (abbreviations and translation). Validation rules are defined in
/// <see cref="BibleBookInfoValidator"/>.
/// </remarks>
public sealed class BibleBookInfo
{
    /// <summary>
    /// The part of the Bible this book belongs to (for example, Old or New Testament).
    /// </summary>
    [Required]
    public required BiblePart BiblePart { get; init; }

    /// <summary>
    /// 1-based position of the book within the whole Bible. Valid values: 1..80.
    /// </summary>
    [Range(1, 80), Required]
    public required int OrderInBible { get; init; }

    /// <summary>
    /// 1-based position of the book within its Bible part. Valid values: 1..60.
    /// </summary>
    [Range(1, 60), Required]
    public required int OrderInPart { get; init; }

    /// <summary>
    /// Common abbreviations for the book (for example, "Gen", "Gn").
    /// May be <c>null</c> when no abbreviations are provided. Each abbreviation
    /// is expected to be at least two characters long.
    /// </summary>
    public required string[]? Abbreviations { get; init; }

    /// <summary>
    /// Short human-readable translation or name for the book. Minimum length is 3 characters.
    /// May be <c>null</c> when a translation is not provided.
    /// </summary>
    [MinLength(3)]
    public required string? Translation { get; init; }

    /// <summary>
    /// Indicates whether the book is considered apocryphal in some canonical lists.
    /// </summary>
    [Required]
    public required bool IsApocryphal { get; init; }
}

/// <summary>
/// FluentValidation validator for <see cref="BibleBookInfo"/>.
/// </summary>
public sealed class BibleBookInfoValidator : AbstractValidator<BibleBookInfo>
{
    /// <summary>
    /// Initializes a new instance of <see cref="BibleBookInfoValidator"/> and sets up
    /// validation rules used by the API layer.
    /// </summary>
    /// <remarks>
    /// Rules applied:
    /// - <see cref="BibleBookInfo.BiblePart"/> must be a defined enum value.
    /// - <see cref="BibleBookInfo.OrderInBible"/> must be between 1 and 80.
    /// - <see cref="BibleBookInfo.OrderInPart"/> must be between 1 and 60.
    /// - Each entry in <see cref="BibleBookInfo.Abbreviations"/> (if provided) must be non-empty
    ///   and at least 2 characters long.
    /// - <see cref="BibleBookInfo.Translation"/> (if provided) must be at least 3 characters long.
    /// </remarks>
    public BibleBookInfoValidator()
    {
        RuleFor(x => x.BiblePart).IsInEnum();
        RuleFor(x => x.OrderInBible).InclusiveBetween(1, 80);
        RuleFor(x => x.OrderInPart).InclusiveBetween(1, 60);
        RuleForEach(x => x.Abbreviations).NotEmpty().MinimumLength(2).When(x => x.Abbreviations != null);
        RuleFor(x => x.Translation).MinimumLength(3);
    }
}
