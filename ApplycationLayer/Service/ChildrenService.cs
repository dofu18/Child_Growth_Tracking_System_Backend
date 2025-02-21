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
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using DomainLayer.Enum;


namespace ApplicationLayer.Service
{
    public interface IChildrenService
    {
        Task<IActionResult> Create(ChildrenCreateDto dto);
        Task<IActionResult> GetAll();
        Task<IActionResult> Update(Guid id, ChildrenUpdateDto dto);
        Task<IActionResult> GetChildByParent(Guid parentId);
        Task<IActionResult> Delete(Guid id);
        Task<IActionResult> HideChildren(Guid childId, bool isHidden);
        Task<IActionResult> ShareProfile(ShareProfileCreateDto dto);

    }
    public class ChildrenService : BaseService, IChildrenService
    {
        private readonly IGenericRepository<Children> _childrenRepo;
        private readonly IGenericRepository<SharingProfile> _sharingRepo;
        private readonly ILogger<ChildrenService> _logger; 

        public ChildrenService(IGenericRepository<Children> childrenRepo, IGenericRepository<SharingProfile> sharingRepo, IMapper mapper, IHttpContextAccessor httpCtx, ILogger<ChildrenService> logger) : base(mapper, httpCtx)
        {
            _childrenRepo = childrenRepo;
            _sharingRepo = sharingRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Create(ChildrenCreateDto dto)
        {
            //var userIdClaim = _httpCtx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim))
            //    return Unauthorized("User not authenticated");

            var childrent = _mapper.Map<Children>(dto);
            //childrent.ParentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
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

        public async Task<IActionResult> Delete(Guid id)
        {
            var children = await _childrenRepo.FindByIdAsync(id);

            if (children == null)
            {
                return ErrorResp.NotFound("Children Not Found");
            }

            await _childrenRepo.DeleteAsync(id);

            return SuccessResp.Ok("Children information deleted successfully");
        }

        public async Task<IActionResult> HideChildren(Guid childId, bool isHidden)
        {
            var child = await _childrenRepo.FindByIdAsync(childId);
            if (child == null)
            {
                return ErrorResp.NotFound("Children Not Found");
            }

            var userId = new Guid("11111111-1111-1111-1111-111111111111");
            if (child.ParentId != userId)
            {
                return ErrorResp.Forbidden("You do not have permission to hide this child's information");
            }

            //child.IsHidden = isHidden;
            //thiếu phần này bên children.cs
            // public bool IsHidden { get; set; } = false;

            await _childrenRepo.UpdateAsync(child);

            return SuccessResp.Ok(isHidden ? "Child information is now hidden." : "Child information is now visible.");
        }

        public async Task<IActionResult> ShareProfile(ShareProfileCreateDto dto)
        {
            _logger.LogInformation("Processing child profile sharing request...");

            var child = await _childrenRepo.FindByIdAsync(dto.ChildId);
            if (child == null)
            {
                _logger.LogWarning("Child profile not found, ID: {ChildId}", dto.ChildId);
                return ErrorResp.NotFound("Child profile not found");
            }

            var userId = new Guid("11111111-1111-1111-1111-111111111111");

            var sharingProfile = new SharingProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ChildrentId = dto.ChildId
            };

            await _sharingRepo.CreateAsync(sharingProfile);

            _logger.LogInformation("Child information shared successfully. ID: {ChildId}, Recipient: {Email}", dto.ChildId, dto.RecipientEmail);

            return SuccessResp.Ok("Child's development information has been shared and stored.");
        }

    }
}
