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

            // Chuyển đổi DateOnly thành DateTime
            DateTime dob = request.DoB.ToDateTime(TimeOnly.MinValue);
            DateTime today = DateTime.UtcNow;

            // Kiểm tra nếu DoB nhập vào nhỏ hơn ngày sinh của trẻ
            if (dob < child.DoB.ToDateTime(TimeOnly.MinValue))
            {
                throw new Exception("Invalid Date of Birth: The entered DoB cannot be earlier than the child's actual birth date.");
            }

            // Tính số tháng tuổi
            int ageInMonths = (today.Year - dob.Year) * 12 + (today.Month - dob.Month);
            if (today.Day < dob.Day) ageInMonths--; // Giảm 1 tháng nếu chưa đến ngày sinh

            // Kiểm tra nếu tuổi nhỏ hơn 0 hoặc lớn hơn 216 tháng (19 tuổi)
            if (ageInMonths < 0 || ageInMonths > 216)
            {
                throw new Exception("Invalid Age: BMI data is only available for ages between 0 and 19 years (0-216 months).");
            }

            // Kiểm tra chiều cao và cân nặng có hợp lệ không
            if (request.Height <= 0 || request.Weight <= 0)
            {
                throw new Exception("Invalid input: Height and weight must be greater than zero.");
            }

            // Kiểm tra chiều cao có thực tế không
            if (request.Height < 30 || request.Height > 250)
            {
                throw new Exception("Invalid height: Height must be between 30cm and 250cm.");
            }

            // Kiểm tra cân nặng có thực tế không
            if (request.Weight < 1 || request.Weight > 300)
            {
                throw new Exception("Invalid weight: Weight must be between 1kg and 300kg.");
            }

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
