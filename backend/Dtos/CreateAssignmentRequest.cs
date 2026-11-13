namespace backend.Dtos;

/// <summary>
/// Входной DTO для создания поручения (клиент передаёт только изменяемые поля).
/// </summary>
public class CreateAssignmentRequest
{
    public string? Title { get; set; }
    public int? DepartmentId { get; set; }
    public int? SiteId { get; set; }
    public string? Description { get; set; }
    public int? CreatedByUserId { get; set; }

    public int ResolvedDepartmentId => DepartmentId ?? SiteId ?? 0;
}
