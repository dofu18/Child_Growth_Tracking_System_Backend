using static System.Net.Mime.MediaTypeNames;

namespace Exe.Controllers;

using Application.ResponseCode;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1")]
public class PingController : ControllerBase
{

    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        _logger.LogInformation("Ping request received");

        var resp = new BaseResp { Code = 200, Message = "pong" };

        return Ok(resp);
    }
}
