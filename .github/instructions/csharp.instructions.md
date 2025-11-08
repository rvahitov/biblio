---
applyTo: '**/*.cs'
---
## Стиль кодирования

### Общие принципы
- Используй неизменяемые типы данных (immutable)
- Предпочитай функциональные конструкции
- Избегай null, используй Option/Maybe паттерны
- Применяй принципы SOLID и функционального программирования

### Именование
- **Классы**: PascalCase (например, `BookService`, `UserRepository`)
- **Методы**: PascalCase (например, `GetBook`, `CreateUser`)
- **Переменные**: camelCase (например, `bookId`, `userName`)
- **Константы**: UPPER_SNAKE_CASE (например, `MAX_BOOKS_COUNT`)
- **Интерфейсы**: Префикс `I` (например, `IBookService`, `IMessage`)

### Функциональные паттерны
- Используй функторы, монады и аппликативы из библиотеки LanguageEx
- Предпочитай композицию функций вместо наследования
- Используй паттерн Either для обработки ошибок
- Применяй неизменяемые коллекции

## Файловая организация
- Один публичный класс на файл
- Имя файла должно соответствовать имени класса
- Группируй связанные классы в папки по доменам
- Выделяй интерфейсы в отдельные файлы

## Комментарии и документация
- Используй XML-документацию для публичных API
- Добавляй комментарии для сложной бизнес-логики
- Объясняй математические концепции из теории категорий
- Документируй функциональные композиции

### XML документация на английском языке

#### Общие принципы
- Вся XML документация должна быть на английском языке
- Используй краткие, но информативные описания
- Объясняй назначение параметров и возвращаемых значений
- Указывай возможные исключения

#### Обязательные теги
```csharp
/// <summary>
/// Brief description of the purpose of the class/method
/// </summary>
/// <param name="parameterName">Description of the parameter</param>
/// <returns>Description of the return value</returns>
/// <exception cref="ExceptionType">Condition under which the exception is thrown</exception>
```

#### Примеры XML документации

##### Для классов
```csharp
/// <summary>
/// Represents a service for managing books in the library system.
/// Provides functional handling of CRUD operations using monadic patterns.
/// </summary>
public class BookService
```

##### Для методов
```csharp
/// <summary>
/// Gets the book by its identifier using functional error handling.
/// </summary>
/// <param name="bookId">The unique identifier of the book</param>
/// <returns>Either containing an error or the found book</returns>
/// <exception cref="ArgumentNullException">Thrown if bookId is null</exception>
public Either<BookNotFoundError, Book> GetBook(BookId bookId)
```

##### Для интерфейсов функционального программирования
```csharp
/// <summary>
/// Defines a functor — a container whose values can be mapped with functions.
/// This is a basic abstraction from category theory for functional programming.
/// </summary>
/// <typeparam name="T">The type of the value inside the functor</typeparam>
public interface IFunctor<T>
{
    /// <summary>
    /// Applies a function to the value inside the functor while preserving the container's structure.
    /// This is the primary functor operation, also known as fmap in category theory.
    /// </summary>
    /// <typeparam name="TResult">The result type of the function application</typeparam>
    /// <param name="func">Function to apply to the value</param>
    /// <returns>A new functor with the transformed value</returns>
    IFunctor<TResult> Map<TResult>(Func<T, TResult> func);
}
```

##### Для record типов
```csharp
/// <summary>
/// Represents an immutable identifier for a book.
/// Provides type safety and prevents confusion with other GUID values.
/// </summary>
/// <param name="Value">The unique identifier value</param>
public record BookId(Guid Value);
```

#### Специальные теги для функционального программирования
```csharp
/// <remarks>
/// This method follows the laws of functors:
/// 1. Identity preservation: Map(x => x) == the original functor
/// 2. Composition: Map(f).Map(g) == Map(x => g(f(x)))
/// </remarks>
```

## Примеры паттернов

### Неизменяемые типы данных
```csharp
public record Book(BookId Id, string Title, AuthorId AuthorId, DateTime PublishedAt);
public record BookId(Guid Value);
```

### Функциональная обработка ошибок
```csharp
public Either<Error, Book> GetBook(BookId id) =>
    bookRepository.Find(id)
        .ToEither(() => new BookNotFoundError(id));
```

### Использование функторов
```csharp
public Functor<string> ProcessBookTitle(Functor<Book> bookFunctor) =>
    bookFunctor.Map(book => book.Title.ToUpperInvariant());
```
