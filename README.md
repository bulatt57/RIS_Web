# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + PostgreSQL (`:5432`)

---

## Дерево файлов репозитория

```text
RIS_Web/
├── README.md
└── docs/
    ├── project-proposal.md         ← Заявка на тему (ЛР1)
    ├── lab-01-requirements.md      ← Требования и критерии приёмки (ЛР1)
    ├── architecture.md             ← Схема из трёх узлов (ЛР2)
    ├── data-model.md               ← ER-диаграмма, ключи и проверка 3НФ (ЛР2)
    ├── api-contract.md             ← Таблица API и примеры JSON (ЛР2)
    ├── requirements-matrix.md      ← Матрица: критерий → поле → запрос (ЛР2)
    └── reports/
        └── lab-02.md               ← Отчёт по ЛР2 и ответы к защите
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

---

## Архитектура и воспроизводимый запуск (контур семестра)

Система строится по трёхузловой архитектуре (подробнее в [docs/architecture.md](docs/architecture.md)):
1. **Клиент (`Vue 3 + Vite`, порт `5173`)** — отрисовывает интерфейс и отправляет HTTP-запросы к API.
2. **Сервер (`ASP.NET Core Web API`, порт `5000`)** — проверяет права ролей (`Manager`, `Executor`, `Viewer`), валидирует данные и обращается к БД.
3. **База данных (`PostgreSQL`, порт `5432`)** — хранит таблицы `Departments`, `Users` и `Assignments`.
