using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.BmiCategory;
using AutoMapper;
using DomainLayer.Constants;
using DomainLayer.Entities;
using DomainLayer.Enum;
using DomainLayer.Exceptions;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLayer.Service
{
    public interface IBmiCategoryService
    {
        Task<IActionResult> Create(BmiCategoryCreateDto dto);
    }
    public class BmiCategoryService : BaseService, IBmiCategoryService
    {
        private readonly IGenericRepository<BmiCategory> _bmiCategoryRepo;

        public BmiCategoryService(IGenericRepository<BmiCategory> bmiCategoryRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _bmiCategoryRepo = bmiCategoryRepo;
        }

        public async Task<IActionResult> Create(BmiCategoryCreateDto dto)
        {
            var bmiCategory = _mapper.Map<BmiCategory>(dto);
            bmiCategory.CreatedBy = new Guid("11111111-1111-1111-1111-111111111111");
            bmiCategory.CreatedAt = DateTime.Now;
            bmiCategory.UpdatedAt = DateTime.Now;
            await _bmiCategoryRepo.CreateAsync(bmiCategory);

            return SuccessResp.Created("Bmi category created successfully");
        }
    }
}
