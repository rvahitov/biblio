using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Represents basic information about a book used in citations.
/// </summary>
public sealed class BookInfo
{
    /// <summary>
    /// Short identifier for the book. Required. Maximum length 5.
    /// </summary>
    [Required, MaxLength(5)]
    public required string Id { get; init; }

    /// <summary>
    /// Short title of the book. Required. Maximum length 5.
    /// </summary>
    [Required, MaxLength(5)]
    public required string Title { get; init; }

    /// <summary>
    /// Array of author names. Required; each author entry must be non-empty.
    /// </summary>
    [Required]
    public required string[] Authors { get; init; }

    /// <summary>
    /// Optional bible-specific information for canonical books.
    /// </summary>
    public BibleBookInfo? BibleInfo { get; init; }
}


/// <summary>
/// Validator for <see cref="BookInfo"/>.
/// </summary>
/// <remarks>
/// Validation rules:
/// - <see cref="BookInfo.Id"/> and <see cref="BookInfo.Title"/>: not empty, minimum length 5.
/// - <see cref="BookInfo.Authors"/>: not null and each author must be non-empty.
/// - <see cref="BookInfo.BibleInfo"/>: when present, validated with <see cref="BibleBookInfoValidator"/>.
/// </remarks>
public sealed class BookInfoValidator : AbstractValidator<BookInfo>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookInfoValidator"/> class
    /// and configures validation rules for <see cref="BookInfo"/>.
    /// </summary>
    public BookInfoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().MinimumLength(5);
        RuleFor(x => x.Title).NotEmpty().MinimumLength(5);
        RuleFor(x => x.Authors).NotNull();
        RuleForEach(x => x.Authors).NotEmpty();
        When(x => x.BibleInfo is not null, () =>
        {
            RuleFor(x => x.BibleInfo!).SetValidator(new BibleBookInfoValidator());
        });
    }
}