# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + PostgreSQL (`:5432`)

---

## Запуск сервера Backend (ЛР7 — каркас ASP.NET Core Web API)

Выполните из корня репозитория:

```bash
cd backend
dotnet restore
dotnet run
```

### Адреса и ограничения ЛР7 (обязательные сведения):
- **API:** `http://localhost:5000`
- **Swagger:** `http://localhost:5000/swagger` (или `http://localhost:5000/swagger/index.html` в среде `Development`)
- **Проверка живости (`GET /health`):** `http://localhost:5000/health` (возвращает `200 OK` и `Healthy`)
- **Сведения о сервисе (`GET /info`):** `http://localhost:5000/info` (имя приложения `TaskControl`, среда `Development` и пометка `ЛР7`)
- **Важные ограничения текущего этапа:**
  - данные заявок (поручений) пока в памяти, после перезапуска пропадут;
  - `/health` проверяет только живость процесса, а не создание заявки (не принимает `title`, `departmentId` / `siteId`, `description` и не сохраняет записи);
  - настоящее создание появится в ЛР8.

---

## Запуск клиента Frontend (ЛР3–ЛР6 — Vue 3 + Vite)

Выполните из корня репозитория в отдельном терминале:

```bash
cd frontend
cp -n .env.example .env
npm install
npm run dev
```

Открыть адрес Vite, обычно http://localhost:5173

---

## Дерево файлов репозитория

```text
RIS_Web/
├── README.md                       ← запуск backend и frontend, ограничения ЛР7
├── .gitignore                      ← исключения Git (node_modules/, .env, **/bin/, **/obj/)
├── docs/
│   ├── project-proposal.md         ← заявка на тему (ЛР1)
│   ├── lab-01-requirements.md      ← требования и критерии приёмки (ЛР1)
│   ├── architecture.md             ← схема из трёх узлов (ЛР2)
│   ├── data-model.md               ← ER-диаграмма, ключи и проверка 3НФ (ЛР2)
│   ├── api-contract.md             ← таблица API и примеры JSON (ЛР2)
│   ├── requirements-matrix.md      ← матрица: критерий → поле → запрос (ЛР2)
│   └── reports/
│       ├── lab-02.md               ← отчёт по ЛР2
│       ├── lab-03.md               ← отчёт по ЛР3
│       ├── lab-04.md               ← отчёт по ЛР4
│       ├── lab-05.md               ← отчёт по ЛР5
│       ├── lab-06.md               ← отчёт по ЛР6
│       └── lab-07.md               ← отчёт по ЛР7 (каркас ASP.NET Core Web API)
├── frontend/                       ← клиент Vue 3 + Vite (остаётся без изменений в ЛР7)
│   ├── .env.example
│   ├── package.json
│   └── src/
└── backend/                        ← сервер ASP.NET Core Web API (ЛР7)
    ├── backend.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── Properties/
    │   └── launchSettings.json     ← HTTP-порт 5000, среда Development
    ├── Controllers/
    │   └── SystemController.cs     ← контроллер со служебными маршрутами /health и /info
    ├── Services/
    │   ├── IAssignmentService.cs   ← интерфейс сервиса поручений
    │   └── AssignmentService.cs    ← сервис-заготовка, регистрируемый через DI
    ├── Models/
    │   ├── .gitkeep
    │   └── AssignmentModel.cs      ← модель-заготовка поручения
    └── Data/
        └── .gitkeep                ← заготовка под хранение данных
```

---

## Навигация по документации (`/docs`)

- [Заявка на тему (`docs/project-proposal.md`)](docs/project-proposal.md)
- [Требования к системе (`docs/lab-01-requirements.md`)](docs/lab-01-requirements.md)
- [Архитектура системы (`docs/architecture.md`)](docs/architecture.md)
- [Модель данных и проверка 3НФ (`docs/data-model.md`)](docs/data-model.md)
- [Контракт REST API (`docs/api-contract.md`)](docs/api-contract.md)
- [Матрица требований (`docs/requirements-matrix.md`)](docs/requirements-matrix.md)
- [Отчёт по ЛР2 (`docs/reports/lab-02.md`)](docs/reports/lab-02.md) · [Отчёт по ЛР3 (`docs/reports/lab-03.md`)](docs/reports/lab-03.md) · [Отчёт по ЛР4 (`docs/reports/lab-04.md`)](docs/reports/lab-04.md) · [Отчёт по ЛР5 (`docs/reports/lab-05.md`)](docs/reports/lab-05.md) · [Отчёт по ЛР6 (`docs/reports/lab-06.md`)](docs/reports/lab-06.md) · [Отчёт по ЛР7 (`docs/reports/lab-07.md`)](docs/reports/lab-07.md)
