using Application.ResponseCode;
using ApplicationLayer.DTOs.Consultation;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IConsultationService
    {
        Task<IActionResult> CreateConsultationRequest(ConsultationRequestDto dto);
        Task<IActionResult> RespondToConsultation(Guid requestId, ConsultationResponseDto dto);
        Task<IEnumerable<ConsultationHistoryDto>> GetConsultationHistory();
    }

    public class ConsultationService : BaseService, IConsultationService
    {
        private readonly IGenericRepository<ConsultationRequest> _requestRepo;
        private readonly IGenericRepository<ConsultationResponse> _responseRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<DoctorLicense> _doctorRepo;

        public ConsultationService(IGenericRepository<ConsultationRequest> requestRepo, IGenericRepository<ConsultationResponse> responseRepo, IGenericRepository<User> userRepo, IGenericRepository<DoctorLicense> doctorRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _requestRepo = requestRepo;
            _responseRepo = responseRepo;
            _userRepo = userRepo;
            _doctorRepo = doctorRepo;
        }

        public async Task<IActionResult> CreateConsultationRequest(ConsultationRequestDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            // Tìm bác sĩ phù hợp hoặc phân công ngẫu nhiên
            var doctor = await _doctorRepo.FindAsync(d => d.Status == DoctorLicenseStatusEnum.Published);

            if (doctor == null)
            {
                return ErrorResp.BadRequest("No available doctors at the moment.");
            }

            var consultationRequest = new ConsultationRequest
            {
                Id = Guid.NewGuid(),
                ChildrentId = dto.ChildrenId, //Sai chính tả trong khai báo entities và trong database check lại
                RequestDate = DateTime.Now,
                Description = dto.Description,
                Status = ConsultationRequestStatusEnum.Pending,
                Attachments = dto.Attachments,
                UserRequestId =  userId,
                Title = dto.Title,
                DoctorReceiveId = doctor.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _requestRepo.CreateAsync(consultationRequest);

            return SuccessResp.Created("Consultation request created successfully.", consultationRequest);
        }

        public async Task<IActionResult> RespondToConsultation(Guid requestId, ConsultationResponseDto dto)
        {
            var consultationRequest = await _requestRepo.FindByIdAsync(requestId);
            if (consultationRequest == null)
            {
                return ErrorResp.NotFound("Consultation request not found.");
            }

            var consultationResponse = new ConsultationResponse
            {
                Id = Guid.NewGuid(),
                RequestId = requestId,
                DoctorId = consultationRequest.DoctorReceiveId,
                Title = dto.Title,
                Content = dto.Content,
                Attachments = dto.Attachments,
                ResponseDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _responseRepo.CreateAsync(consultationResponse);

            consultationRequest.Status = ConsultationRequestStatusEnum.Archived; // Cập nhật trạng thái tư vấn
            consultationRequest.UpdatedAt = DateTime.Now;

            await _requestRepo.UpdateAsync(consultationRequest);

            return SuccessResp.Ok("Response sent successfully.", consultationResponse);
        }

        public async Task<IEnumerable<ConsultationHistoryDto>> GetConsultationHistory()
        {
            // Extract user payload from the token
            var payload = ExtractPayload();
            if (payload == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            var userId = payload.UserId;

            // Fetch consultations for the user
            var consultations = await _requestRepo.FindAllAsync(r => r.UserRequestId == userId);

            // Map consultations to DTOs
            var history = consultations.Select(c => new ConsultationHistoryDto
            {
                RequestId = c.Id,
                Title = c.Title,
                DoctorName = GetDoctorName(c.DoctorReceiveId),
                Status = MapStatusToDisplayString(c.Status), // Custom mapping for the enum
                RequestDate = c.RequestDate,
                ResponseContent = GetResponseContent(c.Id)
            });

            return history; // Return IEnumerable<ConsultationHistoryDto>
        }

        private string MapStatusToDisplayString(ConsultationRequestStatusEnum status)
        {
            return status switch
            {
                ConsultationRequestStatusEnum.Pending => "Pending Approval",
                ConsultationRequestStatusEnum.InProgress => "In Progress",
                ConsultationRequestStatusEnum.Completed => "Completed",
                _ => "Unknown Status"
            };
        }

        private string GetDoctorName(Guid doctorId)
        {
            // Tìm giấy phép bác sĩ trong DoctorLicense bằng doctorId
            var doctorLicense = _doctorRepo.FindByIdAsync(doctorId).Result;
            if (doctorLicense == null)
            {
                return "Unknown Doctor";
            }

            // Sử dụng UserId từ DoctorLicense để lấy thông tin User
            var user = _userRepo.FindByIdAsync(doctorLicense.UserId).Result;
            return user?.Name ?? "Unknown Doctor";
        }

        private string GetResponseContent(Guid requestId)
        {
            var response = _responseRepo.FindAsync(r => r.RequestId == requestId).Result;
            return response?.Content ?? "No response yet.";
        }
    }

}
