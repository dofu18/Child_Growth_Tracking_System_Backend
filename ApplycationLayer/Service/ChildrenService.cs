using Application.ResponseCode;
using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using DomainLayer.Entities;


namespace ApplicationLayer.Service
{
    public interface IChildrenService
    {
        Task<IActionResult> Create(ChildrenCreateDto dto);
        Task<IActionResult> GetAll();
        Task<IActionResult> Update(Guid id, ChildrenUpdateDto dto);
        Task<IActionResult> GetChildByParent(Guid parentId);

    }
    public class ChildrenService : BaseService, IChildrenService
    {
        private readonly IGenericRepository<Children> _childrenRepo;

        public ChildrenService(IGenericRepository<Children> childrenRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _childrenRepo = childrenRepo;
        }

        public async Task<IActionResult> Create(ChildrenCreateDto dto)
        {
            //var userIdClaim = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim))
            //    return Unauthorized("User not authenticated");

            var childrent = _mapper.Map<Children>(dto);
            childrent.ParentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            childrent.CreatedAt = DateTime.Now;
            childrent.UpdatedAt = DateTime.Now;

            await _childrenRepo.CreateAsync(childrent);
            return SuccessResp.Created("Children information added successfully.");
        }

        public async Task<IActionResult> GetAll()
        {
            //var userIdClaim = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim))
            //    return Unauthorized("User not authenticated");

            //var userId = new Guid(userIdClaim);
            var chidren = await _childrenRepo.ListAsync();
            var result = _mapper.Map<List<ChildrentResponseDto>>(chidren);

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> Update(Guid id, ChildrenUpdateDto dto)
        {
            var children = await _childrenRepo.FindByIdAsync(id);

            if (children == null)
            {
                return ErrorResp.NotFound("Children Not Found");
            }

            _mapper.Map(dto, children);

            await _childrenRepo.UpdateAsync(children);

            return SuccessResp.Ok("Children information updated successfully");
        }

        public async Task<IActionResult> GetChildByParent(Guid parentId)
        {
            //var userIdClaim = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim))
            //    return Unauthorized("User not authenticated");

            var chidren = await _childrenRepo.WhereAsync(c => c.ParentId == parentId);

            if (!chidren.Any())
            {
                return ErrorResp.NotFound("No children found for this parent");
            }
            //var result = _mapper.Map<List<ChildrentResponseDto>>(chidren);

            return SuccessResp.Ok(chidren);
        }
    }
}
