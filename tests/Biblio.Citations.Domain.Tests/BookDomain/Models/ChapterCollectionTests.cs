using LanguageExt;
using Prelude = LanguageExt.Prelude;

using Biblio.Citations.Domain.BookDomain.Models;
using LanguageExt.UnsafeValueAccess;
using FluentAssertions;

namespace Biblio.Citations.Domain.Tests.BookDomain.Models
{
    public class ChapterCollectionTests
    {
        [Fact]
        public void Add_Find_Contains_Count_Work_AsExpected()
        {
            // Arrange
            var chapterId = new ChapterId(1, Option<int>.None);
            var chapter = new Chapter(chapterId, Option<string>.None, HashSet<int>.Empty);
            var collection = ChapterCollection.Empty;

            // Act
            var updated = collection.Add(chapter);

            // Assert
            updated.Contains(chapterId).Should().BeTrue();
            var found = updated.Find(chapterId);
            found.IsSome.Should().BeTrue();
            updated.Count.Should().Be(1);
        }

        [Fact]
        public void TryUpdateChapter_UpdatesExistingChapter()
        {
            // Arrange
            var chapterId = new ChapterId(2, Option<int>.None);
            var chapter = new Chapter(chapterId, Option<string>.None, HashSet<int>.Empty.Add(1));
            var collection = ChapterCollection.Empty.Add(chapter);

            // Act
            var updatedCollection = collection.TryUpdateChapter(chapterId, ch => ch.AddParagraph(2));

            // Assert
            var found = updatedCollection.Find(chapterId);
            found.IsSome.Should().BeTrue();
            var value = found.ValueUnsafe();
            value.Should().NotBeNull();
            value!.Paragraphs.Contains(1).Should().BeTrue();
            value.Paragraphs.Contains(2).Should().BeTrue();
        }

        [Fact]
        public void TryUpdateChapter_NoChange_WhenIdMissing()
        {
            // Arrange
            var existingId = new ChapterId(3, Option<int>.None);
            var chapter = new Chapter(existingId, Option<string>.None, HashSet<int>.Empty.Add(1));
            var collection = ChapterCollection.Empty.Add(chapter);

            var missingId = new ChapterId(99, Option<int>.None);

            // Act
            var result = collection.TryUpdateChapter(missingId, ch => ch.AddParagraph(5));

            // Assert
            result.Count.Should().Be(collection.Count);
            result.Contains(missingId).Should().BeFalse();
            // original still present
            result.Contains(existingId).Should().BeTrue();
        }

        [Fact]
        public void From_WithSeq_CreatesCollection()
        {
            // Arrange
            var ch1Id = new ChapterId(10, Option<int>.None);
            var ch2Id = new ChapterId(11, Option<int>.None);
            var ch1 = new Chapter(ch1Id, Option<string>.None, HashSet<int>.Empty);
            var ch2 = new Chapter(ch2Id, Option<string>.None, HashSet<int>.Empty);
            var seq = Prelude.Seq(ch1, ch2);

            // Act
            var coll = ChapterCollection.From<Seq>(seq);

            // Assert
            coll.Count.Should().Be(2);
            coll.Contains(ch1Id).Should().BeTrue();
            coll.Contains(ch2Id).Should().BeTrue();
        }

        [Fact]
        public void From_WithEmptySeq_ReturnsEmptyCollection()
        {
            // Arrange
            var empty = Prelude.Seq<Chapter>();

            // Act
            var coll = ChapterCollection.From<Seq>(empty);

            // Assert
            coll.Count.Should().Be(0);
        }
    }
}
