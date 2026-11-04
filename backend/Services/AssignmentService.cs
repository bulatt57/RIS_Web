namespace backend.Services;

public class AssignmentService : IAssignmentService
{
    public string GetApplicationName() => "TaskControl (ПАО «Таттелеком» — ЕЦУиМС)";

    public string GetLabStageNote() => "ЛР7: каркас ASP.NET Core Web API (создание поручений появится в ЛР8)";
}
