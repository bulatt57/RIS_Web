# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + PostgreSQL (`:5432`)

---

## База данных PostgreSQL и SQL-скрипты (ЛР9)

В папке `db/` расположены SQL-скрипты реляционной схемы в 3НФ (без хранения паролей и секретов в Git):
- `db/schema.sql` — пересоздание учебных таблиц (`departments`, `users`, `assignments`), первичные и внешние ключи (`PK`, `FK`), ограничение `CHECK` на 4 статуса (`New`, `InProgress`, `Closed`, `Cancelled`) и индексы (`assignments_status_idx`, `assignments_department_id_idx`);
- `db/seed.sql` — наполнение справочника подразделений ЕЦУиМС, пользователей и поручений во всех 4 статусах;
- `db/queries.sql` — обязательные запросы с фильтрацией и сортировкой (`WHERE` + `ORDER BY`), соединением таблиц (`JOIN`) и группировкой (`GROUP BY` + `COUNT(*)`).

### Команды запуска через `psql` (локально на Debian / Linux):

```bash
createdb course_assignments
psql -d course_assignments -f db/schema.sql
psql -d course_assignments -f db/seed.sql
psql -d course_assignments -f db/queries.sql
```

*(Пароль пользователя БД вводится интерактивно или задаётся локально в окружении — в репозитории пароли не хранятся. Для повторного прогона достаточно заново выполнить `psql -d course_assignments -f db/schema.sql` и `seed.sql`, так как в начале `schema.sql` предусмотрен `DROP TABLE IF EXISTS ... CASCADE`).*

---

## Запуск сервера Backend (ЛР7–ЛР8 — ASP.NET Core Web API)

Выполните из корня репозитория:

```bash
cd backend
dotnet restore
dotnet run
```

*(Если `dotnet` установлен локально в `~/.dotnet`, используйте `~/.dotnet/dotnet restore` и `~/.dotnet/dotnet run`).*

- **Базовый адрес API:** `http://localhost:5000`
- **Swagger UI:** `http://localhost:5000/swagger`
- **Проверка живости (`GET /health`):** `http://localhost:5000/health` (`200 OK`, `Healthy`)
- **Сведения о сервисе (`GET /info`):** `http://localhost:5000/info`
- **Маршруты главного объекта (`Assignments` / Поручения):**
  - `POST /api/assignments` (`201 Created` / `400 Bad Request`)
  - `GET /api/assignments` (`200 OK`, включая `[]`)
  - `GET /api/assignments/{id}` (`200 OK` / `404 Not Found`)
  - `PATCH /api/assignments/{id}/assignee` (`200 OK` / `400` / `404`)
  - `PATCH /api/assignments/{id}/status` (`200 OK` / `400` / `404` / `409 Conflict` при переводе в `InProgress` без исполнителя)

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
├── README.md                                           ← запуск БД (ЛР9), backend (ЛР7–8) и frontend (ЛР3–6)
├── .gitignore                                          ← исключения Git (node_modules/, .env, **/bin/, **/obj/)
├── db/
│   ├── schema.sql                                      ← таблицы departments, users, assignments, PK, FK, CHECK, индексы (ЛР9)
│   ├── seed.sql                                        ← начальные данные: 4 подразделения, 4 пользователя, 5 поручений во всех 4 статусах (ЛР9)
│   └── queries.sql                                     ← обязательные запросы: фильтр/сортировка, JOIN, GROUP BY и тест CHECK (ЛР9)
├── docs/
│   ├── project-proposal.md                             ← заявка на тему (ЛР1)
│   ├── lab-01-requirements.md                          ← требования и критерии приёмки (ЛР1)
│   ├── architecture.md                                 ← схема из трёх узлов (ЛР2)
│   ├── data-model.md                                   ← ER-диаграмма, ключи и проверка 3НФ (ЛР2)
│   ├── api-contract.md                                 ← таблица API и примеры JSON (ЛР2)
│   ├── requirements-matrix.md                          ← матрица: критерий → поле → запрос (ЛР2)
│   ├── api-collection/
│   │   ├── assignments.postman_collection.json         ← коллекция Postman для проверки REST API (ЛР8)
│   │   └── tickets.postman_collection.json
│   └── reports/
│       ├── lab-02.md .. lab-08.md                      ← отчёты по ЛР2–ЛР8
│       └── lab-09.md                                   ← отчёт по ЛР9 (PostgreSQL и SQL)
├── frontend/                                           ← клиент Vue 3 + Vite
└── backend/                                            ← сервер ASP.NET Core Web API
```
