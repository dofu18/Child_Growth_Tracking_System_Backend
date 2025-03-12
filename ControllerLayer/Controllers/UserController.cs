using ApplicationLayer.DTOs.Users;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

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


        //admin
        [Protected]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] UserQuery req, UserStatusEnum? status)
        {
            _logger.LogInformation("Admin get all users request received");

            return await _service.GetAllUserAsync(req, status);
        }
        [Protected]
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] UserStatusEnum status)
        {
            _logger.LogInformation("Modify User status request received");

            return await _service.HandleStatusAsync(id, status);
        }
        [Protected]
        [HttpPatch("{userId}/{roleId}")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid userId, Guid roleId)
        {
            _logger.LogInformation("Modify User role request received");

            return await _service.HandleRoleAsync(userId, roleId);
        }
    }
}
