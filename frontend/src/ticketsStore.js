import { ref } from 'vue'

export const items = ref([
  {
    id: 1,
    number: 'ПОР-2026-0001',
    title: 'Проверить оптическую линию на узле доступа',
    departmentId: 1,
    description: 'Выполнить диагностику затухания сигнала после плановых работ и приложить сводку',
    status: 'New'
  },
  {
    id: 2,
    number: 'ПОР-2026-0002',
    title: 'Подготовить сводку по ночным инцидентам',
    departmentId: 3,
    description: 'Собрать данные из системы мониторинга за смену и передать руководителю ЕЦУиМС',
    status: 'InProgress'
  },
  {
    id: 3,
    number: 'ПОР-2026-0003',
    title: 'Согласовать окно плановых работ на магистрали',
    departmentId: 2,
    description: 'Подготовить график переключения каналов транспортной сети без прерывания сервиса',
    status: 'Closed'
  },
  {
    id: 4,
    number: 'ПОР-2026-0004',
    title: 'Выездная проверка резервного питания базового узла',
    departmentId: 4,
    description: 'Проверить ёмкость аккумуляторных батарей и работу дизель-генератора на узле связи',
    status: 'Cancelled'
  }
])

export let nextId = 5

export function addItem({ title, departmentId, description }) {
  const currentId = nextId++
  const newItem = {
    id: currentId,
    number: `TMP-${currentId}`,
    title,
    departmentId,
    description,
    status: 'New'
  }
  items.value.push(newItem)
  return newItem
}
