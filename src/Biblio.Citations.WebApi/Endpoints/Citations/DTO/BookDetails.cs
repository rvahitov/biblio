using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Biblio.Citations.Endpoints.Citations.DTO;

/// <summary>
/// Represents the full details of a book, including its basic info and its chapters.
/// </summary>
public sealed class BookDetails
{
    /// <summary>
    /// Required book metadata.
    /// </summary>
    [Required]
    public required BookInfo BookInfo { get; init; }

    /// <summary>
    /// Array of chapters belonging to the book. Required; each entry is validated by <see cref="ChapterDetailsValidator"/>.
    /// </summary>
    [Required]
    public required ChapterDetails[] Chapters { get; init; }
}


/// <summary>
/// Validator for <see cref="BookDetails"/>.
/// </summary>
/// <remarks>
/// Validation rules:
/// - <see cref="BookDetails.BookInfo"/> must be present and is validated with <see cref="BookInfoValidator"/>.
/// - <see cref="BookDetails.Chapters"/> must be present; each chapter is validated with <see cref="ChapterDetailsValidator"/>.
/// </remarks>
public sealed class BookDetailsValidator : AbstractValidator<BookDetails>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookDetailsValidator"/> class
    /// and configures validation rules for <see cref="BookDetails"/>.
    /// </summary>
    public BookDetailsValidator()
    {
        RuleFor(x => x.BookInfo).NotNull().SetValidator(new BookInfoValidator());
        RuleFor(x => x.Chapters).NotNull();
        RuleForEach(x => x.Chapters).NotNull().SetValidator(new ChapterDetailsValidator());
    }
}