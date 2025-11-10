---
applyTo: '**/*.cs'
---
## Контекст проекта
Проект Biblio - это библиотечная система на C# .NET, использующая функциональное программирование и теорию категорий.

## Архитектура
- **Языки**: C# 13+, .NET
- **Парадигма**: Функциональное программирование
- **Структура**: Модульная архитектура с разделением на библиотеки
- **Тестирование**: Unit тесты с xUnit, моки с FakeItEasy

## Структура проекта
```
src/
├── common/
│   ├── Biblio.Common/     # Общие сообщения и интерфейсы
│   └── Cats/              # Библиотека теории категорий
├── services/              # Бизнес-сервисы
└── api/                   # API слой
```

#### Современные возможности xUnit v3
```csharp
// Использование современного синтаксиса C# в тестах
public class ModernTestExample
{
    [Fact]
    public void Should_UseRecordTypes_When_Testing()
    {
        // Arrange
        var bookData = new BookRequest("Title", "Author");
        
        // Act & Assert
        bookData.Should().Match<BookRequest>(x => 
            x.Title == "Title" && x.Author == "Author");
    }

    // Тестирование с использованием pattern matching
    [Theory]
    [InlineData(BookStatus.Available)]
    [InlineData(BookStatus.Reserved)]
    public void Should_HandleBookStatus_When_ValidStatus(BookStatus status)
    {
        // Act & Assert
        var result = status switch
        {
            BookStatus.Available => true,
            BookStatus.Reserved => true,
            _ => false
        };
        
        result.Should().BeTrue();
    }
}
```

## Стиль кодирования

### Общие принципы
- Используй неизменяемые типы данных (immutable)
- Предпочитай функциональные конструкции
- Избегай null, используй Option/Maybe паттерны
- Применяй принципы SOLID и функционального программирования
- Новые классы и записи должны быть sealed

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

### Collection expressions (выражения коллекций)

- Рекомендуется использовать выражения коллекций для краткой и читаемой записи литералов коллекций, когда проект таргетит C# 12/13+.
- Они особенно удобны в тестах и при создании immutable данных: сокращают шум от `new[] { ... }` или `new List<T> { ... }`.
- Используйте их для литералов массивов и списков, но не злоупотребляйте в местах с сложной инициализацией или побочными эффектами.

Примеры:
```csharp
// Array literal
int[] ids = [1, 2, 3];

// String array (tests)
string[] abbreviations = ["Gen", "Gn"];

// List<T> можно записать лаконично и затем при необходимости преобразовать
var list = [1, 2, 3].ToList();
```

Причины для использования:
- Улучшает читаемость тестов и фикстур
- Снижает визуальный шум при создании небольших неизменяемых коллекций
- Совместимо с современным C# и соответствует функциональному стилю проекта

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

## Зависимости и пакеты

### Основные библиотеки
- **LanguageExt.Core** - функциональное программирование и теория категорий
- **xUnit v3** - современный тестовый фреймворк
- **FakeItEasy** - библиотека моков
- **FluentAssertions** - читаемые утверждения в тестах

### Принципы использования зависимостей
- Минимизируй внешние зависимости
- Используй встроенные возможности .NET где возможно
- Избегай reflection и dynamic код
- Предпочитай композицию dependency injection

### Управление пакетами через Directory.Packages.props
Проект использует централизованное управление пакетами. **ОБЯЗАТЕЛЬНО** следуй этому workflow:

#### Добавление нового NuGet пакета
```bash
# 1. СНАЧАЛА добавь пакет в Directory.Packages.props
# Отредактируй файл и добавь новый PackageVersion:
# <PackageVersion Include="NewPackage.Name" Version="1.2.3" />

# 2. ЗАТЕМ добавь PackageReference в нужный .csproj БЕЗ указания версии
dotnet add package NewPackage.Name --no-version

# Или добавь вручную в .csproj:
# <PackageReference Include="NewPackage.Name" />
```

#### Принципы работы с Directory.Packages.props
- ✅ **Все версии пакетов** управляются централизованно в `Directory.Packages.props`
- ✅ **В .csproj файлах** указывай только `<PackageReference Include="PackageName" />` БЕЗ версии
- ❌ **НИКОГДА не указывай** Version в PackageReference в проектах
- 🔄 **При обновлении** версии пакета - меняй только в `Directory.Packages.props`
- 📦 **Для тестовых проектов** также используй централизованное управление

#### Пример правильной структуры
**Directory.Packages.props:**
```xml
<Project>
    <PropertyGroup>
        <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
  
    <ItemGroup>
        <PackageVersion Include="xunit" Version="2.4.2" />
        <PackageVersion Include="FakeItEasy" Version="8.0.0" />
        <PackageVersion Include="FluentAssertions" Version="6.12.0" />
    </ItemGroup>
</Project>
```

**MyProject.csproj:**
```xml
<ItemGroup>
    <PackageReference Include="xunit" />
    <PackageReference Include="FakeItEasy" />
    <PackageReference Include="FluentAssertions" />
</ItemGroup>
```

## Производительность
- Используй Span<T> и Memory<T> для работы с памятью
- Предпочитай ValueTask для асинхронных операций
- Избегай boxing примитивных типов
- Применяй ленивые вычисления где уместно

## Безопасность
- Валидируй все входные параметры
- Используй типобезопасные идентификаторы (например, BookId вместо Guid)
- Не логируй чувствительную информацию
- Применяй принцип наименьших привилегий

## Тестирование

### Фреймворки
- **xUnit** - основной тестовый фреймворк (новая версия)
- **FakeItEasy** - библиотека для создания моков и стабов
- **FluentAssertions** - для читаемых утверждений

### Принципы тестирования
- Пиши unit тесты для всех публичных методов
- Используй описательные имена тестов: `Should_ReturnBook_When_ValidIdProvided`
- Применяй паттерн AAA (Arrange, Act, Assert)
- Тестируй граничные случаи и обработку ошибок
- Изолируй зависимости с помощью моков

### Примеры тестов

#### Базовый тест с xUnit
```csharp
public class BookServiceTests
{
    [Fact]
    public void Should_ReturnBook_When_ValidIdProvided()
    {
        // Arrange
        var bookId = new BookId(Guid.NewGuid());
        var expectedBook = new Book(bookId, "Test Book", new AuthorId(Guid.NewGuid()), DateTime.Now);
        var repository = A.Fake<IBookRepository>();
        A.CallTo(() => repository.Find(bookId)).Returns(expectedBook);
        var service = new BookService(repository);

        // Act
        var result = service.GetBook(bookId);

        // Assert
        result.Should().BeRight().Which.Should().Be(expectedBook);
    }
}
```

#### Тест с параметрами
```csharp
[Theory]
[InlineData("")]
[InlineData(null)]
[InlineData("   ")]
public void Should_ThrowException_When_InvalidTitleProvided(string invalidTitle)
{
    // Arrange & Act & Assert
    var action = () => new Book(new BookId(Guid.NewGuid()), invalidTitle, new AuthorId(Guid.NewGuid()), DateTime.Now);
    action.Should().Throw<ArgumentException>();
}
```

#### Тест асинхронного метода
```csharp
[Fact]
public async Task Should_CreateBook_When_ValidDataProvided()
{
    // Arrange
    var repository = A.Fake<IBookRepository>();
    var service = new BookService(repository);
    var bookData = new CreateBookCommand("New Book", Guid.NewGuid());

    // Act
    var result = await service.CreateBookAsync(bookData);

    // Assert
    result.Should().BeRight();
    A.CallTo(() => repository.SaveAsync(A<Book>._)).MustHaveHappenedOnceExactly();
}
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
