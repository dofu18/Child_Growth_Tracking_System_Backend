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
        private readonly IGenericRepository<DoctorLicense> _licenseRepo;
        private readonly IGenericRepository<User> _userRepo;



        public DoctorController(ILogger<DoctorController> logger, IDoctorLicenseService doctorService,IGenericRepository<DoctorLicense> licenseRepo,IGenericRepository<User> userRepo)
        {
            _logger = logger;
            _doctorService = doctorService;
            _licenseRepo = licenseRepo;
            _userRepo = userRepo;
        }


        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreateDoctorProfile([FromBody] DoctorDto dto)
        {
            _logger.LogInformation("Create Doctor request received");
            return await _doctorService.CreateDoctorProfile(dto);
        }

        [HttpPut("approve-doctor-profile/{doctorLicenseId}")]
        [Authorize(Roles = "Admin")] // Ensure only admins can access this endpoint
        public async Task<IActionResult> ApproveDoctorProfile(Guid doctorLicenseId)
        {
            // Find the doctor license by ID
            var doctorLicense = await _licenseRepo.FindByIdAsync(doctorLicenseId);
            if (doctorLicense == null)
            {
                return ErrorResp.NotFound("Doctor profile not found.");
            }

            // Update the doctor license status to Published
            doctorLicense.Status = DoctorLicenseStatusEnum.Published; // Assuming "Published" means approval
            doctorLicense.UpdatedAt = DateTime.Now;

            await _licenseRepo.UpdateAsync(doctorLicense);

            // Update the user's role to Doctor
            var user = await _userRepo.FindByIdAsync(doctorLicense.UserId);
            if (user == null)
            {
                return ErrorResp.NotFound("User not found.");
            }

            // Assuming you have the predefined GUID for the Doctor role
            var doctorRoleId = new Guid("INSERT_DOCTOR_ROLE_ID_HERE"); // Replace this with the actual role ID
            user.RoleId = doctorRoleId; // Assign the Doctor role
            await _userRepo.UpdateAsync(user);

            return SuccessResp.Ok("Doctor profile approved and user role updated successfully.");
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
    }
}
