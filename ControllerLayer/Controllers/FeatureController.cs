using ApplicationLayer.DTOs.Feature;
using ApplicationLayer.DTOs.RatingFeedback;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using DomainLayer.Constants;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/feature")]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _service;
        private ILogger<FeatureController> _logger;

        public FeatureController(ILogger<FeatureController> logger, IFeatureService service)
        {
            _logger = logger;
            _service = service;
        }

        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FeatureCreateDto dto)
        {
            _logger.LogInformation($"Create {Constants.Entities.FEATURE} request received");

            return await _service.Create(dto);
        }

        [Protected]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] FeatureQuery req)
        {
            _logger.LogInformation("Get all feature request received");

            return await _service.GetAllFeature(req);
        }

        [Protected]
        [HttpPut("{featureId}")]
        public async Task<IActionResult> Update(Guid featureId, [FromBody] FeatureUpdateDto dto)
        {
            _logger.LogInformation("Update feature request received");

            return await _service.UpdateAsync(featureId, dto);
        }

        [Protected]
        [HttpDelete("{featureId}")]
        public async Task<IActionResult> Delete(Guid featureId)
        {
            _logger.LogInformation("Delete feature request received");

            return await _service.DeleteAsync(featureId);
        }
    }
}
