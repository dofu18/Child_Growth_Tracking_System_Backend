using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.User;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLayer.Service
{
    public interface IUserService
    {
        Task<IActionResult> HandleGetByIdAsync();
        Task<IActionResult> HandleUpdateAsync(UserUpdateDto dto);

    }
    public class UserService : BaseService, IUserService
    {
        private readonly IGenericRepository<User> _userRepo;

        public UserService(IGenericRepository<User> userRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _userRepo = userRepo;
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
