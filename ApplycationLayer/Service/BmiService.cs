using ApplicationLayer.DTOs.BMI;
using ApplicationLayer.DTOs.GrowthRecord;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;
using ApplicationLayer.DTOs.BMI;

namespace ApplicationLayer.Service
{
    public interface IBmiService
    {
        Task<CalculateBmiResponseDto> CalculateBmiAsync(CalculateBmiRequestDto request);
        Task<GrowthRecordResponseDto> SaveGrowthRecordAsync(SaveGrowthRecordRequestDto request);
    }

    public class BmiService : BaseService, IBmiService
    {
        private readonly IGenericRepository<Children> _childrenRepo;
        private readonly IGenericRepository<BmiCategory> _bmiCategoryRepo;
        private readonly IGenericRepository<GrowthRecord> _growthRecordRepo;

        public BmiService(IGenericRepository<Children> childrenRepo, IGenericRepository<BmiCategory> bmiCategoryRepo, IGenericRepository<GrowthRecord> growthRecordRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _childrenRepo = childrenRepo;
            _bmiCategoryRepo = bmiCategoryRepo;
            _growthRecordRepo = growthRecordRepo;
        }

        public async Task<CalculateBmiResponseDto> CalculateBmiAsync(CalculateBmiRequestDto request)
        {
            if (request.Height <= 0)
                throw new ArgumentException("Height must be greater than zero.");

            decimal bmi = request.Weight / ((request.Height / 100) * (request.Height / 100));

            if (request.AgeInMonths <= 0)
                throw new ArgumentException("AgeInMonths must be greater than zero.");

            decimal bmiPercentile = CalculateBmiPercentile(bmi, request.AgeInMonths, request.Gender);
            string category = await GetBmiCategoryAsync(bmi);

            return new CalculateBmiResponseDto
            {
                Bmi = bmi,
                BmiPercentile = bmiPercentile,
                BmiCategory = category
            };
        }

        // Hàm hỗ trợ tính percentile
        private decimal CalculateBmiPercentile(decimal bmi, int ageInMonths, GenderEnum gender)
        {
            // Lấy dữ liệu WHO theo giới tính
            var bmiTable = gender == GenderEnum.Male ? DataWHO.WHO_BMI_TABLE_MALE : DataWHO.WHO_BMI_TABLE_FEMALE;


            // Tìm tuổi gần nhất
            if (!bmiTable.ContainsKey(ageInMonths))
                return 50; // Nếu không tìm thấy dữ liệu, trả về trung bình

            var percentiles = bmiTable[ageInMonths];

            // So sánh BMI để tìm percentile phù hợp
            if (bmi < percentiles[5]) return 5;
            if (bmi < percentiles[10]) return 10;
            if (bmi < percentiles[25]) return 25;
            if (bmi < percentiles[50]) return 50;
            if (bmi < percentiles[75]) return 75;
            if (bmi < percentiles[85]) return 85;
            if (bmi < percentiles[95]) return 95;
            return 99;
        }

        // Lấy danh mục BMI
        private async Task<string> GetBmiCategoryAsync(decimal bmi)
        {
            var category = await _bmiCategoryRepo.FirstOrDefaultAsync(c => c.BmiBottom <= bmi && c.BmiTop >= bmi);
            return category?.Name ?? "Unknown";
        }

        // Lưu vào GrowthRecord và cập nhật Children
        public async Task<GrowthRecordResponseDto> SaveGrowthRecordAsync(SaveGrowthRecordRequestDto request)
        {
            var payload = ExtractPayload();
            if (payload == null) throw new UnauthorizedAccessException("Invalid token");

            var child = await _childrenRepo.FoundOrThrowAsync(request.ChildId, "Child not found");

            decimal bmi = request.Weight / (request.Height / 100 * request.Height / 100);
            decimal bmiPercentile = CalculateBmiPercentile(bmi, request.AgeInMonths, request.Gender);
            string category = await GetBmiCategoryAsync(bmi);

            var bmiCategory = await _bmiCategoryRepo.FirstOrDefaultAsync(c => c.BmiBottom <= bmi && c.BmiTop >= bmi);
            if (bmiCategory == null) throw new Exception("BMI Category not found");

            // Lưu vào bảng GrowthRecord
            var record = new GrowthRecord
            {
                ChildrentId = request.ChildId,
                Height = request.Height,
                Weight = request.Weight,
                Bmi = bmi,
                BmiPercentile = bmiPercentile,
                BmiCategory = bmiCategory.Id,
                Notes = request.Notes,
                CreatedBy = payload.UserId
            };

            await _growthRecordRepo.CreateAsync(record);

            // Cập nhật Children
            child.Height = request.Height;
            child.Weight = request.Weight;
            child.Bmi = bmi;
            child.BmiPercentile = bmiPercentile;
            child.BmiCategoryId = bmiCategory.Id;
            child.UpdatedAt = DateTime.UtcNow;

            await _childrenRepo.UpdateAsync(child);

            return new GrowthRecordResponseDto
            {
                RecordId = record.Id,
                Height = record.Height,
                Weight = record.Weight,
                Bmi = record.Bmi,
                BmiPercentile = record.BmiPercentile,
                BmiCategory = category,
                Notes = record.Notes
            };
        }


    }
}
