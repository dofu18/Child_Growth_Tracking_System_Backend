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

namespace ApplicationLayer.Service
{
    public interface IRoleService
    {
        Task<IActionResult> HandleGetByIdAsync(Guid roleId);
    }

    public class RoleService : BaseService, IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepo;

        public RoleService(IGenericRepository<Role> roleRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _roleRepo = roleRepo;
        }

        public async Task<IActionResult> HandleGetByIdAsync(Guid roleId)
        {
           var role = await _roleRepo.FoundOrThrowAsync(roleId);

            return SuccessResp.Ok(role);
        }
    }
}
