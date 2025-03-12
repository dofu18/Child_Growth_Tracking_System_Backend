using Application.ResponseCode;
using ApplicationLayer.DTOs.Doctor;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IDoctorLicenseService
    {
        Task<IActionResult> HandleGetByUserIdAsync(Guid userId);
        Task<IActionResult> CreateDoctorProfile(DoctorDto dto);
        Task<IActionResult> ApproveDoctorProfile(Guid id);
        Task<IActionResult> GetAllDoctors();
        Task<IActionResult> UpdateDoctorProfile(Guid id, DoctorUpdateDto dto);
        Task<IActionResult> GetDoctorByBiography(string biography);
        Task<IActionResult> DeleteDoctorProfile(Guid id);
        /*Task<IActionResult> HideDoctorProfile(Guid doctorId, bool isHidden);*/
        Task<IActionResult> ShareDoctorProfile(Guid doctorId, Guid receiverId);
        Task<IActionResult> UpdateStatusDoctor(Guid doctorLicenseId, DoctorLicenseStatusEnum status);
    }


    public class DoctorLicenseService : BaseService, IDoctorLicenseService
    {
        private readonly IGenericRepository<DoctorLicense> _licenseRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<DoctorSpecialization> _specializationRepo;
        private readonly IGenericRepository<SharingProfile> _sharingRepo;

        public DoctorLicenseService(IGenericRepository<DoctorLicense> licenseRepo, IGenericRepository<User> userRepo, IGenericRepository<DoctorSpecialization> specializationRepo,IGenericRepository<SharingProfile> sharingRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _licenseRepo = licenseRepo;
            _userRepo = userRepo;
            _specializationRepo = specializationRepo;
            _sharingRepo = sharingRepo;
        }

        public async Task<IActionResult> CreateDoctorProfile(DoctorDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var doctorLicense = _mapper.Map<DoctorLicense>(dto);
            doctorLicense.UserId = userId;
            doctorLicense.CreatedAt = DateTime.Now;
            doctorLicense.UpdatedAt = DateTime.Now;
            doctorLicense.Status = DoctorLicenseStatusEnum.Pending; // Đợi admin duyệt

            var existingDoctorLicense = await _licenseRepo.FindByIdAsync(userId);
            if (existingDoctorLicense != null)
            {
                return ErrorResp.BadRequest("Doctor profile already exists for this user.");
            }

            await _licenseRepo.CreateAsync(doctorLicense);
            return SuccessResp.Created("Doctor profile information added successfully, pending admin approval.");
        }

        public async Task<IActionResult> ApproveDoctorProfile(Guid id)
        {
            // Find the doctor license by ID
            var doctorLicense = await _licenseRepo.FindByIdAsync(id);
            if (doctorLicense == null)
            {
                return ErrorResp.NotFound("Doctor profile not found.");
            }

            // Update the doctor license status to Approved
            doctorLicense.Status = DoctorLicenseStatusEnum.Published; // Assuming "Published" indicates approval
            doctorLicense.UpdatedAt = DateTime.Now;

            await _licenseRepo.UpdateAsync(doctorLicense);

            // Update the user's role to Doctor
            var user = await _userRepo.FindByIdAsync(doctorLicense.UserId);
            if (user == null)
            {
                return ErrorResp.NotFound("User not found.");
            }

            // Assuming you have a predefined GUID for the Doctor role
            var doctorRoleId = new Guid("INSERT_DOCTOR_ROLE_ID_HERE"); // Replace with the actual Doctor role ID
            user.RoleId = doctorRoleId; // Assign the Doctor role to the user
            await _userRepo.UpdateAsync(user);

            return SuccessResp.Ok("Doctor profile approved and user role updated successfully.");
        }

        public async Task<IActionResult> UpdateStatusDoctor(Guid doctorLicenseId, DoctorLicenseStatusEnum status)
        {
            // Tìm giấy phép bác sĩ bằng ID
            var doctorLicense = await _licenseRepo.FindByIdAsync(doctorLicenseId);
            if (doctorLicense == null)
            {
                return ErrorResp.NotFound("Doctor profile not found.");
            }

            // Cập nhật trạng thái giấy phép bác sĩ
            doctorLicense.Status = status;
            doctorLicense.UpdatedAt = DateTime.Now;

            await _licenseRepo.UpdateAsync(doctorLicense);

            return SuccessResp.Ok($"Doctor profile status updated to {status} successfully.");
        }


        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _licenseRepo.ListAsync();
            var result = _mapper.Map<List<DoctorDto>>(doctors);

            return SuccessResp.Ok(result);
        }


        public async Task<IActionResult> UpdateDoctorProfile(Guid id, DoctorUpdateDto dto)
        {

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            var doctor = await _licenseRepo.FindByIdAsync(id);
            if (doctor == null)
            {
                return ErrorResp.NotFound("Doctor Not Found");
            }

            if (doctor.UserId != Guid.Parse(userId.ToString()))
            {
                return ErrorResp.Forbidden("You are not authorized to update this profile");
            }

            if (dto == null)
            {
                return ErrorResp.BadRequest("Invalid Doctor Data");
            }

            _mapper.Map(dto, doctor);

            doctor.UpdatedAt = DateTime.Now;

            await _licenseRepo.UpdateAsync(doctor);

            return SuccessResp.Ok("Doctor profile updated successfully");
        }

        public async Task<IActionResult> GetDoctorByBiography(string biography)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(biography))
            {
                return ErrorResp.BadRequest("Biography cannot be empty");
            }

            // Retrieve all doctors (with navigation properties if necessary)
            var doctors = await _licenseRepo.ListAsync();

            // Filter doctors by biography in-memory (case-insensitive)
            var filteredDoctors = doctors
                .Where(d => d.Biography != null && d.Biography.Contains(biography, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Check if any results are found
            if (!filteredDoctors.Any())
            {
                return ErrorResp.NotFound("No doctors found with the provided biography keyword");
            }

            // Map results to DTOs
            var result = _mapper.Map<List<DoctorDto>>(filteredDoctors);

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> DeleteDoctorProfile(Guid id)
        {
            var currentUserId = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin")
            {
                return ErrorResp.Forbidden("You are not authorized to delete this profile");
            }

            var doctor = await _licenseRepo.FindByIdAsync(id);
            if (doctor == null)
            {
                return ErrorResp.NotFound("Doctor Not Found");
            }

            doctor.Status = DoctorLicenseStatusEnum.Archived;
            await _licenseRepo.UpdateAsync(doctor);

            return SuccessResp.Ok("Doctor profile archived successfully.");
        }


        /* public async Task<IActionResult> HideDoctorProfile(Guid doctorId, bool isHidden)
         {

         }*/

        public async Task<IActionResult> ShareDoctorProfile(Guid doctorId, Guid receiverId)
        {
            // Kiểm tra xem hồ sơ bác sĩ có tồn tại không
            var doctor = await _licenseRepo.FindByIdAsync(doctorId);
            if (doctor == null)
            {
                return ErrorResp.NotFound("Doctor not found.");
            }

            // Kiểm tra xem người nhận có tồn tại không
            var receiver = await _userRepo.FindByIdAsync(receiverId);
            if (receiver == null)
            {
                return ErrorResp.NotFound("Recipient not found.");
            }

            // Kiểm tra xem đã tồn tại bản ghi chia sẻ chưa
            //var existingShare = await _sharingRepo.WhereAsync(s => s.UserId == receiverId && s.DoctorId == doctorId);

            //if (existingShare.Any())
            //{
            //    return ErrorResp.BadRequest("This doctor’s profile has already been shared with the recipient.");
            //}

            // Tạo bản ghi mới trong bảng SharingProfiles
            var shareProfile = new SharingProfile
            {
                Id = Guid.NewGuid(),
                UserId = receiverId,
                //DoctorId = doctorId, // Tạo cột riêng cho DoctorId trong SharingProfile nếu cần
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _sharingRepo.CreateAsync(shareProfile);

            return SuccessResp.Ok("Doctor's profile has been shared successfully.");
        }

        public async Task<IActionResult> HandleGetByUserIdAsync(Guid userId)
        {
            var user = await _licenseRepo.WhereAsync(d => d.UserId.Equals(userId), "User");

            return SuccessResp.Ok(user);
        }
    }
}
