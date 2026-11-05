using backend.Dtos;
using backend.Models;

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

    private readonly List<Assignment> _items = new();
    private readonly object _sync = new();
    private int _nextId = 1;

    public AssignmentService()
    {
        // Стартовые записи в памяти: запись #1 намеренно в статусе New без исполнителя (для проверки 409 и двух PATCH)
        SeedItem(
            title: "Проверить оптическую линию на узле доступа",
            departmentId: 1,
            description: "Выполнить диагностику затухания сигнала после плановых работ и приложить сводку",
            status: "New",
            assigneeUserId: null);

        SeedItem(
            title: "Подготовить сводку по ночным инцидентам",
            departmentId: 3,
            description: "Собрать данные из системы мониторинга за смену и передать руководителю ЕЦУиМС",
            status: "InProgress",
            assigneeUserId: 3);
    }

    private void SeedItem(string title, int departmentId, string description, string status, int? assigneeUserId)
    {
        var id = _nextId++;
        _items.Add(new Assignment
        {
            Id = id,
            Number = $"ПОР-2026-{id:D4}",
            Title = title,
            DepartmentId = departmentId,
            Description = description,
            Status = status,
            AssigneeUserId = assigneeUserId
        });
    }

    public string GetApplicationName() => "TaskControl (ПАО «Таттелеком» — ЕЦУиМС)";

    public string GetLabStageNote() => "ЛР8: REST главного объекта (создание, чтение, назначение и статус в памяти)";

    public IReadOnlyList<AssignmentResponse> GetAll()
    {
        lock (_sync)
        {
            return _items.Select(MapToResponse).ToList();
        }
    }

    public AssignmentResponse? GetById(int id)
    {
        lock (_sync)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return item is null ? null : MapToResponse(item);
        }
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

        var errors = new Dictionary<string, string[]>();

        if (cleanTitle.Length < 5 || cleanTitle.Length > 80)
        {
            errors["title"] = new[] { "Поле title должно содержать от 5 до 80 символов." };
        }

        if (departmentId <= 0)
        {
            errors["departmentId"] = new[] { "Поле departmentId (siteId) должно быть целым числом больше нуля." };
        }

        if (cleanDescription.Length < 10 || cleanDescription.Length > 500)
        {
            errors["description"] = new[] { "Поле description должно содержать от 10 до 500 символов." };
        }

        if (errors.Count > 0)
        {
            return ServiceResult<AssignmentResponse>.Validation("Проверьте поля запроса.", errors);
        }

        lock (_sync)
        {
            var id = _nextId++;
            var created = new Assignment
            {
                Id = id,
                Number = $"ПОР-2026-{id:D4}",
                Title = cleanTitle,
                DepartmentId = departmentId,
                Description = cleanDescription,
                Status = "New",
                AssigneeUserId = null
            };

            _items.Add(created);
            return ServiceResult<AssignmentResponse>.Success(MapToResponse(created));
        }
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

        lock (_sync)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item is null)
            {
                return ServiceResult<AssignmentResponse>.NotFound($"Поручение с id={id} не найдено.");
            }

            item.AssigneeUserId = request.AssigneeUserId;
            return ServiceResult<AssignmentResponse>.Success(MapToResponse(item));
        }
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

        lock (_sync)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item is null)
            {
                return ServiceResult<AssignmentResponse>.NotFound($"Поручение с id={id} не найдено.");
            }

            // Правило процесса (п. 3.2 и Шаг G): нельзя переводить запись в InProgress, пока исполнитель не назначен -> 409 Conflict
            if (newStatus == "InProgress" && item.AssigneeUserId is null)
            {
                return ServiceResult<AssignmentResponse>.Conflict(
                    "Нельзя перевести поручение в статус InProgress, пока не назначен исполнитель (assigneeUserId).");
            }

            item.Status = newStatus;
            return ServiceResult<AssignmentResponse>.Success(MapToResponse(item));
        }
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
        AssigneeUserId = item.AssigneeUserId
    };
}
