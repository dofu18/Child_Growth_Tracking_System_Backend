using ApplicationLayer.DTOs.Childrens;
using ApplicationLayer.DTOs.Consultation.ConsultationRequests;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/request")]
    public class ConsultationRequestController : ControllerBase
    {
        private readonly IConsultationRequestService _requestService;
        private readonly ILogger<ConsultationRequestController> _logger;

        public ConsultationRequestController(ILogger<ConsultationRequestController> logger, IConsultationRequestService requestService)
        {
            _logger = logger;
            _requestService = requestService;
        }

        [Protected]
        [HttpPost("send")]
        public async Task<IActionResult> SendRequest([FromBody] ConsultationRequestCreateDto dto, [FromQuery] Guid doctorReceiveId)
        {
            _logger.LogInformation("Send doctor request received");

            return await _requestService.SendRequest(dto, doctorReceiveId);
        }

        [Protected]
        [HttpGet("my-request")]
        public async Task<IActionResult> GetMyRequest([FromQuery] ConsultationRequestQuery query, ConsultationRequestStatusEnum? status)
        {
            _logger.LogInformation("Get my user request received");

            return await _requestService.UserGetMyRequest(query, status);
        }

        [Protected]
        [HttpGet("doctor-request")]
        public async Task<IActionResult> DoctorGetMyRequest([FromQuery] ConsultationRequestQuery query, ConsultationRequestStatusEnum? status)
        {
            _logger.LogInformation("Get doctor request received");

            return await _requestService.DoctorGetMyRequest(query, status);
        }
    }
}
