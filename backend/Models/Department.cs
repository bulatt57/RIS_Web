namespace backend.Models;

/// <summary>
/// Справочник подразделений ЕЦУиМС ПАО «Таттелеком» (таблица departments в PostgreSQL).
/// </summary>
public class Department
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}
