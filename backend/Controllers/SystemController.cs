using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class SystemController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly IHostEnvironment _environment;

    public SystemController(IAssignmentService assignmentService, IHostEnvironment environment)
    {
        _assignmentService = assignmentService;
        _environment = environment;
    }

    [HttpGet("/health")]
    public IActionResult GetHealth()
    {
        return Ok("Healthy");
    }

    [HttpGet("/info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            app = _assignmentService.GetApplicationName(),
            environment = _environment.EnvironmentName,
            version = "ЛР8",
            note = _assignmentService.GetLabStageNote()
        });
    }
}
