using ApplicationLayer.DTOs.Consultation.ConsultationRequests;
using ApplicationLayer.DTOs.Consultation.ConsultationResponses;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/response")]
    public class ConsultationResponseController : ControllerBase
    {
        private readonly IConsultationResponseService _responseService;
        private readonly ILogger<ConsultationResponseController> _logger;

        public ConsultationResponseController(ILogger<ConsultationResponseController> logger, IConsultationResponseService responseService)
        {
            _logger = logger;
            _responseService = responseService;
        }

        [Protected]
        [HttpPost("send")]
        public async Task<IActionResult> SendResponse([FromBody] ConsultationResponseCreateDto dto, [FromQuery] Guid requestId)
        {
            _logger.LogInformation("Send user response received");

            return await _responseService.SendResponse(dto, requestId);
        }

        [Protected]
        [HttpGet("request-{requestId}")]
        public async Task<IActionResult> GetMyResponseOfRequest([FromQuery] ConsultationResponseQuery query, Guid requestId)
        {
            _logger.LogInformation("Get my user request response received");

            return await _responseService.GetByRequestId(query, requestId);
        }

        [Protected]
        [HttpGet("doctor-response")]
        public async Task<IActionResult> DoctorGetMyResponse([FromQuery] ConsultationResponseQuery query)
        {
            _logger.LogInformation("Get doctor response received");

            return await _responseService.DoctorGetMyResponse(query);
        }
    }
}
