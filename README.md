# MeetingRoom API

REST API для управління конференц-залами, бронюваннями, додатковими послугами та розрахунком вартості оренди.

# Технології

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Swagger
- Docker

# Архітектура

Проєкт побудований на **N-Layer Architecture**:

- `MeetingRoom.Domain` — сутності та інтерфейси;
- `MeetingRoom.Application` — DTO, сервіси, валідатори та бізнес-логіка;
- `MeetingRoom.Infrastructure` — EF Core, PostgreSQL і репозиторії;
- `MeetingRoom.WebApi` — controllers, middleware і Swagger.

Для доступу до даних використовується **Repository Pattern**.

## Валідація

Для перевірки вхідних даних використовується **FluentValidation**.

## Основні можливості

- створення, редагування та видалення залів;
- пошук доступних залів;
- створення та скасування бронювань;
- перевірка конфліктів бронювання;
- розрахунок вартості оренди;
- додавання послуг;
- звіти про дохід, популярні послуги та використання залів.

## Розрахунок вартості

Вартість оренди залежить від часу бронювання. У ранкові години з 06:00 до 09:00 діє знижка 10%. У стандартні години з 09:00 до 18:00 використовується базова вартість залу. У пікові години з 12:00 до 14:00 застосовується націнка 15%. У вечірні години з 18:00 до 23:00 діє знижка 20%.

Загальна вартість бронювання складається з вартості оренди залу та вартості вибраних додаткових послуг.

# API endpoints

Для роботи із залами доступні такі запити:

GET /api/rooms — отримати список усіх залів.

GET /api/rooms/{id} — отримати інформацію про зал за його ID.

POST /api/rooms — створити новий зал.

PUT /api/rooms/{id} — оновити інформацію про зал.

DELETE /api/rooms/{id} — видалити зал.

GET /api/rooms/available — знайти доступні зали за датою, часом і місткістю.

## Для роботи з бронюваннями:

POST /api/roombookings — створити нове бронювання.

GET /api/roombookings — отримати список усіх бронювань.

GET /api/roombookings/{id} — отримати бронювання за ID.

PATCH /api/roombookings/{id}/cancel — скасувати бронювання.

## Для звітів:

GET /api/reports/revenue — отримати звіт про дохід.

GET /api/reports/popular-options — отримати інформацію про найпопулярніші послуги.

GET /api/reports/room-usage — отримати статистику використання залів.

# Тестові дані

У системі можна використовувати такі тестові зали:

Зал А — місткість 50 осіб, вартість оренди 2000 гривень за годину.

Зал B — місткість 100 осіб, вартість оренди 3500 гривень за годину.

Зал C — місткість 30 осіб, вартість оренди 1500 гривень за годину.

Доступні додаткові послуги:

Проєктор — 500 гривень.

Wi-Fi — 300 гривень.

Звук — 700 гривень.

# Запуск

## 1. Запустити PostgreSQL у Docker

```powershell
docker run --name meetingroom-postgres `
  -e POSTGRES_DB=MeetingRoomDb `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=postgres `
  -p 5432:5432 `
  -d postgres:16
```

## 2. Налаштувати connection string

`MeetingRoom.WebApi/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "*"
  }
}
```

## 3. Відновити пакети

```powershell
dotnet restore
```

## 4. Створити міграцію

```powershell
dotnet ef migrations add InitialCreate --project MeetingRoom.Infrastructure --startup-project MeetingRoom.WebApi --context RoomDbContext
```

## 5. Оновити базу

```powershell
dotnet ef database update --project MeetingRoom.Infrastructure --startup-project MeetingRoom.WebApi --context RoomDbContext
```

## 6. Запустити API

```powershell
dotnet run --project MeetingRoom.WebApi
```

## Swagger

Після запуску:

```text
https://localhost:<port>/swagger
```

# Тестові сценарії
## Приклад створення залу

```json
{
  "name": "Зал А",
  "capacity": 50,
  "pricePerHour": 2000,
  "options": [
    {
      "name": "Проєктор",
      "price": 500
    },
    {
      "name": "Wi-Fi",
      "price": 300
    }
  ]
}
```

## Приклад бронювання

```json
{
  "roomId": "00000000-0000-0000-0000-000000000001",
  "startTime": "2026-09-01T10:00:00",
  "endTime": "2026-09-01T14:00:00",
  "selectedOptionIds": [
    "00000000-0000-0000-0000-000000000101"
  ]
}
```


## Некоректне створення залу

```json
{
  "name": "",
  "capacity": 0,
  "pricePerHour": -500,
  "options": [
    {
      "name": "",
      "price": -100
    }
  ]
}
```

Очікуваний результат:

```text
400 Bad Request
```

Цей запит перевіряє FluentValidation для назви залу, місткості, вартості оренди та додаткових послуг.

## Некоректний час бронювання

```json
{
  "roomId": "00000000-0000-0000-0000-000000000001",
  "startTime": "2026-09-01T14:00:00",
  "endTime": "2026-09-01T10:00:00",
  "selectedOptionIds": []
}
```

Очікуваний результат:

```text
400 Bad Request
```

Цей запит перевіряє, що час завершення бронювання пізніший за час початку.

## Бронювання неіснуючого залу

```json
{
  "roomId": "99999999-9999-9999-9999-999999999999",
  "startTime": "2026-09-01T10:00:00",
  "endTime": "2026-09-01T12:00:00",
  "selectedOptionIds": []
}
```

Очікувана відповідь:

```json
{
  "statusCode": 400,
  "message": "Meeting room not found."
}
```

Цей сценарій демонструє роботу глобального middleware.

## Конфлікт бронювання

Спочатку потрібно створити бронювання на період з `10:00` до `14:00`.

Після цього повторно надіслати запит для того самого залу на період з `12:00` до `15:00`.

Очікувана відповідь:

```json
{
  "statusCode": 400,
  "message": "Meeting room is already booked for this time."
}
```
