using Biblio.Citations.Domain.BookDomain.Commands;
using Biblio.Citations.Domain.BookDomain.Models;
using Biblio.Citations.Domain.BookDomain.Monads;
using Biblio.Citations.Domain.CitationDomain.Commands;
using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Monads;
using Biblio.Citations.Endpoints.Citations.DTO;
using Biblio.Citations.Services.AppMonad;
using LanguageExt;

namespace Biblio.Citations.Services;

public static class CitationService
{
    public static App<AddCitationResponse> AddCitation(AddCitationRequest request) =>
        from bookId in BookId.From<App>(request.Book.Id)
        let bibleInfo = Prelude.Optional(request.Book.BibleInfo)
        let addBook = new AddBookCommand
        {
            BookId = bookId,
            Title = request.Book.Title,
            Authors = HashSet.createRange(request.Book.Authors),
            BibleInfo = Prelude.None // TODO: add BibleBookInfo conversion to domain model
        }
        // Add new book. If it already exists - ignore the error.
        from _1 in BookCollectionActor<App>.ExecuteCommand(addBook) | Prelude.@catch(App.UnitM)
        from chapterId in ChapterId.From<App>(request.Chapter.Number, Prelude.Optional(request.Chapter.Volume))
        let addChapter = new AddChapterCommand
        {
            BookId = bookId,
            ChapterNumber = chapterId.Number,
            VolumeNumber = chapterId.Volume,
            Title = Prelude.Optional(request.Chapter.Title)
        }
        // Add new chapter. If it already exists - ignore the error.
        from _2 in BookCollectionActor<App>.ExecuteCommand(addChapter) | Prelude.@catch(App.UnitM)
        let addParagraph = new AddParagraphCommand
        {
            BookId = bookId,
            ChapterId = chapterId,
            ParagraphNumber = request.Paragraph
        }
        // Add new paragraph. If it already exists - ignore the error.
        from _3 in BookCollectionActor<App>.ExecuteCommand(addParagraph) | Prelude.@catch(App.UnitM)
        let citationId = new CitationId(bookId, chapterId, request.Paragraph)
        let meta = request.Metadata is null
            ? Seq<CitationMeta>.Empty
            : Seq.createRange(request.Metadata!).Map(kv => new CitationMeta(kv.Key, kv.Value))
        let tags = request.Tags is null
            ? HashSet<string>.Empty
            : HashSet.createRange(request.Tags!)
        let addCitation = new AddCitationCommand
        {
            CitationId = citationId,
            Text = request.Text,
            Meta = CitationMetaCollection.From(meta),
            Tags = CitationTagCollection.From(tags)
        }
        from _4 in CitationActor<App>.ExecuteCommand(addCitation)
        select new AddCitationResponse
        {
            CitationId = citationId.ToPersistentId()
        };

}
