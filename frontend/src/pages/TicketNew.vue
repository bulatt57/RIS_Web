<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { createTicket } from '../api/tickets.js'
import { addCreatedItem } from '../ticketsStore.js'

const router = useRouter()
const titleInput = ref(null)

const form = ref({
  title: '',
  departmentId: 1,
  description: ''
})

const errors = ref({
  title: '',
  departmentId: '',
  description: ''
})

const apiError = ref('')
const isSubmitting = ref(false)

onMounted(() => {
  titleInput.value?.focus()
})

async function onCreate() {
  if (isSubmitting.value) return

  const cleanTitle = form.value.title.trim()
  const deptId = form.value.departmentId
  const cleanDescription = form.value.description.trim()

  errors.value = {
    title: '',
    departmentId: '',
    description: ''
  }
  apiError.value = ''

  if (cleanTitle.length < 5 || cleanTitle.length > 80) {
    errors.value.title = 'Тема: от 5 до 80 символов'
  }

  if (typeof deptId !== 'number' || !Number.isInteger(deptId) || deptId <= 0) {
    errors.value.departmentId = 'Номер подразделения: целое число больше нуля'
  }

  if (cleanDescription.length < 10 || cleanDescription.length > 500) {
    errors.value.description = 'Описание: от 10 до 500 символов'
  }

  isSubmitting.value = true

  try {
    const created = await createTicket({
      title: cleanTitle,
      departmentId: deptId,
      description: cleanDescription
    })

    addCreatedItem(created)

    errors.value = {
      title: '',
      departmentId: '',
      description: ''
    }
    form.value = {
      title: '',
      departmentId: 1,
      description: ''
    }

    router.push('/')
  } catch (err) {
    apiError.value = err?.message || 'Нет связи с сервером'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <section aria-labelledby="new-heading">
    <h2 id="new-heading">Создание поручения</h2>
    <p class="section-hint">
      Отправка выполняется через <code>createTicket()</code> из <code>src/api/tickets.js</code>.
      Поля <code>id</code>, <code>number</code> и <code>status</code> приходят из ответа.
    </p>

    <form class="card-form" novalidate @submit.prevent="onCreate">
      <div v-if="apiError" class="api-error-banner" role="alert">
        {{ apiError }}
      </div>

      <div class="form-group">
        <label for="title">Тема поручения (title, 5–80 символов)</label>
        <input
          id="title"
          ref="titleInput"
          v-model="form.title"
          name="title"
          type="text"
          :disabled="isSubmitting"
          placeholder="Например: Проверить оптическую линию на узле доступа"
          autofocus
        />
        <p v-if="errors.title" class="field-error" role="alert">
          {{ errors.title }}
        </p>
      </div>

      <div class="form-group">
        <label for="departmentId">Номер подразделения из справочника (departmentId)</label>
        <input
          id="departmentId"
          v-model.number="form.departmentId"
          name="departmentId"
          type="number"
          :disabled="isSubmitting"
          placeholder="1"
        />
        <small class="field-help">
          1 — Сеть доступа, 2 — Транспортная сеть, 3 — Мониторинг, 4 — Эксплуатация
        </small>
        <p v-if="errors.departmentId" class="field-error" role="alert">
          {{ errors.departmentId }}
        </p>
      </div>

      <div class="form-group">
        <label for="description">Описание поручения (description, 10–500 символов)</label>
        <textarea
          id="description"
          v-model="form.description"
          name="description"
          rows="4"
          :disabled="isSubmitting"
          placeholder="Подробно опишите задачу, участок сети и ожидаемый результат"
        ></textarea>
        <p v-if="errors.description" class="field-error" role="alert">
          {{ errors.description }}
        </p>
      </div>

      <button type="submit" class="primary-btn" :disabled="isSubmitting">
        {{ isSubmitting ? 'Создаём…' : 'Создать' }}
      </button>
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

.api-error-banner {
  margin-bottom: 16px;
  padding: 12px 14px;
  border-radius: 6px;
  border: 1px solid #f87171;
  background-color: #fef2f2;
  color: #991b1b;
  font-weight: 600;
  font-size: 0.95rem;
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

.form-group input:disabled,
.form-group textarea:disabled {
  background-color: #f3f4f6;
  color: #6b7280;
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

.field-error {
  margin: 6px 0 0;
  font-size: 0.88rem;
  font-weight: 600;
  color: #dc2626;
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

.primary-btn:hover:not(:disabled) {
  background-color: #1d4ed8;
}

.primary-btn:disabled {
  background-color: #93c5fd;
  cursor: not-allowed;
}
</style>
