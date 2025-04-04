using Application.ResponseCode;
using ApplicationLayer.DTOs.WHOData;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/WHOData")]
    public class WHODataController
    {
        private readonly IWHODataService _whoDataService;
        private readonly ILogger<WHODataController> _logger;

        public WHODataController(IWHODataService whoDataService, ILogger<WHODataController> logger)
        {
            _whoDataService = whoDataService;
            _logger = logger;
        }

        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreateData([FromBody] WhoDataDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("CreateData request received with null DTO");
                return ErrorResp.BadRequest("Invalid data");
            }

            _logger.LogInformation("Request received: Creating WHO Data - AgeMonth: {AgeMonth}, Gender: {Gender}", dto.AgeMonth, dto.Gender);
            return await _whoDataService.CreateData(dto);
        }

        [Protected]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateData([FromBody] WhoDataUpdateDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("UpdateData request received with null DTO.");
                return ErrorResp.BadRequest("Invalid data.");
            }

            _logger.LogInformation("Request received: Updating WHO Data - ID: {Id}", dto.Id);
            return await _whoDataService.UpdateData(dto);
        }

        [Protected]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteData(Guid id)
        {
            _logger.LogInformation("Request received: Deleting WHO Data - ID: {Id}", id);
            return await _whoDataService.DeleteData(id);
        }

        [Protected]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllData()
        {
            _logger.LogInformation("Request received: Fetching all WHO Data");
            return await _whoDataService.GetAllData();
        }

        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDataById(Guid id)
        {
            _logger.LogInformation("Request received: Fetching WHO Data by {Id}", id);
            return await _whoDataService.GetDataById(id);
        }

        [Protected]
        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetDataByGender(GenderEnum gender)
        {
            _logger.LogInformation("Request received: Fetching WHO Data by {Gender}", gender);
            return await _whoDataService.GetDataByGender(gender);
        }
    }
}
