using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.User;
using ApplicationLayer.Shared;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Ocsp;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IUserService
    {
        Task<IActionResult> HandleGetByIdAsync();
        Task<IActionResult> HandleUpdateAsync(UserUpdateDto dto);
        //admin
        Task<IActionResult> GetAllUserAsync(UserQuery query, UserStatusEnum? status);
        Task<IActionResult> HandleStatusAsync(Guid id, UserStatusEnum status);
        Task<IActionResult> HandleRoleAsync(Guid id, Guid roleId);
    }
    public class UserService : BaseService, IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;

        public UserService(IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        public async Task<IActionResult> GetAllUserAsync(UserQuery query, UserStatusEnum? status)
        {
            //string searchKeyword = query.SearchKeyword ?? "";
            //List<Guid> roleIds = query.RoleIds ?? new List<Guid>();
            //int page = query.Page < 0 ? 0 : query.Page;
            //int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            //var payload = ExtractPayload();
            //if (payload == null)
            //{
            //    return ErrorResp.Unauthorized("Invalid token");
            //}

            //var resp = new List<User>();

            //if (status == null)
            //{
            //    resp = await _userRepo.WhereAsync(r => r.Name.Contains(searchKeyword) || r.Email.Contains(searchKeyword) || r.UserName.Contains(searchKeyword));
            //}
            //else
            //{
            //    resp = await _userRepo.WhereAsync(r => r.Name.Contains(searchKeyword) || r.Email.Contains(searchKeyword) || r.UserName.Contains(searchKeyword) && r.Status == status);
            //}

            //var users = resp
            //  .Skip(page * pageSize)
            //  .Take(pageSize)
            //  .ToList();

            //var user = _mapper.Map<IEnumerable<User>>(users);

            //var result = new
            //{
            //    Data = user,
            //    Total = resp.Count,
            //    Page = query.Page,
            //    PageSize = query.PageSize
            //};

            //return SuccessResp.Ok(result);
            string searchKeyword = query.SearchKeyword ?? "";
            List<Guid> roleIds = query.RoleIds ?? new List<Guid>();
            int page = query.Page < 0 ? 0 : query.Page;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var filter = PredicateBuilder.True<User>();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter = filter.And(u => u.Name.Contains(searchKeyword) ||
                                         u.Email.Contains(searchKeyword) ||
                                         u.UserName.Contains(searchKeyword));
            }

            if (status != null)
            {
                filter = filter.And(u => u.Status == status);
            }

            if (roleIds.Any())
            {
                filter = filter.And(u => roleIds.Contains(u.RoleId));
            }

            var users = await _userRepo.WhereAsync(
                filter,
                orderBy: q => q.OrderByDescending(u => u.CreatedAt),
                page: page,
                pageSize: pageSize,
                "Role"
            );

            var totalUsers = await _userRepo.CountAsync(filter);

            var result = new
            {
                Data = _mapper.Map<IEnumerable<User>>(users),
                Total = totalUsers,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> HandleGetByIdAsync()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            var user = await _userRepo.FoundOrThrowAsync(userId, "Role");

            return SuccessResp.Ok(user);
        }

        public async Task<IActionResult> HandleRoleAsync(Guid id, Guid roleId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var user = await _userRepo.FindByIdAsync(id);
            var role = await _roleRepo.FindByIdAsync(roleId);

            if (user == null)
            {
                return ErrorResp.NotFound("User not found");
            }

            if (role == null)
            {
                return ErrorResp.NotFound("Role not found");
            }

            if (user.RoleId == roleId)
            {
                return ErrorResp.BadRequest($"This User is already in role {role.RoleName}");
            }

            user.RoleId = roleId;
            await _userRepo.UpdateAsync(user);

            return SuccessResp.Ok($"Updated user with role {role.RoleName}");
        }

        public async Task<IActionResult> HandleStatusAsync(Guid id, UserStatusEnum status)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var user = await _userRepo.FindByIdAsync(id);

            if (user == null)
            {
                return ErrorResp.NotFound("User not found");
            }

            user.Status = status;

            await _userRepo.UpdateAsync(user);

            return SuccessResp.Ok($"Updated user with status {status}");
        }

        public async Task<IActionResult> HandleUpdateAsync(UserUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            var user = await _userRepo.FoundOrThrowAsync(userId, "Role");

            _mapper.Map(dto, user);

            await _userRepo.UpdateAsync(user);

            return SuccessResp.Ok("User updated successfully");
        }
    }
}
