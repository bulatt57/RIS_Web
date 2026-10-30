<script setup>
import { departments } from '../ticketsStore.js'

defineProps({
  items: {
    type: Array,
    required: true
  }
})
</script>

<template>
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
</template>

<style scoped>
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
</style>
