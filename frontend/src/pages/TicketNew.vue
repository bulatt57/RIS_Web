<script setup>
import { ref, onMounted } from 'vue'

const titleInput = ref(null)

// Поля формы создания поручения (имена строго по api-contract.md из ЛР2)
const title = ref('')
const departmentId = ref(1)
const description = ref('')

onMounted(() => {
  titleInput.value?.focus()
})
</script>

<template>
  <section aria-labelledby="new-heading">
    <h2 id="new-heading">Создание поручения</h2>
    <p class="section-hint">
      Маршрут клиента <code>/new</code> (будущий запрос к серверу — <code>POST /api/assignments</code>).
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
</template>

<style scoped>
h2 {
  margin-top: 0;
  font-size: 1.3rem;
  color: #111827;
}

.section-hint {
  margin-bottom: 16px;
  font-size: 0.9rem;
  color: #4b5563;
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
