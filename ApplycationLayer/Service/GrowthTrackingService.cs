using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.BmiCategory;
using ApplicationLayer.DTOs.GrowthRecord;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DomainLayer.Enum.GeneralEnum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApplicationLayer.Service
{
    public interface IGrowthTrackingService
    {
        Task<IActionResult> GetGrowthTracking(Guid childId, DateTime? startDate, DateTime? endDate);
        Task<IActionResult> DeleteGrowthRecord(Guid growthRecordId);
        Task<IActionResult> GetBmiCategoryName(Guid bmiCategoryId);
        Task<IActionResult> GetGrowthTrackingHistory(GrowthTrackingQuery query, Guid childId, DateTime? startDate, DateTime? endDate);
    }

    public class GrowthTrackingService : BaseService, IGrowthTrackingService
    {
        private readonly IGenericRepository<GrowthRecord> _growthRepo;
        private readonly IGenericRepository<Children> _childRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<BmiCategory> _bmiCategoryRepo;

        public GrowthTrackingService(IGenericRepository<GrowthRecord> growthRepo, IGenericRepository<Children> childRepo, 
            IGenericRepository<User> userRepo, IGenericRepository<BmiCategory> bmiCategoryRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _growthRepo = growthRepo;
            _childRepo = childRepo;
            _userRepo = userRepo;
            _bmiCategoryRepo = bmiCategoryRepo;
        }

        public async Task<IActionResult> GetGrowthTracking(Guid childId, DateTime? startDate, DateTime? endDate)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var myChild = await _childRepo.WhereAsync(c => c.ParentId == userId && c.Id == childId);

            if (!myChild.Any())
            {
                return ErrorResp.BadRequest("You don't have this children in system!");
            }

            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                return ErrorResp.BadRequest("Start date cannot be greater than end date");
            }


            //var result = await _growthRepo
            //                .WhereAsync(g => g.ChildrentId == childId &&
            //                (!startDate.HasValue || g.CreatedAt >= startDate.Value) &&
            //                (!endDate.HasValue || g.CreatedAt <= endDate.Value));

            var rawGrowthRecords = await _growthRepo.WhereAsync(g =>
                    g.ChildrentId == childId &&
                    (!startDate.HasValue || g.CreatedAt >= startDate.Value) &&
                    (!endDate.HasValue || g.CreatedAt <= endDate.Value));

            var groupedByDate = rawGrowthRecords
                .GroupBy(g => g.CreatedAt.GetValueOrDefault().Date)
                .Select(grp => grp.OrderByDescending(g => g.CreatedAt).First())
                .OrderBy(g => g.CreatedAt)
                .ToList();

            return SuccessResp.Ok(groupedByDate);
        }

        public async Task<IActionResult> DeleteGrowthRecord(Guid growthRecordId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var growthRecord = await _growthRepo.FindAsync(g =>
                    g.Id == growthRecordId,
                    nameof(GrowthRecord.Children)
            );

            if (growthRecord == null)
            {
                return ErrorResp.NotFound("Growth record not found");
            }

            var child = growthRecord.Children;
            if (child == null)
            {
                return ErrorResp.BadRequest("Child information not found");
            }

            if (child.ParentId != userId)
            {
                return ErrorResp.Forbidden("You do not have permission to delete this record");
            }

            await _growthRepo.DeleteAsync(growthRecord);

            return SuccessResp.Ok("Growth record deleted successfully.");
        }

        public async Task<IActionResult> GetBmiCategoryName(Guid bmiCategoryId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var category = await _bmiCategoryRepo.FindByIdAsync(bmiCategoryId);
            if (category == null)
            {
                return ErrorResp.NotFound("BMI Category not found");
            }

            var dto = _mapper.Map<BmiCategoryDto>(category);

            return SuccessResp.Ok(dto);
        }
        public async Task<IActionResult> GetGrowthTrackingHistory(GrowthTrackingQuery query, Guid childId, DateTime? startDate, DateTime? endDate)
        {
            string searchKeyword = query.SearchKeyword ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            var myChild = await _childRepo.WhereAsync(c => c.ParentId == userId && c.Id == childId);

            if (!myChild.Any())
            {
                return ErrorResp.BadRequest("You don't have this children in system!");
            }

            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                return ErrorResp.BadRequest("Start date cannot be greater than end date");
            }


            var resp = await _growthRepo
                            .WhereAsync(g => g.ChildrentId == childId &&
                            (!startDate.HasValue || g.CreatedAt >= startDate.Value) &&
                            (!endDate.HasValue || g.CreatedAt <= endDate.Value));

            var tracking = resp
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new
            {
                Data = tracking,
                Total = resp.Count,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }
    }
}
