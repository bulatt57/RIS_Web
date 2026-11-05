using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/assignments")]
[Route("api/tickets")]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly ILogger<AssignmentsController> _logger;

    public AssignmentsController(
        IAssignmentService assignmentService,
        ILogger<AssignmentsController> logger)
    {
        _assignmentService = assignmentService;
        _logger = logger;
    }

    /// <summary>
    /// Получить список всех поручений (включая пустой массив [] со статусом 200).
    /// </summary>
    [HttpGet]
    public ActionResult<IReadOnlyList<AssignmentResponse>> GetAll()
    {
        return Ok(_assignmentService.GetAll());
    }

    /// <summary>
    /// Получить одно поручение по id (200 или 404).
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<AssignmentResponse> GetById(int id)
    {
        var item = _assignmentService.GetById(id);
        if (item is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Запись не найдена",
                Detail = $"Поручение с id={id} не существует."
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Создать новое поручение (201 Created или 400 Bad Request).
    /// </summary>
    [HttpPost]
    public ActionResult<AssignmentResponse> Create([FromBody] CreateAssignmentRequest? request)
    {
        var departmentId = request?.ResolvedDepartmentId ?? 0;

        try
        {
            var result = _assignmentService.Create(request);
            if (!result.IsSuccess)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.ValidationErrors ?? new Dictionary<string, string[]>())
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Ошибка проверки полей",
                    Detail = result.ErrorMessage
                });
            }

            // Для проверки п. 5.3 методички (раскомментировать временно для скриншота лога 500, затем снова закомментировать):
            // throw new InvalidOperationException("Simulated unexpected failure after validation");

            var created = result.Data!;
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            // Пункт 5.3: одна понятная строка в логе сервера и безопасный ProblemDetails клиенту без stack trace
            _logger.LogError(
                "CreateAssignment failed; departmentId={DepartmentId}; error={ErrorType}",
                departmentId,
                ex.GetType().Name);

            return Problem(
                title: "Сервер не смог обработать запрос",
                detail: "Неожиданный сбой при создании записи.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Отдельно назначить исполнителя поручения (200, 400 или 404).
    /// </summary>
    [HttpPatch("{id:int}/assignee")]
    public ActionResult<AssignmentResponse> AssignAssignee(int id, [FromBody] AssignAssigneeRequest? request)
    {
        var result = _assignmentService.AssignUser(id, request);
        return ToActionResult(result);
    }

    /// <summary>
    /// Отдельно изменить статус поручения (200, 400, 404 или 409 при отсутствии исполнителя для InProgress).
    /// </summary>
    [HttpPatch("{id:int}/status")]
    public ActionResult<AssignmentResponse> ChangeStatus(int id, [FromBody] ChangeStatusRequest? request)
    {
        var result = _assignmentService.ChangeStatus(id, request);
        return ToActionResult(result);
    }

    private ActionResult<AssignmentResponse> ToActionResult(ServiceResult<AssignmentResponse> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return result.ErrorType switch
        {
            OperationErrorType.Validation => ValidationProblem(new ValidationProblemDetails(
                result.ValidationErrors ?? new Dictionary<string, string[]>())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ошибка проверки полей",
                Detail = result.ErrorMessage
            }),
            OperationErrorType.NotFound => NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Запись не найдена",
                Detail = result.ErrorMessage
            }),
            OperationErrorType.Conflict => Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Конфликт состояния процесса",
                Detail = result.ErrorMessage
            }),
            _ => Problem(
                title: "Сервер не смог обработать запрос",
                detail: result.ErrorMessage,
                statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
