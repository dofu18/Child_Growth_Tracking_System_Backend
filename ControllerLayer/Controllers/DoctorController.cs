using Application.ResponseCode;
using ApplicationLayer.DTOs.Doctor;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

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


        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreateDoctorProfile([FromBody] DoctorDto dto)
        {
            _logger.LogInformation("Create Doctor request received");
            return await _doctorService.CreateDoctorProfile(dto);
        }

        [HttpPut("approve-doctor-profile/{doctorLicenseId}")]
        [Authorize(Roles = "Admin")] // Chỉ cho phép Admin truy cập
        public async Task<IActionResult> ApproveDoctorProfile(Guid doctorLicenseId)
        {
            return await _doctorService.ApproveDoctorProfile(doctorLicenseId);
        }

        [HttpPut("update-doctor-status/{doctorLicenseId}")]
        [Authorize(Roles = "Admin")] // Chỉ dành cho Admin
        public async Task<IActionResult> UpdateStatusDoctor(Guid doctorLicenseId, [FromBody] DoctorLicenseStatusEnum status)
        {
            // Gọi service xử lý logic
            return await _doctorService.UpdateStatusDoctor(doctorLicenseId, status);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            _logger.LogInformation("Get all doctors request received");
            return await _doctorService.GetAllDoctors();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctorProfile(Guid id, [FromBody] DoctorUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Request body cannot be null");
            }

            // Gọi tới service để xử lý logic
            var result = await _doctorService.UpdateDoctorProfile(id, dto);

            // Trả về kết quả từ service (thành công hoặc lỗi)
            return result;
        }

        [HttpGet("biography/{biography}")]
        public async Task<IActionResult> GetDoctorByBiography([FromRoute] string biography)
        {
            return await _doctorService.GetDoctorByBiography(biography);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorProfile([FromRoute] Guid id)
        {
            return await _doctorService.DeleteDoctorProfile(id);
        }

        [HttpPost("{doctorId}/share/{receiverId}")]
        public async Task<IActionResult> ShareDoctorProfile(Guid doctorId, Guid receiverId)
        {
            return await _doctorService.ShareDoctorProfile(doctorId, receiverId);
        }

        [HttpGet("doctorprofile/{userId}")]
        public async Task<IActionResult> GetDoctorProfile([FromRoute] Guid userId)
        {
            return await _doctorService.HandleGetByUserIdAsync(userId);
        }
    }
}
