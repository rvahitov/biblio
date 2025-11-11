# Biblio

## Описание

Biblio — модульная библиотечная система на C# (.NET), использующая элементы функционального программирования и теорию категорий. Проект состоит из нескольких библиотек, веб‑API и тестов. Основная цель репозитория — предоставить платформу для работы с цитированием, доменными моделями книг и инфраструктурой сервисов.

## Ключевые особенности

- Архитектура на .NET (C# 13+)
- Функциональные паттерны (Either, Option, Monad-подходы)
- Модульная структура: домены, общие библиотеки, веб API и тесты
- Unit-тесты с xUnit

## Структура репозитория

- `Biblio.sln` — решение .NET для проекта
- `src/` — исходники
  - `Biblio.Aspire/` — приложение/host
  - `Biblio.Citations.Domain/` — доменные модели и логика для цитат и книг
  - `Biblio.Citations.WebApi/` — Web API для цитат
  - `Biblio.Common/` и `Biblio.Common.Akka/` — общие библиотеки и Akka-помощники
  - `ServiceDefaults/` — утилиты и настройки по умолчанию
- `tests/` — модульные тесты по проектам

## Быстрый старт (требования)

- .NET SDK (версия, совместимая с проектом — используйте ту, что указана в `global.json`, если есть)
- Рекомендуется использовать PowerShell (pwsh) на Windows
- Visual Studio / VS Code для работы с задачами и отладкой

## Как собрать проект

В корне репозитория (Windows PowerShell):

```powershell
# Восстановить зависимости
dotnet restore .\Biblio.sln

# Собрать решение (Debug)
dotnet build .\Biblio.sln -c Debug
```

В VS Code доступны задачи (Tasks) для восстановления и сборки (см. `Tasks`):
- "Restore Solution"
- "Build Solution"

## Как запустить тесты

```powershell
# Запустить все тесты для решения
dotnet test .\Biblio.sln -c Debug
```

В VS Code доступна задача "Run All Tests (solution)" для удобного запуска.

## Конвенции разработки

- Сообщения коммитов должны быть на английском языке.
- Формат коммит‑сообщения:
  - Краткий заголовок (до 80 символов)
  - Одна пустая строка
  - Подробное описание изменений

Примеры:

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

## Тестирование и качество

- Unit-тесты используют xUnit.
- Моки реализуются через FakeItEasy (где применимо).
- Перед открытием pull request запустите тесты и убедитесь, что сборка проходит.

## Полезные советы

- Работайте в feature-ветках: `feature/...`, `fix/...`, `refactor/...`.
- Поддерживайте ветки небольшими и фокусированными.
- Ребейзьте с `main` по мере необходимости для уменьшения конфликтов.

## Контракт для разработчиков (коротко)

- Входы: PR/коммиты в feature-ветках
- Выходы: корректно собранное решение, зеленые тесты
- Ошибки: сборка или провал тестов

## Лицензия

Уточните лицензию/правовую информацию в репозитории (если требуется, добавьте файл `LICENSE`).

## Контакты

Если нужно — откройте issue или свяжитесь с авторами/maintainers репозитория.

---

README сгенерирован автоматически. Если нужно добавить секции (архитектура, диаграммы, документация API), сообщите — добавлю. 
