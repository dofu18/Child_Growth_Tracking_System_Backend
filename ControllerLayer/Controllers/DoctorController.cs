using ApplicationLayer.DTOs.Doctor;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [Route("api/v1/doctors")]
    //[Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorLicenseService _doctorService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger, IDoctorLicenseService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDoctorProfile([FromBody] DoctorDto dto)
        {
            _logger.LogInformation("Create Doctor request received");
            return await _doctorService.CreateDoctorProfile(dto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            _logger.LogInformation("Get all doctors request received");
            return await _doctorService.GetAllDoctors();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctorProfile(Guid id, [FromBody] DoctorDto dto)
        {
            return await _doctorService.UpdateDoctorProfile(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorProfile([FromRoute] Guid id)
        {
            return await _doctorService.DeleteDoctorProfile(id);
        }
    }
}
