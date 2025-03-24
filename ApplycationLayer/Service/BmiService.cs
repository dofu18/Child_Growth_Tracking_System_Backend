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
using Org.BouncyCastle.Asn1.Ocsp;

namespace ApplicationLayer.Service
{
    public interface IBmiService
    {
        Task<GrowthRecordResponseDto> SaveGrowthRecordAsync(SaveGrowthRecordRequestDto request);
    }

    public class BmiService : BaseService, IBmiService
    {
        private readonly IGenericRepository<Children> _childrenRepo;
        private readonly IGenericRepository<BmiCategory> _bmiCategoryRepo;
        private readonly IGenericRepository<GrowthRecord> _growthRecordRepo;
        private readonly IGenericRepository<WhoData> _whoDataRepo;
        private readonly IGenericRepository<User> _userRepo;

        public BmiService(IGenericRepository<Children> childrenRepo, IGenericRepository<BmiCategory> bmiCategoryRepo, IGenericRepository<GrowthRecord> growthRecordRepo, IGenericRepository<WhoData> whoDataRepo, IGenericRepository<User> userRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _childrenRepo = childrenRepo;
            _bmiCategoryRepo = bmiCategoryRepo;
            _growthRecordRepo = growthRecordRepo;
            _whoDataRepo = whoDataRepo;
            _userRepo = userRepo;
        }

        public async Task<GrowthRecordResponseDto> SaveGrowthRecordAsync(SaveGrowthRecordRequestDto request)
        {
            var payload = ExtractPayload();
            if (payload == null) throw new UnauthorizedAccessException("Invalid token");

            // Lấy thông tin user từ database
            var user = await _userRepo.FindByIdAsync(payload.UserId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            // Kiểm tra trạng thái của user
            if (user.Status == UserStatusEnum.Disable || user.Status == UserStatusEnum.Archived || user.Status == UserStatusEnum.NotVerified)
            {
                throw new UnauthorizedAccessException("Your account status does not allow updating growth records.");
            }

            //Lấy thông tin trẻ em
            var child = await _childrenRepo.FoundOrThrowAsync(request.ChildId, "Child not found");

            // Tính số tháng tuổi từ DoB của request
            DateTime dob = request.DoB.ToDateTime(TimeOnly.MinValue);
            DateTime today = DateTime.UtcNow;

            int ageInMonths = (today.Year - dob.Year) * 12 + (today.Month - dob.Month);
            if (today.Day < dob.Day) ageInMonths--; // Giảm 1 tháng nếu chưa đến ngày sinh

            if (ageInMonths < 0) throw new Exception("Invalid Date of Birth");

            //Tính BMI
            decimal bmi = Math.Round(request.Weight / ((request.Height / 100) * (request.Height / 100)), 2);

            // Lấy BMI Percentile từ bảng WhoData
            var whoDataList = await _whoDataRepo.WhereAsync(w => w.AgeMonth == ageInMonths && w.Gender == request.Gender);
            var whoData = whoDataList.OrderBy(w => Math.Abs(w.Bmi - bmi)).FirstOrDefault();

            if (whoData == null) throw new Exception("BMI Percentile data not found");

            decimal bmiPercentile = whoData.BmiPercentile;

            //Xác định BMI Category
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
                BmiCategory = bmiCategory.Name,
                Notes = record.Notes
            };
        }


    }
}
