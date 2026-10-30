<script setup>
import TicketTable from '../components/TicketTable.vue'
import { items } from '../ticketsStore.js'
</script>

<template>
  <section aria-labelledby="list-heading">
    <h2 id="list-heading">Список поручений</h2>
    <p class="section-hint">
      Таблица получает массив через props, источник — <code>ticketsStore.js</code>.
    </p>

    <TicketTable :items="items" />
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
</style>
