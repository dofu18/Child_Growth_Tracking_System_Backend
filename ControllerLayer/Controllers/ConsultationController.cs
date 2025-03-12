using ApplicationLayer.DTOs.Consultation;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/consultations")]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        private readonly ILogger<ConsultationController> _logger;

        public ConsultationController(ILogger<ConsultationController> logger, IConsultationService consultationService)
        {
            _logger = logger;
            _consultationService = consultationService;
        }

        [HttpPost("create")]
        [Authorize] // Chỉ dành cho người dùng đã đăng nhập
        public async Task<IActionResult> CreateConsultationRequest([FromBody] ConsultationRequestDto dto)
        {
            _logger.LogInformation("Create Consultation Request received");
            return await _consultationService.CreateConsultationRequest(dto);
        }

        [HttpPost("{requestId}/response")]
        [Authorize(Roles = "Doctor")] // Chỉ dành cho bác sĩ
        public async Task<IActionResult> RespondToConsultation(Guid requestId, [FromBody] ConsultationResponseDto dto)
        {
            _logger.LogInformation($"Response to Consultation Request {requestId} received");
            return await _consultationService.RespondToConsultation(requestId, dto);
        }

        [HttpGet("history")]
        [Authorize] // Ensure the user is authenticated
        public async Task<IActionResult> GetConsultationHistory()
        {
            _logger.LogInformation("Get Consultation History request received");

            // Call the service to get the consultation history
            var history = await _consultationService.GetConsultationHistory();

            // Wrap the result in an Ok object to return as IActionResult
            return Ok(history);
        }
    }
}
