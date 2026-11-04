namespace backend.Models;

// Модель-заготовка главного объекта Поручение (Assignment) для последующих лабораторных работ
public class AssignmentModel
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
}
