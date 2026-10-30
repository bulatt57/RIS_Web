import { ref } from 'vue'

// Справочник подразделений ЕЦУиМС ПАО «Таттелеком» (сущность Department из ЛР2)
export const departments = {
  1: 'Сеть доступа',
  2: 'Транспортная сеть',
  3: 'Мониторинг',
  4: 'Эксплуатация'
}

// Единственный общий массив записей поручений (сущность Assignment из ЛР2)
export const items = ref([
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
