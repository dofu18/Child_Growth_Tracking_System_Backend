using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.Consultation.ConsultationRequests;
using ApplicationLayer.Shared;
using AutoMapper;
using DomainLayer.Constants;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IConsultationRequestService
    {
        Task<IActionResult> SendRequest(ConsultationRequestCreateDto dto, Guid doctorReceiveId);
        Task<IActionResult> UserGetMyRequest(ConsultationRequestQuery query, ConsultationRequestStatusEnum? status);
        Task<IActionResult> DoctorGetMyRequest(ConsultationRequestQuery query, ConsultationRequestStatusEnum? status);

    }
    public class ConsultationRequestService : BaseService, IConsultationRequestService
    {
        private readonly IGenericRepository<ConsultationRequest> _requestRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Children> _childrenRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<UserPackage> _userPackageRepo;

        public ConsultationRequestService(IGenericRepository<ConsultationRequest> requestRepo, IGenericRepository<Role> roleRepo 
            , IGenericRepository<User> userRepo, IGenericRepository<Children> childRepo, 
            IMapper mapper, IHttpContextAccessor httpCtx, IGenericRepository<UserPackage> userPackageRepo) : base(mapper, httpCtx)
        {
            _requestRepo = requestRepo;
            _userRepo = userRepo;
            _childrenRepo = childRepo;
            _roleRepo = roleRepo;
            _userPackageRepo = userPackageRepo;
        }

        public async Task<IActionResult> UserGetMyRequest(ConsultationRequestQuery query, ConsultationRequestStatusEnum? status)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            string searchKeyword = query.SearchKeyword ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;
            var userId = payload.UserId;

            var filter = PredicateBuilder.True<ConsultationRequest>();

            filter = filter.And(r => r.UserRequestId == userId);

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = filter.And(u => u.Title.Contains(searchKeyword) ||
                                         u.Description.Contains(searchKeyword));
            }

            if (status != null)
            {
                filter = filter.And(u => u.Status == status);
            }

            var requests = await _requestRepo.WhereAsync(
                filter,
                orderBy: q => q.OrderByDescending(u => u.CreatedAt),
                page: page,
                pageSize: pageSize
            );

            var totalRequests = await _requestRepo.CountAsync(filter);

            var result = new
            {
                Data = _mapper.Map<IEnumerable<ConsultationRequest>>(requests),
                Total = totalRequests,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> SendRequest(ConsultationRequestCreateDto dto, Guid doctorReceiveId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var doctor = await _userRepo.FindByIdAsync(doctorReceiveId);


            //var children = await _childrenRepo.FindByIdAsync(childId);

            //if (childId ==  Guid.Empty || doctorReceiveId == Guid.Empty
            //    || children == null || doctor == null)
            //{
            //    return ErrorResp.NotFound("Doctor or Children not found");
            //}
            var userId = payload.UserId;
            var existPackage = await _userPackageRepo.WhereAsync(u => u.OwnerId.Equals(userId) && u.ExpireDate >= DateOnly.FromDateTime(DateTime.UtcNow));
            if (existPackage == null)
            {
                return ErrorResp.BadRequest("You need to upgrade your plan to use these access!");
            }

            //if (children.ParentId != userId)
            //{
            //    return ErrorResp.BadRequest("This child is not yours");
            //}

            if (doctor.RoleId != Guid.Parse(GeneralConst.ROLE_DOCTOR_GUID))
            {
                return ErrorResp.BadRequest("The receiver is not available");
            }


            var request = _mapper.Map<ConsultationRequest>(dto);
            request.UserRequestId = userId;
            //request.ChildrentId = childId;
            request.Status = ConsultationRequestStatusEnum.Pending;
            request.DoctorReceiveId = doctorReceiveId;
            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;
            await _requestRepo.CreateAsync(request);


            return SuccessResp.Created($"Request sent to doctor {doctor.Name} successfully");
        }

        public async Task<IActionResult> DoctorGetMyRequest(ConsultationRequestQuery query, ConsultationRequestStatusEnum? status)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            string searchKeyword = query.SearchKeyword ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;
            var userId = payload.UserId;

            var filter = PredicateBuilder.True<ConsultationRequest>();

            filter = filter.And(r => r.DoctorReceiveId == userId);

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = filter.And(u => u.Title.Contains(searchKeyword) ||
                                         u.Description.Contains(searchKeyword));
            }

            if (status != null)
            {
                filter = filter.And(u => u.Status == status);
            }

            var requests = await _requestRepo.WhereAsync(
                filter,
                orderBy: q => q.OrderByDescending(u => u.CreatedAt),
                page: page,
                pageSize: pageSize
            );

            var totalRequests = await _requestRepo.CountAsync(filter);

            var result = new
            {
                Data = _mapper.Map<IEnumerable<ConsultationRequest>>(requests),
                Total = totalRequests,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }
    }
}
