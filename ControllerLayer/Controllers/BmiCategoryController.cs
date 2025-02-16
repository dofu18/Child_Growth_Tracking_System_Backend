using ApplicationLayer.DTOs.BmiCategory;
using ApplicationLayer.Service;
using DomainLayer.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [Route(Constants.Http.API_VERSION + "/bmi-category")]
    public class BmiCategoryController : ControllerBase
    {
        private readonly IBmiCategoryService _bmiCategoryService;
        private ILogger<BmiCategoryController> _logger;

        public BmiCategoryController(ILogger<BmiCategoryController> logger, IBmiCategoryService bmiCategoryService)
        {
            _logger = logger;
            _bmiCategoryService = bmiCategoryService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BmiCategoryCreateDto dto)
        {
            _logger.LogInformation($"Create {Constants.Entities.BMI_CATEGORY} request received");

            return await _bmiCategoryService.Create(dto);
        }
    }
}
