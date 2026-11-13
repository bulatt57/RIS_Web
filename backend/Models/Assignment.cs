namespace backend.Models;

/// <summary>
/// Главный объект — Поручение (таблица assignments в PostgreSQL).
/// </summary>
public class Assignment
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Status { get; set; } = "New";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public int CreatedByUserId { get; set; } = 1;
    public int? AssigneeUserId { get; set; }

    public Department? Department { get; set; }
    public User? CreatedByUser { get; set; }
    public User? AssigneeUser { get; set; }
}
