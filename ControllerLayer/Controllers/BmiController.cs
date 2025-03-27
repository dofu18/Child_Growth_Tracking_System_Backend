using ApplicationLayer.DTOs.BMI;
using ApplicationLayer.DTOs.GrowthRecord;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/bmi")]
    public class BmiController : ControllerBase
    {
        private readonly IBmiService _bmiService;
        private readonly IGrowthTrackingService _growthTrackingService;
        private readonly ILogger<BmiController> _logger;

        public BmiController(IBmiService bmiService, IGrowthTrackingService growthTrackingService, ILogger<BmiController> logger)
        {
            _bmiService = bmiService;
            _growthTrackingService = growthTrackingService;
            _logger = logger;
        }

        [Protected]
        [HttpPost("save")]
        public async Task<IActionResult> SaveGrowthRecord([FromBody] SaveGrowthRecordRequestDto request)
        {
            var result = await _bmiService.SaveGrowthRecordAsync(request);
            return Ok(result);
        }

        [Protected]
        [HttpGet("tracking")]
        public async Task<IActionResult> GrowthTracking([FromQuery] Guid childId, DateTime? startDate, DateTime? endDate)
        {
            var result = await _growthTrackingService.GetGrowthTracking(childId, startDate, endDate);
            return Ok(result);
        }
    }
}
