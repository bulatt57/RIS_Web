-- Лабораторная работа №9: Обязательные SQL-запросы (queries.sql)

-- ============================================================================
-- 6.1. Фильтр и сортировка: выборка поручений в статусе 'New' по убыванию даты создания
-- ============================================================================
SELECT
    number,
    title,
    status,
    created_at
FROM assignments
WHERE status = 'New'
ORDER BY created_at DESC;

-- ============================================================================
-- 6.2. JOIN со справочником подразделений (и пользователями):
-- Результат показывает человекочитаемое название подразделения (department_name), а не только department_id
-- ============================================================================
SELECT
    a.number,
    a.title,
    d.name AS department_name,
    u_creator.full_name AS created_by,
    COALESCE(u_assignee.full_name, 'Не назначен') AS assignee_name,
    a.status,
    a.created_at
FROM assignments AS a
JOIN departments AS d ON d.id = a.department_id
JOIN users AS u_creator ON u_creator.id = a.created_by_user_id
LEFT JOIN users AS u_assignee ON u_assignee.id = a.assignee_user_id
ORDER BY a.created_at DESC;

-- ============================================================================
-- 6.3. GROUP BY: количество поручений по каждому из четырёх статусов
-- Начальные данные (seed.sql) позволяют увидеть все 4 группы: Cancelled, Closed, InProgress, New
-- ============================================================================
SELECT
    status,
    COUNT(*) AS count
FROM assignments
GROUP BY status
ORDER BY status;

-- ============================================================================
-- 6.4. Проверка ограничения CHECK (выполнять отдельно для демонстрации отказа СУБД):
-- Попытка вставить строку с недопустимым статусом 'Done' завершается ошибкой:
-- ERROR:  new row for relation "assignments" violates check constraint "assignments_status_check"
-- ============================================================================
/*
INSERT INTO assignments (
    number,
    title,
    description,
    department_id,
    status,
    created_by_user_id
) VALUES (
    'ПОР-2026-9999',
    'Проверка недопустимого статуса Done',
    'Эта строка должна быть отклонена ограничением CHECK в PostgreSQL',
    1,
    'Done',
    1
);
*/
