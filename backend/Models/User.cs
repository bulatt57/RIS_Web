namespace backend.Models;

/// <summary>
/// Пользователь системы: создатель поручений или исполнитель (таблица users в PostgreSQL).
/// </summary>
public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Engineer";

    public ICollection<Assignment> CreatedAssignments { get; set; } = new List<Assignment>();
    public ICollection<Assignment> AssignedAssignments { get; set; } = new List<Assignment>();
}
