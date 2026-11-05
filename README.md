# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + PostgreSQL (`:5432`)

---

## Запуск сервера Backend (ЛР8 — REST: создание, чтение, назначение и статус)

Выполните из корня репозитория:

```bash
cd backend
dotnet restore
dotnet run
```

*(Если `dotnet` установлен локально в `~/.dotnet`, используйте `~/.dotnet/dotnet restore` и `~/.dotnet/dotnet run`).*

### Адреса и ограничения ЛР8 (обязательные сведения):
- **Базовый адрес API:** `http://localhost:5000`
- **Swagger UI:** `http://localhost:5000/swagger` (в среде `Development`)
- **Проверка живости (`GET /health`):** `http://localhost:5000/health` (`200 OK`, `Healthy` — проверяет только живость процесса, а не создание записи)
- **Сведения о сервисе (`GET /info`):** `http://localhost:5000/info`
- **Маршруты главного объекта (`Assignments` / Поручения):**
  - `POST /api/assignments` — создать поручение (`201 Created`, сервер сам назначает `id`, `number`, `status: "New"`, `assigneeUserId: null`; при ошибке проверки полей — `400 Bad Request`)
  - `GET /api/assignments` — получить список поручений (`200 OK`, включая пустой массив `[]`)
  - `GET /api/assignments/{id}` — получить одно поручение (`200 OK` или `404 Not Found`)
  - `PATCH /api/assignments/{id}/assignee` — назначить исполнителя (`{ "assigneeUserId": 5 }` → `200 OK`, `400` или `404`)
  - `PATCH /api/assignments/{id}/status` — изменить статус (`{ "status": "InProgress" }`, допустимые статусы: `New`, `InProgress`, `Closed`, `Cancelled` → `200 OK`, `400`, `404` или `409 Conflict` при переводе в `InProgress` без назначенного исполнителя)
- **Важные ограничения текущего этапа (ЛР8):**
  - данные поручений пока хранятся в оперативной памяти сервера (`AssignmentService`), после перезапуска процесса они пропадут;
  - метод `DELETE` в курсе не используется (отмена поручения выполняется переводом в статус `Cancelled`);
  - при неожиданном сбое (`500`) сервер пишет краткую строку в лог (`CreateAssignment failed; departmentId=...; error=...`), а клиенту возвращает безопасный `ProblemDetails` без `stack trace`.

---

## Коллекция запросов Postman (ЛР8)

Готовая коллекция с проверками `201`, `400`, `404`, `409` и обоих `PATCH` находится в репозитории:
- `docs/api-collection/assignments.postman_collection.json`
- `docs/api-collection/tickets.postman_collection.json`

---

## Запуск клиента Frontend (ЛР3–ЛР6 — Vue 3 + Vite)

Выполните из корня репозитория в отдельном терминале:

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
├── README.md                                           ← запуск backend и frontend, маршруты и ограничения ЛР8
├── .gitignore                                          ← исключения Git (node_modules/, .env, **/bin/, **/obj/)
├── docs/
│   ├── project-proposal.md                             ← заявка на тему (ЛР1)
│   ├── lab-01-requirements.md                          ← требования и критерии приёмки (ЛР1)
│   ├── architecture.md                                 ← схема из трёх узлов (ЛР2)
│   ├── data-model.md                                   ← ER-диаграмма, ключи и проверка 3НФ (ЛР2)
│   ├── api-contract.md                                 ← таблица API и примеры JSON (ЛР2)
│   ├── requirements-matrix.md                          ← матрица: критерий → поле → запрос (ЛР2)
│   ├── api-collection/
│   │   ├── assignments.postman_collection.json         ← коллекция Postman для проверки REST API (ЛР8)
│   │   └── tickets.postman_collection.json             ← копия коллекции со стандартным именем из методички
│   └── reports/
│       ├── lab-02.md .. lab-07.md                      ← отчёты по ЛР2–ЛР7
│       └── lab-08.md                                   ← отчёт по ЛР8 (REST, коды 201/400/404/409 и лог ошибок)
├── frontend/                                           ← клиент Vue 3 + Vite
└── backend/                                            ← сервер ASP.NET Core Web API (ЛР7–ЛР8)
    ├── backend.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── Properties/
    │   └── launchSettings.json                         ← HTTP-порт 5000, среда Development
    ├── Controllers/
    │   ├── SystemController.cs                         ← служебные маршруты /health и /info (ЛР7)
    │   └── AssignmentsController.cs                    ← REST-маршруты главного объекта (ЛР8)
    ├── Dtos/
    │   ├── CreateAssignmentRequest.cs                  ← входной DTO создания (title, departmentId/siteId, description)
    │   ├── AssignAssigneeRequest.cs                    ← входной DTO назначения исполнителя (assigneeUserId)
    │   ├── ChangeStatusRequest.cs                      ← входной DTO смены статуса (status)
    │   └── AssignmentResponse.cs                       ← выходной DTO поручения
    ├── Models/
    │   └── Assignment.cs                               ← серверная модель поручения
    ├── Services/
    │   ├── IAssignmentService.cs                       ← интерфейс сервиса поручений и результаты операций
    │   └── AssignmentService.cs                        ← хранение в памяти, счётчик и правила процесса
    └── Data/
        └── .gitkeep                                    ← заготовка под контекст БД для следующих ЛР
```
