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
        Task<IActionResult> CreateDoctorProfile(DoctorDto dto);
        Task<IActionResult> GetAllDoctors();
        Task<IActionResult> UpdateDoctorProfile(Guid id, DoctorDto dto);
       /* Task<IActionResult> GetDoctorByLicenseNumber(string licenseNumber);*/
        Task<IActionResult> DeleteDoctorProfile(Guid id);
        /*Task<IActionResult> HideDoctorProfile(Guid doctorId, bool isHidden);*/
        /*Task<IActionResult> ShareDoctorProfile(Guid doctorId, Guid receiverId);*/
    }

    public class DoctorLicenseService : BaseService, IDoctorLicenseService
    {
        private readonly IGenericRepository<DoctorLicense> _licenseRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<DoctorSpecialization> _specializationRepo;

        public DoctorLicenseService(IGenericRepository<DoctorLicense> licenseRepo, IGenericRepository<User> userRepo, IGenericRepository<DoctorSpecialization> specializationRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _licenseRepo = licenseRepo;
            _userRepo = userRepo;
            _specializationRepo = specializationRepo;
        }

        public async Task<IActionResult> CreateDoctorProfile(DoctorDto dto)
        {
            var doctorLicense = _mapper.Map<DoctorLicense>(dto);

            doctorLicense.CreatedAt = DateTime.Now;
            doctorLicense.UpdatedAt = DateTime.Now;
            doctorLicense.Status = DoctorLicenseStatusEnum.Pending; // Đợi admin duyệt

            await _licenseRepo.CreateAsync(doctorLicense);
            return SuccessResp.Created("Doctor profile information added successfully, pending admin approval.");
        }


        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _licenseRepo.ListAsync();
            var result = _mapper.Map<List<DoctorDto>>(doctors);

            return SuccessResp.Ok(result);
        }


        public async Task<IActionResult> UpdateDoctorProfile(Guid id, DoctorDto dto)
        {
            var currentUserId = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var doctor = await _licenseRepo.FindByIdAsync(id);
            if (doctor == null)
            {
                return ErrorResp.NotFound("Doctor Not Found");
            }

            if (doctor.UserId.ToString() != currentUserId)
            {
                return ErrorResp.Forbidden("You are not authorized to update this profile");
            }

            _mapper.Map(dto, doctor);

            await _licenseRepo.UpdateAsync(doctor);

            return SuccessResp.Ok("Doctor profile updated successfully");
        }


        /*public async Task<IActionResult> GetDoctorByLicenseNumber(string licenseNumber)
        {

        }*/

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

       /* public async Task<IActionResult> ShareDoctorProfile(Guid doctorId, Guid receiverId)
        {
            
        }*/
    }
}
