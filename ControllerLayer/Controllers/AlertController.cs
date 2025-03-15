using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Alert;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("/api/v1/alerts")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly ILogger<AlertController> _logger;

        public AlertController(IAlertService alertService, ILogger<AlertController> logger)
        {
            _alertService = alertService;
            _logger = logger;
        }


        [Protected] // Ensures only authorized users can call this
        [HttpPost("check-and-send")]
        public async Task<IActionResult> CheckAndSendHealthAlerts([FromBody] CheckHealthAlertRequestDto requestDto)
        {
            _logger.LogInformation($"Checking and sending health alerts for childId: {requestDto.ChildId}");
            return await _alertService.CheckAndSendHealthAlerts(requestDto.ChildId, requestDto.Bmi);
        }


        [Protected]
        [HttpGet("{userId}/health-alerts")]
        public async Task<IActionResult> GetHealthAlertsByUser(Guid userId)
        {
            _logger.LogInformation($"Fetching health alerts for userId: {userId}");
            var alerts = await _alertService.GetHealthAlertsByUser(userId);
            return Ok(alerts);
        }


        [Protected]
        [HttpPatch("{alertId}/mark-as-read")]
        public async Task<IActionResult> MarkAlertAsRead(Guid alertId)
        {
            _logger.LogInformation($"Marking alert as read for alertId: {alertId}");
            await _alertService.MarkAlertAsRead(alertId);
            return Ok(new { message = "Alert marked as read successfully." });
        }
    }
}
