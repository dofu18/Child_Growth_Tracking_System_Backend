using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.Consultation.ConsultationResponses;
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
    public interface IConsultationResponseService
    {
        Task<IActionResult> SendResponse(ConsultationResponseCreateDto dto, Guid requestId);
        Task<IActionResult> GetByRequestId(ConsultationResponseQuery query, Guid requestId);
        Task<IActionResult> DoctorGetMyResponse(ConsultationResponseQuery query);
    }

    public class ConsultationResponseService : BaseService, IConsultationResponseService
    {
        private readonly IGenericRepository<ConsultationResponse> _responseRepo;
        private readonly IGenericRepository<ConsultationRequest> _requestRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Children> _childrenRepo;
        private readonly IGenericRepository<Role> _roleRepo;

        public ConsultationResponseService(IGenericRepository<ConsultationRequest> requestRepo, IGenericRepository<Role> roleRepo
            , IGenericRepository<User> userRepo, IGenericRepository<Children> childRepo, 
            IMapper mapper, IHttpContextAccessor httpCtx, IGenericRepository<ConsultationResponse> responseRepo) : base(mapper, httpCtx)
        {
            _requestRepo = requestRepo;
            _userRepo = userRepo;
            _childrenRepo = childRepo;
            _roleRepo = roleRepo;
            _responseRepo = responseRepo;
        }

        public async Task<IActionResult> DoctorGetMyResponse(ConsultationResponseQuery query)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            string searchKeyword = query.SearchKeywords ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;
            var userId = payload.UserId;

            var filter = PredicateBuilder.True<ConsultationResponse>();

            filter = filter.And(r => r.DoctorId == userId);

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = filter.And(u => u.Title.Contains(searchKeyword) ||
                                         u.Content.Contains(searchKeyword));
            }

            var responses = await _responseRepo.WhereAsync(
                filter,
                orderBy: q => q.OrderByDescending(u => u.CreatedAt),
                page: page,
                pageSize: pageSize,
                "ConsultationRequest"
            );

            var totalResponse = await _responseRepo.CountAsync(filter);

            var result = new
            {
                Data = _mapper.Map<IEnumerable<ConsultationResponse>>(responses),
                Total = totalResponse,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> GetByRequestId(ConsultationResponseQuery query, Guid requestId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            string searchKeyword = query.SearchKeywords ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var userId = payload.UserId;

            var user = await _userRepo.FindByIdAsync(userId);
            //if (user.RoleId != Guid.Parse(GeneralConst.ROLE_DOCTOR_GUID))
            //{
            //    return ErrorResp.Unauthorized("Just doctor can response consultation");
            //}

            var request = await _requestRepo.FoundOrThrowAsync(requestId);

            if (requestId == Guid.Empty || request == null)
            {
                ErrorResp.NotFound("Request not found or invalid request");
            }

            var filter = PredicateBuilder.True<ConsultationResponse>();

            filter = filter.And(r => r.RequestId == requestId);

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = filter.And(u => u.Title.Contains(searchKeyword) ||
                                         u.Content.Contains(searchKeyword));
            }

            var responses = await _responseRepo.WhereAsync(
                filter,
                orderBy: q => q.OrderByDescending(u => u.CreatedAt),
                page: page,
                pageSize: pageSize,
                "ConsultationRequest"
            );

            var totalResponses = await _responseRepo.CountAsync(filter);

            var result = new
            {
                Data = _mapper.Map<IEnumerable<ConsultationResponse>>(responses),
                Total = totalResponses,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> SendResponse(ConsultationResponseCreateDto dto, Guid requestId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var request = await _requestRepo.FoundOrThrowAsync(requestId);

            if (requestId == Guid.Empty || request == null)
            {
                ErrorResp.NotFound("Request not found or invalid request");
            }

            var userId = payload.UserId;
            var user = await _userRepo.FindByIdAsync(userId);
            if (user.RoleId != Guid.Parse(GeneralConst.ROLE_DOCTOR_GUID))
            {
                return ErrorResp.Unauthorized("Just doctor can response consultation");
            } 
            else if (user.Id != request.DoctorReceiveId)
            {
                return ErrorResp.BadRequest("You can't response this request");
            }

            var response = _mapper.Map<ConsultationResponse>(dto);
            response.DoctorId = userId;
            response.RequestId = requestId;
            response.ResponseDate = DateTime.UtcNow;
            response.CreatedAt = DateTime.UtcNow;
            response.UpdatedAt = DateTime.UtcNow;
            await _responseRepo.CreateAsync(response);

            request.Status = ConsultationRequestStatusEnum.Approve;
            await _requestRepo.UpdateAsync(request);


            return SuccessResp.Created($"Response for request {request.Title} successfully");
        }


    }
}
