using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class SystemController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly IHostEnvironment _environment;

    // Зависимость внедряется через конструктор (DI), без использования new AssignmentService()
    public SystemController(IAssignmentService assignmentService, IHostEnvironment environment)
    {
        _assignmentService = assignmentService;
        _environment = environment;
    }

    /// <summary>
    /// Проверка живости процесса (не создаёт записи и не принимает поля заявки).
    /// </summary>
    [HttpGet("/health")]
    public IActionResult GetHealth()
    {
        return Ok("Healthy");
    }

    /// <summary>
    /// Сведения о приложении, текущей среде и этапе лабораторной работы.
    /// </summary>
    [HttpGet("/info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            app = _assignmentService.GetApplicationName(),
            environment = _environment.EnvironmentName,
            version = "ЛР7",
            note = _assignmentService.GetLabStageNote()
        });
    }
}
