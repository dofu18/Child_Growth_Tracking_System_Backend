using Application.ResponseCode;
using ApplicationLayer.DTOs.WHOData;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IWHODataService
    {
        Task<IActionResult> CreateData(WhoDataCreateDto dto);
        Task<IActionResult> UpdateData(WhoDataUpdateDto dto);
        Task<IActionResult> DeleteData(Guid id);
        Task<IActionResult> GetAllData();
        Task<IActionResult> GetDataById(Guid id);
        Task<IActionResult> GetDataByGender(GenderEnum gender);
    }

    public class WHODataService : BaseService, IWHODataService
    {
        private readonly IGenericRepository<WhoData> _whoDataRepo;

        public WHODataService(IGenericRepository<WhoData> whoData, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _whoDataRepo = whoData;
        }

        public async Task<IActionResult> CreateData(WhoDataCreateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            // Kiểm tra nếu đã tồn tại bản ghi với AgeMonth & Gender
            var existingData = await _whoDataRepo.FirstOrDefaultAsync(x => 
                    x.AgeMonth == dto.AgeMonth && 
                    x.Gender == dto.Gender && 
                    x.BmiPercentile == dto.BmiPercentile);

            if (existingData != null)
            {
                return ErrorResp.BadRequest("Data already exists for the given AgeMonth and Gender.");
            }

            var whoData = _mapper.Map<WhoData>(dto);

            await _whoDataRepo.CreateAsync(whoData);

            return SuccessResp.Ok("WHO Data created successfully");
        }

        public async Task<IActionResult> UpdateData(WhoDataUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            // Kiểm tra dữ liệu có tồn tại không
            var existingData = await _whoDataRepo.FoundOrThrowAsync(dto.Id);
            if (existingData == null)
            {
                return ErrorResp.BadRequest("WHO Data not found");
            }

            // Cập nhật dữ liệu từ DTO
            _mapper.Map(dto, existingData);

            await _whoDataRepo.UpdateAsync(existingData);

            return SuccessResp.Ok("WHO Data updated successfully");
        }

        public async Task<IActionResult> DeleteData(Guid id)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            // Kiểm tra nếu dữ liệu không tồn tại
            var existingData = await _whoDataRepo.FoundOrThrowAsync(id);
            if (existingData == null)
            {
                return ErrorResp.BadRequest("WHO Data not found");
            }

            await _whoDataRepo.DeleteAsync(id);

            return SuccessResp.Ok("WHO Data deleted successfully");
        }

        public async Task<IActionResult> GetAllData()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            // Lấy danh sách dữ liệu WHO từ cơ sở dữ liệu
            var dataList = await _whoDataRepo.ListAsync();
            if (dataList == null || !dataList.Any())
            {
                return ErrorResp.BadRequest("WHO Data not found");
            }

            // Sắp xếp dữ liệu theo AgeMonth, BMI và BmiPercentile từ bé đến lớn
            var sortedData = dataList
                             .OrderBy(x => x.AgeMonth)      // Sắp xếp theo AgeMonth từ bé đến lớn
                             .ThenBy(x => x.Bmi)            // Sắp xếp theo BMI từ bé đến lớn nếu AgeMonth giống nhau
                             .ThenBy(x => x.BmiPercentile)  // Sắp xếp theo BmiPercentile từ bé đến lớn nếu AgeMonth và BMI giống nhau
                             .ToList();

            var dtoList = _mapper.Map<List<WhoDataDto>>(sortedData);

            return SuccessResp.Ok(dtoList);
        }

        public async Task<IActionResult> GetDataById(Guid id)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var data = await _whoDataRepo.FoundOrThrowAsync(id);
            if (data == null)
            {
                return ErrorResp.BadRequest("WHO Data not found");
            }

            var dto = _mapper.Map<WhoDataDto>(data);

            return SuccessResp.Ok(dto);
        }

        public async Task<IActionResult> GetDataByGender(GenderEnum gender)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            // Lấy danh sách dữ liệu WHO theo giới tính
            var dataList = await _whoDataRepo.WhereAsync(x => x.Gender == gender);
            if (dataList == null || !dataList.Any())
            {
                return ErrorResp.BadRequest("WHO Data not found");
            }

            // Sắp xếp dữ liệu theo AgeMonth, BMI và BmiPercentile từ bé đến lớn
            var sortedData = dataList
                             .OrderBy(x => x.AgeMonth)      // Sắp xếp theo AgeMonth từ bé đến lớn
                             .ThenBy(x => x.Bmi)            // Sắp xếp theo BMI từ bé đến lớn nếu AgeMonth giống nhau
                             .ThenBy(x => x.BmiPercentile)  // Sắp xếp theo BmiPercentile từ bé đến lớn nếu AgeMonth và BMI giống nhau
                             .ToList();

            var dtoList = _mapper.Map<List<WhoDataDto>>(sortedData);

            return SuccessResp.Ok(dtoList);
        }
    }
}
