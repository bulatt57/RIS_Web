# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + EF Core + PostgreSQL (`:5432`)

---

## Подключение Backend к PostgreSQL через EF Core (ЛР10)

В ЛР10 сервер `ASP.NET Core` переведён с хранения в оперативной памяти на работу с СУБД `PostgreSQL` через `Entity Framework Core` (`AppDbContext` и миграция `InitialCreate`).

### 1. Настройка строки подключения через `dotnet user-secrets` (пароль не хранится в Git)

```bash
cd backend
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default"   "Host=localhost;Port=5432;Database=course_assignments;Username=postgres;Password=ВАШ_ПАРОЛЬ"
```

Шаблон без секрета сохранён в `backend/appsettings.example.json`.

### 2. Применение миграций EF Core и запуск сервера

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

*(При запуске `dotnet run` сервер также автоматически проверяет применение миграций и при пустой БД заполняет стартовые справочники `departments`, `users` и начальные поручения `assignments`).*

- **Базовый адрес API:** `http://localhost:5000`
- **Swagger UI:** `http://localhost:5000/swagger`
- **Маршруты главного объекта (`Assignments` / Поручения в PostgreSQL):**
  - `POST /api/assignments` — `201 Created`, сохраняет запись в таблицу `assignments` БД PostgreSQL
  - `GET /api/assignments` — `200 OK`, читает список из БД PostgreSQL
  - `GET /api/assignments/{id}` — `200 OK` или `404 Not Found` (из БД PostgreSQL)
  - `PATCH /api/assignments/{id}/assignee` — `200 OK`, сохраняет `assignee_user_id` в БД
  - `PATCH /api/assignments/{id}/status` — `200 OK` или `409 Conflict` (если `InProgress` без исполнителя), сохраняет `status` в БД

---

## База данных PostgreSQL и SQL-скрипты (ЛР9)

В папке `db/` расположены SQL-скрипты реляционной схемы в 3НФ:
- `db/schema.sql` — таблицы `departments`, `users`, `assignments`, `PK`, `FK`, `CHECK` и индексы;
- `db/seed.sql` — начальные данные;
- `db/queries.sql` — запросы с фильтрацией, сортировкой, `JOIN` и `GROUP BY`.

---

## Запуск клиента Frontend (ЛР3–ЛР6 — Vue 3 + Vite)

```bash
cd frontend
cp -n .env.example .env
npm install
npm run dev
```

Открыть адрес Vite, обычно `http://localhost:5173`.

---

## Дерево файлов репозитория

```text
RIS_Web/
├── README.md                                           ← инструкции по EF Core (ЛР10), SQL (ЛР9), API и Vue
├── .gitignore                                          ← исключения Git (node_modules/, .env, **/bin/, **/obj/, локальные секреты)
├── backend/
│   ├── backend.csproj                                  ← пакеты Npgsql.EntityFrameworkCore.PostgreSQL и Design
│   ├── Program.cs                                      ← регистрация AppDbContext по ключу ConnectionStrings:Default
│   ├── appsettings.json                                ← без пароля (секрет задаётся через user-secrets)
│   ├── appsettings.example.json                        ← пример конфигурации подключения без реального пароля
│   ├── Data/
│   │   ├── AppDbContext.cs                             ← DbContext (Departments, Users, Assignments и Fluent API под schema.sql)
│   │   └── DbSeeder.cs                                 ← начальное заполнение справочников, если база пуста
│   ├── Models/
│   │   ├── Department.cs                               ← модель подразделения (departments)
│   │   ├── User.cs                                     ← модель пользователя (users)
│   │   └── Assignment.cs                               ← модель поручения (assignments)
│   ├── Migrations/
│   │   ├── 20261113180000_InitialCreate.cs             ← миграция создания таблиц, ключей, CHECK и индексов
│   │   ├── 20261113180000_InitialCreate.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   ├── Controllers/
│   │   ├── SystemController.cs
│   │   └── AssignmentsController.cs
│   ├── Dtos/
│   └── Services/
│       ├── IAssignmentService.cs
│       └── AssignmentService.cs                        ← сервис переведён с памяти на AppDbContext (EF Core)
├── db/
│   ├── schema.sql
│   ├── seed.sql
│   └── queries.sql
├── docs/
│   ├── api-collection/
│   └── reports/
│       ├── lab-02.md .. lab-09.md
│       └── lab-10.md                                   ← отчёт по ЛР10 (EF Core, миграции и рестарт-тест)
└── frontend/
```
