using backend.Dtos;

namespace backend.Services;

public enum OperationErrorType
{
    None,
    Validation,
    NotFound,
    Conflict
}

public class ServiceResult<T>
{
    public bool IsSuccess => ErrorType == OperationErrorType.None;
    public OperationErrorType ErrorType { get; init; } = OperationErrorType.None;
    public string ErrorMessage { get; init; } = string.Empty;
    public IDictionary<string, string[]>? ValidationErrors { get; init; }
    public T? Data { get; init; }

    public static ServiceResult<T> Success(T data) => new()
    {
        ErrorType = OperationErrorType.None,
        Data = data
    };

    public static ServiceResult<T> Validation(string message, IDictionary<string, string[]>? errors = null) => new()
    {
        ErrorType = OperationErrorType.Validation,
        ErrorMessage = message,
        ValidationErrors = errors
    };

    public static ServiceResult<T> NotFound(string message) => new()
    {
        ErrorType = OperationErrorType.NotFound,
        ErrorMessage = message
    };

    public static ServiceResult<T> Conflict(string message) => new()
    {
        ErrorType = OperationErrorType.Conflict,
        ErrorMessage = message
    };
}

public interface IAssignmentService
{
    string GetApplicationName();
    string GetLabStageNote();
    IReadOnlyList<AssignmentResponse> GetAll();
    AssignmentResponse? GetById(int id);
    ServiceResult<AssignmentResponse> Create(CreateAssignmentRequest? request);
    ServiceResult<AssignmentResponse> AssignUser(int id, AssignAssigneeRequest? request);
    ServiceResult<AssignmentResponse> ChangeStatus(int id, ChangeStatusRequest? request);
}
