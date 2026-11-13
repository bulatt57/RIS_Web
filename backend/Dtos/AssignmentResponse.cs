namespace backend.Dtos;

/// <summary>
/// Выходной DTO поручения, возвращаемый клиенту из БД PostgreSQL.
/// </summary>
public class AssignmentResponse
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int SiteId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }
}
