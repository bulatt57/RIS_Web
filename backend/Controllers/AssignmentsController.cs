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

    [HttpGet]
    public ActionResult<IReadOnlyList<AssignmentResponse>> GetAll()
    {
        return Ok(_assignmentService.GetAll());
    }

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

            var created = result.Data!;
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
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

    [HttpPatch("{id:int}/assignee")]
    public ActionResult<AssignmentResponse> AssignAssignee(int id, [FromBody] AssignAssigneeRequest? request)
    {
        var result = _assignmentService.AssignUser(id, request);
        return ToActionResult(result);
    }

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
