namespace backend.Dtos;

/// <summary>
/// Входной DTO для создания поручения (ЛР8, п. 3.1).
/// Клиент передаёт только изменяемые человеком поля (без id, number, status и assigneeUserId).
/// Поддерживаются оба имени поля подразделения: departmentId (из ЛР2) и siteId (из шаблона методички).
/// </summary>
public class CreateAssignmentRequest
{
    public string? Title { get; set; }
    public int? DepartmentId { get; set; }
    public int? SiteId { get; set; }
    public string? Description { get; set; }

    public int ResolvedDepartmentId => DepartmentId ?? SiteId ?? 0;
}
