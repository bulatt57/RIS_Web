# Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl)

- **Студент:** Тухбатуллин Булат Айдарович
- **Группа:** ТРИС-1-23
- **Курс:** Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027
- **Заказчик:** ПАО «Таттелеком», подразделение ЕЦУиМС (единый центр управления и мониторинга сети)
- **Стек:** Vue 3 (`:5173`) + ASP.NET Core (`:5000`) + PostgreSQL (`:5432`)

---

## Запуск клиента (ЛР3–ЛР6 — Vue 3 + Vite + Vue Router + API/Mock)

Выполните из корня репозитория:

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
├── README.md                     ← описание проекта и команды запуска клиента
├── .gitignore                    ← исключения Git (node_modules/, dist/, .env, но не .env.example)
├── docs/
│   ├── project-proposal.md       ← заявка на тему (ЛР1)
│   ├── lab-01-requirements.md    ← требования и критерии приёмки (ЛР1)
│   ├── architecture.md           ← схема из трёх узлов (ЛР2)
│   ├── data-model.md             ← ER-диаграмма, ключи и проверка 3НФ (ЛР2)
│   ├── api-contract.md           ← таблица API и примеры JSON (ЛР2)
│   ├── requirements-matrix.md    ← матрица: критерий → поле → запрос (ЛР2)
│   └── reports/
│       ├── lab-02.md             ← отчёт по ЛР2
│       ├── lab-03.md             ← отчёт по ЛР3
│       ├── lab-04.md             ← отчёт по ЛР4
│       ├── lab-05.md             ← отчёт по ЛР5
│       └── lab-06.md             ← отчёт по ЛР6 (клиент API, mock и состояния UI)
└── frontend/
    ├── .env.example              ← шаблон переменных окружения (VITE_API_BASE_URL=http://localhost:5000)
    ├── package.json              ← зависимости (vue, vue-router)
    ├── package-lock.json
    ├── index.html
    ├── vite.config.js
    └── src/
        ├── main.js               ← точка входа и подключение router
        ├── App.vue               ← AppHeader + router-view
        ├── router.js             ← три маршрута (/, /new, /login)
        ├── ticketsStore.js       ← общее состояние списка
        ├── api/
        │   ├── tickets.js        ← getTickets, createTicket, переключатель USE_MOCK и fetch
        │   └── mockTickets.js    ← задержка 500 мс и учебные ответы (200, 201, 400, force500)
        ├── components/
        │   ├── AppHeader.vue
        │   └── TicketTable.vue
        └── pages/
            ├── TicketList.vue    ← четыре состояния: loading / empty / data / error
            ├── TicketNew.vue     ← отправка через createTicket, блокировка кнопки и вывод ошибок
            └── Login.vue
```
