using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.BmiCategory;
using ApplicationLayer.DTOs.RatingFeedback;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IRatingFeedbackService
    {
        Task<IActionResult> Create(RatingFeedbackCreateDto dto);
        Task<IActionResult> GetRatingPublish(RatingFeedbackQuery query);
        Task<IActionResult> HandleUpdateAsync(Guid id, RatingFeedbackUpdateDto dto);

        //admin
        Task<IActionResult> HandleStatusAsync(Guid id, RatingFeedbackStatusEnum status);

        Task<IActionResult> AdminGetAllRating(RatingFeedbackQuery query, RatingFeedbackStatusEnum? status);

    }

    public class RatingFeedbackService : BaseService, IRatingFeedbackService
    {
        private readonly IGenericRepository<RatingFeedback> _ratingFeedbackRepo;

        public RatingFeedbackService(IGenericRepository<RatingFeedback> ratingFeedbackRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _ratingFeedbackRepo = ratingFeedbackRepo;
        }

        public async Task<IActionResult> Create(RatingFeedbackCreateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;


            var ratingFeedback = _mapper.Map<RatingFeedback>(dto);
            if (dto.Feedback == null)
            {
                ratingFeedback.Feedback = "";
            }
            ratingFeedback.UserId = userId;
            ratingFeedback.CreatedAt = DateTime.Now;
            ratingFeedback.UpdatedAt = DateTime.Now;
            await _ratingFeedbackRepo.CreateAsync(ratingFeedback);

            return SuccessResp.Created("Rating feedback created successfully");
        }

        public async Task<IActionResult> AdminGetAllRating(RatingFeedbackQuery query, RatingFeedbackStatusEnum? status)
        {
            string searchKeyword = query.SearchKeyword ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var resp = new List<RatingFeedback>();

            if (status == null)
            {
                resp = await _ratingFeedbackRepo.WhereAsync(r => r.Feedback.Contains(searchKeyword));
            }
            else
            {
                resp = await _ratingFeedbackRepo.WhereAsync(r => r.Feedback.Contains(searchKeyword) && r.Status == status);
            }

            var ratingFeedback = resp
              .Skip(page * pageSize)
              .Take(pageSize)
              .ToList();

            var feedback = _mapper.Map<IEnumerable<RatingFeedbackDto>>(ratingFeedback);

            var result = new
            {
                Data = feedback,
                Total = resp.Count,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> GetRatingPublish(RatingFeedbackQuery query)
        {
            string searchKeyword = query.SearchKeyword ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var resp = await _ratingFeedbackRepo.WhereAsync(r => r.Feedback.Contains(searchKeyword) && r.Status == RatingFeedbackStatusEnum.Publish);


            var ratingFeedback = resp
              .Skip(page * pageSize)
              .Take(pageSize)
              .ToList();

            var feedback = _mapper.Map<IEnumerable<RatingFeedbackDto>>(ratingFeedback);

            var result = new
            {
                Data = feedback,
                Total = resp.Count,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> HandleUpdateAsync(Guid id, RatingFeedbackUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var ratingFeedback = await _ratingFeedbackRepo.FindByIdAsync(id);

            if (!payload.UserId.Equals(ratingFeedback.UserId))
            {
                return ErrorResp.BadRequest("Just the owner can update this feedback");
            }

            if (ratingFeedback == null)
            {
                return ErrorResp.NotFound("Feedback not found");
            }

            _mapper.Map(dto, ratingFeedback);

            await _ratingFeedbackRepo.UpdateAsync(ratingFeedback);

            return SuccessResp.Ok("Feedback updated successfully");
        }

        public async Task<IActionResult> HandleStatusAsync(Guid id, RatingFeedbackStatusEnum status)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var ratingFeedback = await _ratingFeedbackRepo.FindByIdAsync(id);

            if (ratingFeedback == null)
            {
                return ErrorResp.NotFound("Feedback not found");
            }

            ratingFeedback.Status = status;

            await _ratingFeedbackRepo.UpdateAsync(ratingFeedback);

            return SuccessResp.Ok($"Updated feedback with status {status}");
        }
    }
}
