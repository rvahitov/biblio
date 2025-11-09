using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.BookDomain.Models;
/// <summary>
/// An immutable collection of <see cref="Book"/> instances keyed by <see cref="BookId"/>.
/// Backed by LanguageExt's <see cref="HashMap{TKey,TValue}"/> for efficient functional operations.
/// </summary>
/// <param name="Items">The underlying map of books.</param>
public sealed record class BookCollection(HashMap<BookId, Book> Items)
{
    /// <summary>
    /// Returns an empty <see cref="BookCollection"/>.
    /// </summary>
    public static BookCollection Empty => new(HashMap<BookId, Book>.Empty);

    /// <summary>
    /// Creates a <see cref="BookCollection"/> from a foldable container of <see cref="Book"/> values.
    /// </summary>
    /// <typeparam name="T">The foldable container type used by LanguageExt. Must implement <see cref="Foldable{T}"/>.</typeparam>
    /// <param name="foldable">A foldable container of books.</param>
    /// <returns>A new <see cref="BookCollection"/> containing the provided books keyed by their <see cref="BookId"/>.</returns>
    public static BookCollection From<T>(K<T, Book> foldable) where T : Foldable<T>
    {
        var map = foldable.Fold(HashMap<BookId, Book>.Empty, (books, book) => books.Add(book.Id, book));
        return new BookCollection(map);
    }

    /// <summary>
    /// Returns a new <see cref="BookCollection"/> with the specified <paramref name="book"/> added or replaced.
    /// </summary>
    /// <param name="book">The book to add or replace.</param>
    /// <returns>A new collection containing the updated set of books.</returns>
    public BookCollection Add(Book book) => new(Items: Items.Add(book.Id, book));

    /// <summary>
    /// Determines whether a book with the specified id exists in the collection.
    /// </summary>
    /// <param name="bookId">The identifier of the book to check for.</param>
    /// <returns><c>true</c> if the book exists; otherwise <c>false</c>.</returns>
    public bool Contains(BookId bookId) => Items.ContainsKey(bookId);

    /// <summary>
    /// Finds a book by its identifier.
    /// </summary>
    /// <param name="bookId">The identifier of the book to find.</param>
    /// <returns>An <see cref="Option{T}"/> containing the book when found, or <c>None</c> otherwise.</returns>
    public Option<Book> Find(BookId bookId) => Items.Find(bookId);

    /// <summary>
    /// Attempts to update an existing book by applying <paramref name="updateFunc"/> to the found value.
    /// If the book is not found, the original collection is returned unchanged.
    /// </summary>
    /// <param name="bookId">The identifier of the book to update.</param>
    /// <param name="updateFunc">A function that receives the existing <see cref="Book"/> and returns the updated one.</param>
    /// <returns>A new <see cref="BookCollection"/> with the updated book, or the original collection if the id was not found.</returns>
    public BookCollection TryUpdateBook(BookId bookId, Func<Book, Book> updateFunc)
    {
        var books = Items.TrySetItem(bookId, updateFunc);
        return new BookCollection(books);
    }

    /// <summary>
    /// Returns an iterable sequence of books contained in the collection.
    /// </summary>
    /// <returns>An <see cref="Iterable{T}"/> of <see cref="Book"/> values.</returns>
    public Iterable<Book> ToIterable() => Items.Values;
}
