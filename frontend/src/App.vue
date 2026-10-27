<script setup>
import { ref, watch, nextTick } from 'vue'

// Текущий активный экран: 'list' (Список), 'new' (Создание), 'login' (Вход)
const screen = ref('list')

// Ссылка на первое поле формы создания для гарантированного фокуса при переключении экрана
const titleInput = ref(null)

watch(screen, async (newScreen) => {
  if (newScreen === 'new') {
    await nextTick()
    titleInput.value?.focus()
  }
})

// Справочник подразделений ЕЦУиМС ПАО «Таттелеком» (соответствует сущности Department из ЛР2)
const departments = {
  1: 'Сеть доступа',
  2: 'Транспортная сеть',
  3: 'Мониторинг',
  4: 'Эксплуатация'
}

// Тестовые записи поручений (соответствуют сущности Assignment и контракту API из ЛР2)
const items = ref([
  {
    id: 1,
    number: 'ПОР-2026-0001',
    title: 'Проверить оптическую линию на узле доступа',
    departmentId: 1,
    status: 'New'
  },
  {
    id: 2,
    number: 'ПОР-2026-0002',
    title: 'Подготовить сводку по ночным инцидентам',
    departmentId: 3,
    status: 'InProgress'
  },
  {
    id: 3,
    number: 'ПОР-2026-0003',
    title: 'Согласовать окно плановых работ на магистрали',
    departmentId: 2,
    status: 'Closed'
  },
  {
    id: 4,
    number: 'ПОР-2026-0004',
    title: 'Выездная проверка резервного питания базового узла',
    departmentId: 4,
    status: 'Cancelled'
  }
])

// Поля формы создания поручения (имена строго по api-contract.md из ЛР2)
const title = ref('')
const departmentId = ref(1)
const description = ref('')

// Поля формы входа
const login = ref('')
const password = ref('')
</script>

<template>
  <div class="app-layout">
    <header class="app-header">
      <div class="brand">
        <h1>TaskControl · ЕЦУиМС ПАО «Таттелеком»</h1>
        <p class="subtitle">Система контроля исполнения поручений руководителя</p>
      </div>

      <nav class="screen-nav" aria-label="Переключение экранов">
        <button
          type="button"
          :class="{ active: screen === 'list' }"
          @click="screen = 'list'"
        >
          Список поручений
        </button>
        <button
          type="button"
          :class="{ active: screen === 'new' }"
          @click="screen = 'new'"
        >
          Создание поручения
        </button>
        <button
          type="button"
          :class="{ active: screen === 'login' }"
          @click="screen = 'login'"
        >
          Вход
        </button>
      </nav>
    </header>

    <main class="app-main">
      <!-- Экран 1: Список поручений -->
      <section v-if="screen === 'list'" aria-labelledby="list-heading">
        <h2 id="list-heading">Список поручений</h2>
        <p class="section-hint">
          Отображаются тестовые записи сущности <code>Assignment</code> (статусы курса:
          <code>New</code>, <code>InProgress</code>, <code>Closed</code>, <code>Cancelled</code>).
        </p>

        <table class="assignments-table">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Номер (number)</th>
              <th scope="col">Тема поручения (title)</th>
              <th scope="col">Подразделение (departmentId)</th>
              <th scope="col">Статус (status)</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in items" :key="item.id">
              <td>{{ item.id }}</td>
              <td class="mono">{{ item.number }}</td>
              <td>{{ item.title }}</td>
              <td>
                {{ item.departmentId }}
                <span class="dept-name">({{ departments[item.departmentId] || 'Отдел ЕЦУиМС' }})</span>
              </td>
              <td>
                <span class="status-badge" :data-status="item.status">
                  {{ item.status }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </section>

      <!-- Экран 2: Создание поручения -->
      <section v-else-if="screen === 'new'" aria-labelledby="new-heading">
        <h2 id="new-heading">Создание поручения</h2>
        <p class="section-hint">
          Поля формы соответствуют контракту <code>POST /api/assignments</code> из ЛР2.
        </p>

        <form class="card-form" @submit.prevent>
          <div class="form-group">
            <label for="title">Заголовок поручения (title, 5–80 символов)</label>
            <input
              id="title"
              ref="titleInput"
              v-model="title"
              name="title"
              type="text"
              minlength="5"
              maxlength="80"
              placeholder="Например: Проверить оптическую линию на узле доступа"
              autofocus
              required
            />
          </div>

          <div class="form-group">
            <label for="departmentId">Номер подразделения из справочника (departmentId)</label>
            <input
              id="departmentId"
              v-model.number="departmentId"
              name="departmentId"
              type="number"
              min="1"
              placeholder="1"
              required
            />
            <small class="field-help">
              1 — Сеть доступа, 2 — Транспортная сеть, 3 — Мониторинг, 4 — Эксплуатация
            </small>
          </div>

          <div class="form-group">
            <label for="description">Описание поручения (description, 10–500 символов)</label>
            <textarea
              id="description"
              v-model="description"
              name="description"
              rows="4"
              minlength="10"
              maxlength="500"
              placeholder="Подробно опишите задачу, участок сети и ожидаемый результат"
              required
            ></textarea>
          </div>

          <button type="submit" class="primary-btn">Создать</button>
        </form>
      </section>

      <!-- Экран 3: Вход в систему -->
      <section v-else-if="screen === 'login'" aria-labelledby="login-heading">
        <h2 id="login-heading">Вход в систему</h2>
        <p class="section-hint">
          Авторизация сотрудников ЕЦУиМС (Руководитель, Исполнитель, Наблюдатель).
        </p>

        <form class="card-form" @submit.prevent>
          <div class="form-group">
            <label for="login">Логин (email сотрудника)</label>
            <input
              id="login"
              v-model="login"
              name="login"
              type="text"
              placeholder="manager@tattelecom.ru"
              required
            />
          </div>

          <div class="form-group">
            <label for="password">Пароль</label>
            <input
              id="password"
              v-model="password"
              name="password"
              type="password"
              placeholder="Введите пароль"
              required
            />
          </div>

          <button type="submit" class="primary-btn">Войти</button>
        </form>
      </section>
    </main>
  </div>
</template>

<style scoped>
.app-layout {
  max-width: 1080px;
  margin: 0 auto;
  padding: 24px;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  color: #1f2937;
}

.app-header {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
  padding-bottom: 20px;
  margin-bottom: 24px;
  border-bottom: 2px solid #e5e7eb;
}

.brand h1 {
  margin: 0;
  font-size: 1.4rem;
  color: #111827;
}

.subtitle {
  margin: 4px 0 0;
  font-size: 0.9rem;
  color: #4b5563;
}

.screen-nav {
  display: flex;
  gap: 8px;
}

.screen-nav button {
  padding: 8px 16px;
  font-size: 0.95rem;
  font-weight: 500;
  color: #374151;
  background-color: #f3f4f6;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  cursor: pointer;
}

.screen-nav button:hover {
  background-color: #e5e7eb;
}

.screen-nav button.active {
  color: #ffffff;
  background-color: #2563eb;
  border-color: #2563eb;
}

.app-main h2 {
  margin-top: 0;
  font-size: 1.3rem;
  color: #111827;
}

.section-hint {
  margin-bottom: 16px;
  font-size: 0.9rem;
  color: #4b5563;
}

.assignments-table {
  width: 100%;
  border-collapse: collapse;
  background-color: #ffffff;
  border: 1px solid #d1d5db;
}

.assignments-table th,
.assignments-table td {
  padding: 12px 14px;
  text-align: left;
  border-bottom: 1px solid #e5e7eb;
}

.assignments-table th {
  background-color: #f9fafb;
  font-weight: 600;
  color: #374151;
}

.mono {
  font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace;
  font-weight: 600;
}

.dept-name {
  color: #6b7280;
  font-size: 0.88rem;
}

.status-badge {
  display: inline-block;
  padding: 4px 10px;
  font-size: 0.82rem;
  font-weight: 600;
  border-radius: 4px;
  background-color: #e5e7eb;
  color: #1f2937;
}

.status-badge[data-status='New'] {
  background-color: #dbeafe;
  color: #1e40af;
}

.status-badge[data-status='InProgress'] {
  background-color: #fef3c7;
  color: #92400e;
}

.status-badge[data-status='Closed'] {
  background-color: #d1fae5;
  color: #065f46;
}

.status-badge[data-status='Cancelled'] {
  background-color: #fee2e2;
  color: #991b1b;
}

.card-form {
  max-width: 560px;
  padding: 20px;
  background-color: #f9fafb;
  border: 1px solid #d1d5db;
  border-radius: 8px;
}

.form-group {
  display: flex;
  flex-direction: column;
  margin-bottom: 16px;
}

.form-group label {
  margin-bottom: 6px;
  font-weight: 600;
  font-size: 0.95rem;
  color: #111827;
}

.form-group input,
.form-group textarea {
  padding: 10px 12px;
  font-size: 0.95rem;
  font-family: inherit;
  color: #111827;
  background-color: #ffffff;
  border: 1px solid #9ca3af;
  border-radius: 6px;
}

.form-group input:focus,
.form-group textarea:focus {
  outline: 2px solid #2563eb;
  border-color: #2563eb;
}

.field-help {
  margin-top: 4px;
  font-size: 0.82rem;
  color: #4b5563;
}

.primary-btn {
  padding: 10px 20px;
  font-size: 0.95rem;
  font-weight: 600;
  color: #ffffff;
  background-color: #2563eb;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}

.primary-btn:hover {
  background-color: #1d4ed8;
}
</style>
