using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class AssignmentService : IAssignmentService
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.Ordinal)
    {
        "New",
        "InProgress",
        "Closed",
        "Cancelled"
    };

    private readonly AppDbContext _db;

    public AssignmentService(AppDbContext db)
    {
        _db = db;
    }

    public string GetApplicationName() => "TaskControl (ПАО «Таттелеком» — ЕЦУиМС)";

    public string GetLabStageNote() => "ЛР10: EF Core — сервер читает и пишет в базу PostgreSQL";

    public IReadOnlyList<AssignmentResponse> GetAll()
    {
        return _db.Assignments
            .AsNoTracking()
            .OrderBy(a => a.Id)
            .Select(a => MapToResponse(a))
            .ToList();
    }

    public AssignmentResponse? GetById(int id)
    {
        var item = _db.Assignments
            .AsNoTracking()
            .FirstOrDefault(a => a.Id == id);

        return item is null ? null : MapToResponse(item);
    }

    public ServiceResult<AssignmentResponse> Create(CreateAssignmentRequest? request)
    {
        if (request is null)
        {
            return ServiceResult<AssignmentResponse>.Validation("Тело запроса не может быть пустым.");
        }

        var cleanTitle = (request.Title ?? string.Empty).Trim();
        var cleanDescription = (request.Description ?? string.Empty).Trim();
        var departmentId = request.ResolvedDepartmentId;
        var createdByUserId = request.CreatedByUserId is > 0 ? request.CreatedByUserId.Value : 1;

        var errors = new Dictionary<string, string[]>();

        if (cleanTitle.Length < 5 || cleanTitle.Length > 80)
        {
            errors["title"] = new[] { "Поле title должно содержать от 5 до 80 символов." };
        }

        if (departmentId <= 0)
        {
            errors["departmentId"] = new[] { "Поле departmentId (siteId) должно быть целым числом больше нуля." };
        }
        else if (!_db.Departments.Any(d => d.Id == departmentId))
        {
            errors["departmentId"] = new[] { $"Подразделение с id={departmentId} не существует в справочнике departments." };
        }

        if (cleanDescription.Length < 10 || cleanDescription.Length > 500)
        {
            errors["description"] = new[] { "Поле description должно содержать от 10 до 500 символов." };
        }

        if (!_db.Users.Any(u => u.Id == createdByUserId))
        {
            errors["createdByUserId"] = new[] { $"Пользователь-создатель с id={createdByUserId} не найден." };
        }

        if (errors.Count > 0)
        {
            return ServiceResult<AssignmentResponse>.Validation("Проверьте поля запроса.", errors);
        }

        var created = new Assignment
        {
            Number = $"TMP-{Guid.NewGuid():N}",
            Title = cleanTitle,
            DepartmentId = departmentId,
            Description = cleanDescription,
            Status = "New",
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByUserId = createdByUserId,
            AssigneeUserId = null
        };

        _db.Assignments.Add(created);
        _db.SaveChanges();

        // Формируем постоянный регистрационный номер по сгенерированному БД идентификатору Id
        created.Number = $"ПОР-2026-{created.Id:D4}";
        _db.SaveChanges();

        return ServiceResult<AssignmentResponse>.Success(MapToResponse(created));
    }

    public ServiceResult<AssignmentResponse> AssignUser(int id, AssignAssigneeRequest? request)
    {
        if (request is null || request.AssigneeUserId <= 0)
        {
            return ServiceResult<AssignmentResponse>.Validation(
                "Поле assigneeUserId должно быть целым числом больше нуля.",
                new Dictionary<string, string[]>
                {
                    ["assigneeUserId"] = new[] { "Укажите корректный идентификатор исполнителя (> 0)." }
                });
        }

        var item = _db.Assignments.FirstOrDefault(a => a.Id == id);
        if (item is null)
        {
            return ServiceResult<AssignmentResponse>.NotFound($"Поручение с id={id} не найдено.");
        }

        if (!_db.Users.Any(u => u.Id == request.AssigneeUserId))
        {
            return ServiceResult<AssignmentResponse>.Validation(
                $"Пользователь с id={request.AssigneeUserId} не существует в таблице users.",
                new Dictionary<string, string[]>
                {
                    ["assigneeUserId"] = new[] { $"Пользователь с id={request.AssigneeUserId} не найден." }
                });
        }

        item.AssigneeUserId = request.AssigneeUserId;
        _db.SaveChanges();

        return ServiceResult<AssignmentResponse>.Success(MapToResponse(item));
    }

    public ServiceResult<AssignmentResponse> ChangeStatus(int id, ChangeStatusRequest? request)
    {
        var newStatus = (request?.Status ?? string.Empty).Trim();

        if (string.IsNullOrEmpty(newStatus) || !AllowedStatuses.Contains(newStatus))
        {
            return ServiceResult<AssignmentResponse>.Validation(
                "Недопустимый статус. Допустимые значения: New, InProgress, Closed, Cancelled.",
                new Dictionary<string, string[]>
                {
                    ["status"] = new[] { "Допустимые статусы: New, InProgress, Closed, Cancelled." }
                });
        }

        var item = _db.Assignments.FirstOrDefault(a => a.Id == id);
        if (item is null)
        {
            return ServiceResult<AssignmentResponse>.NotFound($"Поручение с id={id} не найдено.");
        }

        // Правило процесса: нельзя переводить запись в InProgress, пока исполнитель не назначен -> 409 Conflict
        if (newStatus == "InProgress" && item.AssigneeUserId is null)
        {
            return ServiceResult<AssignmentResponse>.Conflict(
                "Нельзя перевести поручение в статус InProgress, пока не назначен исполнитель (assigneeUserId).");
        }

        item.Status = newStatus;
        _db.SaveChanges();

        return ServiceResult<AssignmentResponse>.Success(MapToResponse(item));
    }

    private static AssignmentResponse MapToResponse(Assignment item) => new()
    {
        Id = item.Id,
        Number = item.Number,
        Title = item.Title,
        DepartmentId = item.DepartmentId,
        SiteId = item.DepartmentId,
        Description = item.Description,
        Status = item.Status,
        CreatedAt = item.CreatedAt,
        CreatedByUserId = item.CreatedByUserId,
        AssigneeUserId = item.AssigneeUserId
    };
}
