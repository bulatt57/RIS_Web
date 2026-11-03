// Учебный mock-модуль (имитация ответов сервера ASP.NET Core)
// Переключатель для проверки ошибки 500 (Шаг B и Шаг G методички)
export const force500 = false

// Переключатель для проверки пустого списка [] со статусом 200 (Шаг D методички)
export const forceEmpty = false

const DELAY_MS = 500

function wait(ms = DELAY_MS) {
  return new Promise((resolve) => setTimeout(resolve, ms))
}

let nextId = 5

const mockItems = [
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
]

export async function mockGetTickets() {
  await wait()

  if (force500) {
    return {
      ok: false,
      status: 500,
      data: { message: 'Сервер не смог обработать запрос' }
    }
  }

  if (forceEmpty) {
    return {
      ok: true,
      status: 200,
      data: []
    }
  }

  return {
    ok: true,
    status: 200,
    data: [...mockItems]
  }
}

export async function mockCreateTicket(body) {
  await wait()

  if (force500) {
    return {
      ok: false,
      status: 500,
      data: { message: 'Сервер не смог обработать запрос' }
    }
  }

  const cleanTitle = typeof body?.title === 'string' ? body.title.trim() : ''
  const deptId = Number(body?.departmentId ?? body?.siteId)
  const cleanDescription = typeof body?.description === 'string' ? body.description.trim() : ''

  if (
    cleanTitle.length < 5 ||
    cleanTitle.length > 80 ||
    !Number.isInteger(deptId) ||
    deptId <= 0 ||
    cleanDescription.length < 10 ||
    cleanDescription.length > 500
  ) {
    return {
      ok: false,
      status: 400,
      data: { message: 'Проверьте поля' }
    }
  }

  const id = nextId++
  const number = `ПОР-2026-${String(id).padStart(4, '0')}`

  const created = {
    id,
    number,
    title: cleanTitle,
    departmentId: deptId,
    description: cleanDescription,
    status: 'New'
  }

  mockItems.push(created)

  return {
    ok: true,
    status: 201,
    data: created
  }
}
