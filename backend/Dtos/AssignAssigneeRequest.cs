namespace backend.Dtos;

/// <summary>
/// Входной DTO для отдельного назначения исполнителя: PATCH /api/assignments/{id}/assignee
/// </summary>
public class AssignAssigneeRequest
{
    public int AssigneeUserId { get; set; }
}
