-- Лабораторная работа №9: Начальные учебные данные (seed.sql)
-- Очистка таблиц перед повторной загрузкой с перезапуском счётчиков идентификаторов
TRUNCATE TABLE assignments, users, departments RESTART IDENTITY CASCADE;

-- 1. Справочник подразделений ЕЦУиМС ПАО «Таттелеком» (4 строки, минимум по методичке — 2)
INSERT INTO departments (id, code, name) VALUES
    (1, 'ACCESS', 'Сеть доступа'),
    (2, 'TRANSPORT', 'Транспортная сеть'),
    (3, 'MONITORING', 'Мониторинг'),
    (4, 'OPERATIONS', 'Эксплуатация');

SELECT setval(pg_get_serial_sequence('departments', 'id'), (SELECT MAX(id) FROM departments));

-- 2. Пользователи: руководители и инженеры (4 пользователя, минимум по методичке — 2)
INSERT INTO users (id, full_name, email, role) VALUES
    (1, 'Салихов Рустам Ильдарович', 'r.salikhov@tattelecom.local', 'Manager'),
    (2, 'Тухбатуллин Булат Айдарович', 'b.tukhbatullin@tattelecom.local', 'Engineer'),
    (3, 'Каримов Айрат Маратович', 'a.karimov@tattelecom.local', 'Engineer'),
    (4, 'Зарипова Алия Фаритовна', 'a.zaripova@tattelecom.local', 'Dispatcher');

SELECT setval(pg_get_serial_sequence('users', 'id'), (SELECT MAX(id) FROM users));

-- 3. Главный объект — Поручения (представлены все 4 обязательных статуса: New, InProgress, Closed, Cancelled)
INSERT INTO assignments (
    id,
    number,
    title,
    description,
    department_id,
    status,
    created_at,
    created_by_user_id,
    assignee_user_id
) VALUES
    (
        1,
        'ПОР-2026-0001',
        'Проверить оптическую линию на узле доступа',
        'Выполнить диагностику затухания сигнала после плановых работ и приложить сводку',
        1,
        'New',
        '2026-11-01 09:15:00+03',
        1,
        NULL
    ),
    (
        2,
        'ПОР-2026-0002',
        'Подготовить сводку по ночным инцидентам',
        'Собрать данные из системы мониторинга за смену и передать руководителю ЕЦУиМС',
        3,
        'InProgress',
        '2026-11-02 10:30:00+03',
        1,
        2
    ),
    (
        3,
        'ПОР-2026-0003',
        'Согласовать окно плановых работ на магистрали',
        'Подготовить график переключения каналов транспортной сети без прерывания сервиса',
        2,
        'Closed',
        '2026-11-03 14:20:00+03',
        1,
        3
    ),
    (
        4,
        'ПОР-2026-0004',
        'Выездная проверка резервного питания базового узла',
        'Проверить ёмкость аккумуляторных батарей и работу дизель-генератора на узле связи',
        4,
        'Cancelled',
        '2026-11-04 16:45:00+03',
        4,
        NULL
    ),
    (
        5,
        'ПОР-2026-0005',
        'Проверить резервный канал связи на узле Б',
        'После грозы наблюдается затухание сигнала на резервной линии транспортной сети',
        2,
        'New',
        '2026-11-05 11:00:00+03',
        1,
        NULL
    );

SELECT setval(pg_get_serial_sequence('assignments', 'id'), (SELECT MAX(id) FROM assignments));
