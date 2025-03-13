using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using InfrastructureLayer.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/children")]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildrenService _childrenService;
        private readonly ILogger<ChildrenController> _logger;

        public ChildrenController(ILogger<ChildrenController> logger, IChildrenService childrenService)
        {
            _logger = logger;
            _childrenService = childrenService;
        }

        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ChildrenCreateDto dto)
        {
            _logger.LogInformation("Create Children request received");

            return await _childrenService.Create(dto);
        }

        [Protected]
        [HttpGet("getChildByToken")]
        public async Task<IActionResult> GetChildByToken()
        {
            _logger.LogInformation("Get all children by parent request received");

            return await _childrenService.GetChildByToken();
        }

        [Protected]
        [HttpGet("getChildByParent")]
        public async Task<IActionResult> GetChildByParent(Guid parentId)
        {
            _logger.LogInformation("Get all children by parent request received");

            return await _childrenService.GetChildByParent(parentId);
        }

        [Protected]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all children request received");

            return await _childrenService.GetAll();
        }

        [Protected]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ChildrenUpdateDto dto)
        {
            return await _childrenService.Update(id, dto);
        }

        [Protected]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Delete children request received for ID: {id}");

            return await _childrenService.Delete(id);
        }

        [Protected]
        [HttpPost("hideChildren/{id}")]
        public async Task<IActionResult> HideChildren(Guid id, [FromBody] bool isHidden)
        {
            return await _childrenService.HideChildren(id, isHidden);
        }

        [Protected]
        [HttpPost("{childId}/share")]
        public async Task<IActionResult> ShareProfile([FromRoute] Guid childId, [FromQuery] string recipientEmail)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return BadRequest("Recipient email is required.");
            }
            return await _childrenService.SharingProfile(childId, recipientEmail);
        }

        [Protected]
        [HttpGet("shared")]
        public async Task<IActionResult> GetSharedChildren()
        {
            return await _childrenService.GetSharedChildren();
        }
    }
}
