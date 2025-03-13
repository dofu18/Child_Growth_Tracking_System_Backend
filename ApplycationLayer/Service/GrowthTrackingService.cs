using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IGrowthTrackingService
    {
        Task<IActionResult> GetGrowthTracking(Guid childId);

    }

    public class GrowthTrackingService : BaseService, IGrowthTrackingService
    {
        private readonly IGenericRepository<GrowthRecord> _growthRepo;
        private readonly IGenericRepository<Children> _childRepo;
        private readonly IGenericRepository<User> _userRepo;

        public GrowthTrackingService(IGenericRepository<GrowthRecord> growthRepo, IGenericRepository<Children> childRepo, 
            IGenericRepository<User> userRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _growthRepo = growthRepo;
            _childRepo = childRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> GetGrowthTracking(Guid childId)
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

            var result = await _growthRepo.WhereAsync(g => g.ChildrentId == childId);

            return SuccessResp.Ok(result);
        }
    }
}
