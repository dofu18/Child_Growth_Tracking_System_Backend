using ApplicationLayer.DTOs.BmiCategory;
using ApplicationLayer.DTOs.RatingFeedback;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using DomainLayer.Constants;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ControllerLayer.Controllers
{
    [Route(Constants.Http.API_VERSION + "/rating-feedback")]

    public class RatingFeedbackController : ControllerBase 
    {
        private readonly IRatingFeedbackService _ratingFeedbackService;
        private ILogger<RatingFeedbackController> _logger;

        public RatingFeedbackController(ILogger<RatingFeedbackController> logger, IRatingFeedbackService ratingFeedbackService)
        {
            _logger = logger;
            _ratingFeedbackService = ratingFeedbackService;
        }

        [Protected]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RatingFeedbackCreateDto dto)
        {
            _logger.LogInformation($"Create {Constants.Entities.RATING_FEEDBACK} request received");

            return await _ratingFeedbackService.Create(dto);
        }

        [Protected]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAll([FromQuery] RatingFeedbackQuery req, RatingFeedbackStatusEnum? status)
        {
            _logger.LogInformation("Admin get all rating feedback request received");

            return await _ratingFeedbackService.AdminGetAllRating(req, status);
        }

        [Protected]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RatingFeedbackUpdateDto dto)
        {
            _logger.LogInformation("Update feedback request received");

            return await _ratingFeedbackService.HandleUpdateAsync(id, dto);
        }
        [Protected]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] RatingFeedbackStatusEnum status)
        {
            _logger.LogInformation("Modify Feedvacj status request received");

            return await _ratingFeedbackService.HandleStatusAsync(id, status);
        }
    }
}
