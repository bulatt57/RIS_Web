# Матрица трассировки требований (TaskControl)

Курс: Б1.В.ДЭ.01.02.02 · КГЭУ · семестр 1 · 2026/2027  
Студент: Тухбатуллин Булат Айдарович · Группа: ТРИС-1-23  
Тема: Разработка системы контроля исполнения поручений руководителя в средней компании (TaskControl, ПАО «Таттелеком» — ЕЦУиМС)

---

## 1. Обязательная матрица для трёх критериев приёмки ЛР1

| Требование ЛР1 | Поле или сущность | Запрос | Критерий |
|----------------|-------------------|--------|----------|
| Руководитель создаёт поручение | `Assignment`, `title`, `departmentId`, `description` (ответ: `id`, `number`, `status: New`) | `POST /api/assignments` | Критерий 1 |
| Руководитель назначает исполнителя | `Assignment.assigneeUserId` (FK → `User.id`) | `PATCH /api/assignments/{id}/assignee` | Критерий 2 |
| Исполнитель переводит поручение в работу | `Assignment.status` (`New` → `InProgress`), `startedAt` | `PATCH /api/assignments/{id}/status` | Критерий 3 |

---

## 2. Полная трассировка функциональных требований (ФТ-1 … ФТ-11) из ЛР1

| Код ФТ | Краткое содержание требования из `lab-01-requirements.md` | Сущности и поля модели данных | Метод и путь API | Код ответа |
|--------|-----------------------------------------------------------|-------------------------------|------------------|------------|
| **ФТ-1** | Руководитель входит и видит список всех поручений подразделения | `User` (`login`, `passwordHash`, `role: Manager`), `Assignment` | `GET /api/assignments` | `200 OK` |
| **ФТ-2** | Исполнитель входит и видит только свои поручения | `User` (`role: Executor`), `Assignment.assigneeUserId` | `GET /api/assignments?assigneeUserId={me}` | `200 OK` |
| **ФТ-3** | Руководитель создаёт поручение (заголовок, подразделение, описание, срок); получает `New` и `ПОР-2026-0001` | `Assignment` (`id`, `number`, `title`, `departmentId`, `description`, `dueDate`, `status`) | `POST /api/assignments` | `201 Created` / `400` |
| **ФТ-4** | Руководитель назначает или меняет исполнителя отдельным действием без смены статуса | `Assignment.assigneeUserId`, `User.id` | `PATCH /api/assignments/{id}/assignee` | `200 OK` / `404` |
| **ФТ-5** | Исполнитель переводит своё поручение из `New` в `InProgress` (фиксируется дата начала) | `Assignment.status` (`InProgress`), `Assignment.startedAt` | `PATCH /api/assignments/{id}/status` | `200 OK` / `409` |
| **ФТ-6** | Исполнитель переводит своё поручение из `InProgress` в `Closed` (фиксируется дата закрытия) | `Assignment.status` (`Closed`), `Assignment.closedAt` | `PATCH /api/assignments/{id}/status` | `200 OK` / `409` |
| **ФТ-7** | Руководитель отменяет поручение (`New` / `InProgress` → `Cancelled`) с указанием причины | `Assignment.status` (`Cancelled`), `Assignment.cancelReason` | `PATCH /api/assignments/{id}/status` | `200 OK` / `409` |
| **ФТ-8** | Руководитель фильтрует список по статусу, подразделению, исполнителю и сроку | `Assignment` (`status`, `departmentId`, `assigneeUserId`, `dueDate`) | `GET /api/assignments?status=...&departmentId=...` | `200 OK` |
| **ФТ-9** | Исполнитель не может создать поручение и не может назначить исполнителя | `User.role` (`Executor`) | `POST /api/assignments`, `PATCH .../assignee` | `403 Forbidden` |
| **ФТ-10** | Исполнитель не может изменить статус чужого поручения | `Assignment.assigneeUserId != currentUserId` | `PATCH /api/assignments/{id}/status` | `403 Forbidden` |
| **ФТ-11** | Наблюдатель (`Viewer`) видит сводку только для чтения, любая мутация запрещена | `User.role` (`Viewer`) | Любой `POST` / `PATCH` | `403 Forbidden` |
