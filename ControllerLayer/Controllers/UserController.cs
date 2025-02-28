using ApplicationLayer.DTOs.User;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;

        public UserController(ILogger<UserController> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        [Protected]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            _logger.LogInformation("Get user profile request received");

            return await _service.HandleGetByIdAsync();
        }

        [Protected]
        [HttpPut("update-profile")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto req)
        {
            _logger.LogInformation("Update user request received");

            return await _service.HandleUpdateAsync(req);
        }
    }
}
