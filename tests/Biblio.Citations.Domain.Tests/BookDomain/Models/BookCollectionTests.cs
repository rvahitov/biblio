using LanguageExt;

using Biblio.Citations.Domain.BookDomain.Models;
using Prelude = LanguageExt.Prelude;
using LanguageExt.UnsafeValueAccess;
using FluentAssertions;

namespace Biblio.Citations.Domain.Tests.BookDomain.Models
{
    public class BookCollectionTests
    {
        [Fact]
        public void Add_Find_Contains_Work_AsExpected()
        {
            // Arrange
            var id = new BookId("b1");
            var book = new Book(id, "Title", HashSet<string>.Empty.Add("A"), ChapterCollection.Empty, Option<BibleInfo>.None);
            var coll = BookCollection.Empty;

            // Act
            var updated = coll.Add(book);

            // Assert
            updated.Contains(id).Should().BeTrue();
            var found = updated.Find(id);
            found.IsSome.Should().BeTrue();
            // ensure one item
            updated.Items.Count.Should().Be(1);
        }

        [Fact]
        public void TryUpdateBook_UpdatesExistingBook()
        {
            // Arrange
            var id = new BookId("b2");
            var book = new Book(id, "V1", HashSet<string>.Empty.Add("Auth"), ChapterCollection.Empty, Option<BibleInfo>.None);
            var coll = BookCollection.Empty.Add(book);

            // Act
            var updated = coll.TryUpdateBook(id, b => b with { Title = b.Title + " v2" });

            // Assert
            var found = updated.Find(id);
            found.IsSome.Should().BeTrue();
            var value = found.ValueUnsafe();
            value.Should().NotBeNull();
            value!.Title.Should().Be("V1 v2");
        }

        [Fact]
        public void TryUpdateBook_NoChange_WhenIdMissing()
        {
            // Arrange
            var id = new BookId("b3");
            var book = new Book(id, "T", HashSet<string>.Empty.Add("A"), ChapterCollection.Empty, Option<BibleInfo>.None);
            var coll = BookCollection.Empty.Add(book);

            var missing = new BookId("missing");

            // Act
            var result = coll.TryUpdateBook(missing, b => b with { Title = "X" });

            // Assert
            result.Items.Count.Should().Be(coll.Items.Count);
            result.Contains(missing).Should().BeFalse();
            result.Contains(id).Should().BeTrue();
        }

        [Fact]
        public void From_WithSeq_CreatesCollection()
        {
            // Arrange
            var b1 = new Book(new BookId("b10"), "T1", HashSet<string>.Empty, ChapterCollection.Empty, Option<BibleInfo>.None);
            var b2 = new Book(new BookId("b11"), "T2", HashSet<string>.Empty, ChapterCollection.Empty, Option<BibleInfo>.None);
            var seq = Prelude.Seq(b1, b2);

            // Act
            var coll = BookCollection.From<Seq>(seq);

            // Assert
            coll.Items.Count.Should().Be(2);
            coll.Contains(b1.Id).Should().BeTrue();
            coll.Contains(b2.Id).Should().BeTrue();
        }

        [Fact]
        public void From_WithEmptySeq_ReturnsEmptyCollection()
        {
            // Arrange
            var empty = Prelude.Seq<Book>();

            // Act
            var coll = BookCollection.From<Seq>(empty);

            // Assert
            coll.Items.Count.Should().Be(0);
        }
    }
}
