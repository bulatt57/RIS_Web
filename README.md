# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + PostgreSQL (`:5432`)

---

## Запуск клиента (ЛР3 — Vue 3 + Vite)

Выполните из корня репозитория:

```bash
cd frontend
npm install
npm run dev
```

Открыть адрес Vite, обычно http://localhost:5173

На экране доступны три переключаемых режима:
1. **Список поручений** — таблица `<table>` с тестовыми записями `Assignment` (`id`, `number`, `title`, `departmentId`, `status`).
2. **Создание поручения** — семантическая форма `<form>` с полями `title`, `departmentId`, `description` и связанными `<label>`.
3. **Вход** — семантическая форма `<form>` с полями `login` и `password`.

---

## Дерево файлов репозитория

```text
RIS_Web/
├── README.md                     ← описание проекта и команды запуска клиента
├── .gitignore                    ← исключения Git (node_modules/, dist/, .DS_Store)
├── docs/
│   ├── project-proposal.md       ← заявка на тему (ЛР1)
│   ├── lab-01-requirements.md    ← требования и критерии приёмки (ЛР1)
│   ├── architecture.md           ← схема из трёх узлов (ЛР2)
│   ├── data-model.md             ← ER-диаграмма, ключи и проверка 3НФ (ЛР2)
│   ├── api-contract.md           ← таблица API и примеры JSON (ЛР2)
│   ├── requirements-matrix.md    ← матрица: критерий → поле → запрос (ЛР2)
│   └── reports/
│       ├── lab-02.md             ← отчёт по ЛР2
│       └── lab-03.md             ← отчёт по ЛР3
└── frontend/
    ├── package.json
    ├── package-lock.json
    ├── index.html
    ├── vite.config.js
    ├── src/
    │   ├── main.js
    │   ├── App.vue
    │   └── assets/
    └── public/
```

---

## Навигация по документации (`/docs`)

### Лабораторная работа №1 — Требования и заявка на тему
- [Заявка на тему (`docs/project-proposal.md`)](docs/project-proposal.md)
- [Требования к системе (`docs/lab-01-requirements.md`)](docs/lab-01-requirements.md)

### Лабораторная работа №2 — Архитектура и модель данных
- [Архитектура системы (`docs/architecture.md`)](docs/architecture.md)
- [Модель данных и проверка 3НФ (`docs/data-model.md`)](docs/data-model.md)
- [Контракт REST API (`docs/api-contract.md`)](docs/api-contract.md)
- [Матрица требований (`docs/requirements-matrix.md`)](docs/requirements-matrix.md)
- [Отчёт по лабораторной работе №2 (`docs/reports/lab-02.md`)](docs/reports/lab-02.md)

### Лабораторная работа №3 — Vue 3: каркас и три экрана
- [Отчёт по лабораторной работе №3 (`docs/reports/lab-03.md`)](docs/reports/lab-03.md)
