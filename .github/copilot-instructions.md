# Copilot Instructions для проекта Biblio

## Контекст проекта
Проект Biblio - это библиотечная система на C# .NET, использующая функциональное программирование и теорию категорий.

## Архитектура
- **Языки**: C# 13+, .NET
- **Парадигма**: Функциональное программирование
- **Структура**: Модульная архитектура с разделением на библиотеки
- **Тестирование**: Unit тесты с xUnit, моки с FakeItEasy

## Правила для commit-сообщений

### Формат
```
короткий заголовок

детальное описание
```

### Принципы
- **Заголовок**: краткое описание изменений (до 80 символов)
- **Описание**: подробное объяснение что и зачем было изменено
- Между заголовком и описанием должна быть пустая строка
- Используй повелительное наклонение в заголовке ("добавь", "исправь", "обнови")
- **Язык**: сообщения коммитов должны быть написаны на английском языке. / Commit messages must be written in English.

### Примеры
```
add the IFunctor interface for category theory

Implemented a basic IFunctor interface with a Map method for applying
functions to values in a context. This is the foundation for building
functional abstractions in the Cats library.
```

```
fix error handling in BookService

Replaced throwing exceptions with using Either<Error, T> for functional error handling. This makes the code more predictable and aligns with the principles of functional programming.
```

## Работа с Git

### Базовые команды
- **Проверка статуса**: `git status` - показывает состояние рабочей директории и индекса
- **Коммит изменений**: `git commit` - создает коммит с staged файлами

### Workflow для "закоммитить staged files"
```bash
# 1. Проверить статус репозитория
git status

# 2. Закоммитить staged файлы
git commit -m "краткий заголовок

детальное описание изменений"
```

### Типичные сценарии
- Если есть staged файлы - используй `git commit` с осмысленным сообщением
- Если нет staged файлов - сначала добавь нужные файлы через `git add`
- Всегда проверяй `git status` перед коммитом для понимания текущего состояния

## Работа с фичами (Feature Branches)

### Принципы работы с ветками
- Создавай отдельную ветку для каждой фичи или задачи
- Используй описательные имена веток: `feature/add-book-service`, `fix/null-reference-error`
- Работай в короткоживущих ветках (1-3 дня)
- Регулярно синхронизируйся с основной веткой

### Базовые команды для веток
```bash
# Создать и переключиться на новую ветку
git checkout -b feature/название-фичи

# Посмотреть все ветки
git branch -a

# Переключиться на существующую ветку
git checkout название-ветки

# Удалить локальную ветку
git branch -d название-ветки
```

### Workflow для фичи
```bash
# 1. Создать ветку для фичи
git checkout -b feature/my-new-feature

# 2. Работать над фичей (коммиты)
git add .
git commit -m "краткое описание изменений"

# 3. Отправить ветку в удаленный репозиторий
git push -u origin feature/my-new-feature

# 4. После завершения - слияние через Pull Request
# 5. Удалить локальную ветку после слияния
git checkout main
git branch -d feature/my-new-feature
```

### Именование веток
- **feature/**: новые функции (`feature/user-authentication`)
- **fix/**: исправления багов (`fix/login-validation`)
- **refactor/**: рефакторинг кода (`refactor/extract-book-service`)
- **docs/**: обновление документации (`docs/api-endpoints`)
- **test/**: добавление тестов (`test/book-service-unit-tests`)

### Лучшие практики
- Делай небольшие, сфокусированные коммиты в ветке
- Регулярно делай rebase с main для актуальности
- Тестируй код перед созданием Pull Request
- Используй осмысленные названия веток и коммитов


Часть инструкций по разработке на C# перемещена в `.github/instructions/csharp.instructions.md`.
Пожалуйста, смотрите этот файл как источник истинных инструкций для C# разработки, тестирования и управления пакетами.
 (Этот файл оставлен для общей информации и метаданных; подробные практики разработки C# теперь находятся в `csharp.instructions.md`.)
