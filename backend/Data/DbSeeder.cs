using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public static class DbSeeder
{
    public static void SeedIfEmpty(AppDbContext db)
    {
        if (!db.Departments.Any())
        {
            db.Departments.AddRange(
                new Department { Id = 1, Code = "ACCESS", Name = "Сеть доступа" },
                new Department { Id = 2, Code = "TRANSPORT", Name = "Транспортная сеть" },
                new Department { Id = 3, Code = "MONITORING", Name = "Мониторинг" },
                new Department { Id = 4, Code = "OPERATIONS", Name = "Эксплуатация" }
            );
            db.SaveChanges();
            db.Database.ExecuteSqlRaw(
                "SELECT setval(pg_get_serial_sequence('departments', 'id'), COALESCE((SELECT MAX(id) FROM departments), 1));");
        }

        if (!db.Users.Any())
        {
            db.Users.AddRange(
                new User { Id = 1, FullName = "Салихов Рустам Ильдарович", Email = "r.salikhov@tattelecom.local", Role = "Manager" },
                new User { Id = 2, FullName = "Тухбатуллин Булат Айдарович", Email = "b.tukhbatullin@tattelecom.local", Role = "Engineer" },
                new User { Id = 3, FullName = "Каримов Айрат Маратович", Email = "a.karimov@tattelecom.local", Role = "Engineer" },
                new User { Id = 4, FullName = "Зарипова Алия Фаритовна", Email = "a.zaripova@tattelecom.local", Role = "Dispatcher" },
                new User { Id = 5, FullName = "Ибрагимов Тимур Рафаэлевич", Email = "t.ibragimov@tattelecom.local", Role = "Engineer" }
            );
            db.SaveChanges();
            db.Database.ExecuteSqlRaw(
                "SELECT setval(pg_get_serial_sequence('users', 'id'), COALESCE((SELECT MAX(id) FROM users), 1));");
        }
        else if (!db.Users.Any(u => u.Id == 5))
        {
            db.Users.Add(new User
            {
                Id = 5,
                FullName = "Ибрагимов Тимур Рафаэлевич",
                Email = "t.ibragimov@tattelecom.local",
                Role = "Engineer"
            });
            db.SaveChanges();
            db.Database.ExecuteSqlRaw(
                "SELECT setval(pg_get_serial_sequence('users', 'id'), COALESCE((SELECT MAX(id) FROM users), 1));");
        }

        if (!db.Assignments.Any())
        {
            db.Assignments.AddRange(
                new Assignment
                {
                    Id = 1,
                    Number = "ПОР-2026-0001",
                    Title = "Проверить оптическую линию на узле доступа",
                    Description = "Выполнить диагностику затухания сигнала после плановых работ и приложить сводку",
                    DepartmentId = 1,
                    Status = "New",
                    CreatedAt = new DateTimeOffset(2026, 11, 1, 9, 15, 0, TimeSpan.FromHours(3)).ToUniversalTime(),
                    CreatedByUserId = 1,
                    AssigneeUserId = null
                },
                new Assignment
                {
                    Id = 2,
                    Number = "ПОР-2026-0002",
                    Title = "Подготовить сводку по ночным инцидентам",
                    Description = "Собрать данные из системы мониторинга за смену и передать руководителю ЕЦУиМС",
                    DepartmentId = 3,
                    Status = "InProgress",
                    CreatedAt = new DateTimeOffset(2026, 11, 2, 10, 30, 0, TimeSpan.FromHours(3)).ToUniversalTime(),
                    CreatedByUserId = 1,
                    AssigneeUserId = 2
                },
                new Assignment
                {
                    Id = 3,
                    Number = "ПОР-2026-0003",
                    Title = "Согласовать окно плановых работ на магистрали",
                    Description = "Подготовить график переключения каналов транспортной сети без прерывания сервиса",
                    DepartmentId = 2,
                    Status = "Closed",
                    CreatedAt = new DateTimeOffset(2026, 11, 3, 14, 20, 0, TimeSpan.FromHours(3)).ToUniversalTime(),
                    CreatedByUserId = 1,
                    AssigneeUserId = 3
                },
                new Assignment
                {
                    Id = 4,
                    Number = "ПОР-2026-0004",
                    Title = "Выездная проверка резервного питания базового узла",
                    Description = "Проверить ёмкость аккумуляторных батарей и работу дизель-генератора на узле связи",
                    DepartmentId = 4,
                    Status = "Cancelled",
                    CreatedAt = new DateTimeOffset(2026, 11, 4, 16, 45, 0, TimeSpan.FromHours(3)).ToUniversalTime(),
                    CreatedByUserId = 4,
                    AssigneeUserId = null
                },
                new Assignment
                {
                    Id = 5,
                    Number = "ПОР-2026-0005",
                    Title = "Проверить резервный канал связи на узле Б",
                    Description = "После грозы наблюдается затухание сигнала на резервной линии транспортной сети",
                    DepartmentId = 2,
                    Status = "New",
                    CreatedAt = new DateTimeOffset(2026, 11, 5, 11, 0, 0, TimeSpan.FromHours(3)).ToUniversalTime(),
                    CreatedByUserId = 1,
                    AssigneeUserId = null
                }
            );
            db.SaveChanges();
            db.Database.ExecuteSqlRaw(
                "SELECT setval(pg_get_serial_sequence('assignments', 'id'), COALESCE((SELECT MAX(id) FROM assignments), 1));");
        }
    }
}
