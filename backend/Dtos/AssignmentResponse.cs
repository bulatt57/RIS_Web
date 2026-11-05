namespace backend.Dtos;

/// <summary>
/// Выходной DTO поручения, возвращаемый клиенту.
/// Содержит серверные поля id, number, status и assigneeUserId.
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
    public int? AssigneeUserId { get; set; }
}
