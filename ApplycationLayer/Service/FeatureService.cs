using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.Feature;
using ApplicationLayer.DTOs.RatingFeedback;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IFeatureService
    {
        Task<IActionResult> Create(FeatureCreateDto dto);
        Task<IActionResult> UpdateAsync(Guid featureId, FeatureUpdateDto dto);
        Task<IActionResult> GetAllFeature(FeatureQuery query);
        Task<IActionResult> DeleteAsync(Guid featureId);
    }

    public class FeatureService : BaseService, IFeatureService
    {
        private readonly IGenericRepository<Feature> _repo;

        public FeatureService(IGenericRepository<Feature> repo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Create(FeatureCreateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;


            var feature = _mapper.Map<Feature>(dto);

            feature.CreatedBy = userId;
            feature.CreatedAt = DateTime.UtcNow;
            feature.UpdatedAt = DateTime.UtcNow;
            await _repo.CreateAsync(feature);

            return SuccessResp.Created("Feature created successfully");
        }

        public async Task<IActionResult> DeleteAsync(Guid featureId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var feature = await _repo.FoundOrThrowAsync(featureId);

            if (feature == null)
            {
                return ErrorResp.NotFound("Feature not found");
            }

            await _repo.DeleteAsync(featureId);
            return SuccessResp.Ok("Deleted feature");
        }

        public async Task<IActionResult> GetAllFeature(FeatureQuery query)
        {
            string searchKeyword = query.SearchKeyword ?? "";
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var resp = await _repo.WhereAsync(r => r.FeatureName.Contains(searchKeyword) ||
                                                r.Description.Contains(searchKeyword));

            var features = resp
              .Skip(page * pageSize)
              .Take(pageSize)
              .ToList();

            var feature = _mapper.Map<IEnumerable<FeatureDto>>(features);

            var result = new
            {
                Data = feature,
                Total = resp.Count,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> UpdateAsync(Guid featureId, FeatureUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var feature = await _repo.FindByIdAsync(featureId);

            if (feature == null)
            {
                return ErrorResp.NotFound("Feature not found");
            }

            _mapper.Map(dto, feature);
            feature.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(feature);

            return SuccessResp.Ok("Feature updated successfully");
        }
    }
}
