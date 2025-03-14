﻿using ApplicationLayer.DTOs.BMI;
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
        private readonly ILogger<BmiController> _logger;

        public BmiController(IBmiService bmiService, ILogger<BmiController> logger)
        {
            _bmiService = bmiService;
            _logger = logger;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateBmi([FromBody] CalculateBmiRequestDto request)
        {
            var result = await _bmiService.CalculateBmiAsync(request);
            return Ok(result);
        }

        [Protected]
        [HttpPost("save")]
        public async Task<IActionResult> SaveGrowthRecord([FromBody] SaveGrowthRecordRequestDto request)
        {
            var result = await _bmiService.SaveGrowthRecordAsync(request);
            return Ok(result);
        }
    }
}
