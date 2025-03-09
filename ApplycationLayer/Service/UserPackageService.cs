using Application.ResponseCode;
using ApplicationLayer.DTOs.Package;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enum;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IUserPackageService
    {
        Task<IActionResult> CreatePackage(PackageCreateDto dto);
        Task<IActionResult> RenewPackage(Guid packageId);
        Task<IActionResult> CancelMembership();
        Task<IActionResult> UpdatePackage(Guid packageId, PackageUpdateDto dto);
        Task<IActionResult> DeletePackage(Guid packageId);
    }
    public class UserPackageService : BaseService, IUserPackageService
    {
        private readonly IGenericRepository<UserPackage> _userPackageRepo;
        private readonly IGenericRepository<Package> _packageRepo;
        private readonly IGenericRepository<Transaction> _transactionRepo;
        private readonly IVNPAYService _vnPayService;
        private readonly IConfiguration _configuration;

        public UserPackageService(IGenericRepository<UserPackage> userPackageRepo, IGenericRepository<Package> packageRepo, IGenericRepository<Transaction> transactionRepo, IVNPAYService vnPayService, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _userPackageRepo = userPackageRepo;
            _packageRepo = packageRepo;
            _transactionRepo = transactionRepo;
            _vnPayService = vnPayService;
            _configuration = configuration;
        }

        public async Task<IActionResult> CreatePackage(PackageCreateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            try
            {
                var package = new Package
                {
                    Id = Guid.NewGuid(),
                    PackageName = dto.PackageName,
                    Description = dto.Description,
                    Price = dto.Price,
                    DurationMonths = dto.DurationMonths,
                    TrialPeriodDays = dto.TrialPeriodDays,
                    MaxChildrentAllowed = dto.MaxChildrentAllowed,
                    CreatedBy = userId,
                    Status = PackageStatusEnum.Published,
                    CreatedAt = DateTime.UtcNow
                };

                await _packageRepo.CreateAsync(package);

                return SuccessResp.Created("Package created successfully");
            }
            catch (DbUpdateException ex)
            {
                return ErrorResp.InternalServerError($"DbUpdateException: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError($"Exception: {ex.Message}");
            }
        }



        public async Task<IActionResult> RenewPackage(Guid packageId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            try
            {
                var userId = payload.UserId;

                //Kiểm tra package có tồn tại không
                var package = await _packageRepo.FindByIdAsync(packageId);
                if (package == null)
                {
                    return ErrorResp.NotFound("Children not found");
                }

                //Kiểm tra user đã có gói đang hoạt động chưa
                var currentPackage = await _userPackageRepo.FindByIdAsync(userId);
                if (currentPackage == null)
                {
                    return ErrorResp.BadRequest("No active package found for renewal.");
                }

                //Cập nhật ngày hết hạn của gói hiện tại
                currentPackage.ExpireDate = currentPackage.ExpireDate.AddMonths(package.DurationMonths);
                await _userPackageRepo.UpdateAsync(currentPackage);

                //Tạo transaction cho lần gia hạn
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PackageId = packageId,
                    Amount = package.Price,
                    Currency = "USD",
                    TransactionType = "Renewal",
                    PaymentMethod = "CreditCard",
                    TransactionDate = DateTime.UtcNow,
                    PaymentStatus = GeneralEnum.PaymentStatusEnum.Successfully,
                    CreatedAt = DateTime.UtcNow
                };

                await _transactionRepo.CreateAsync(transaction);

                return SuccessResp.Ok("Membership renewed successfully");

            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError(ex.Message);
            }
        }

        public async Task<IActionResult> CancelMembership()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            try
            {
                var userId = payload.UserId;

                // Tìm gói thành viên hiện tại của người dùng
                var userPackage = await _userPackageRepo.WhereAsync(up => up.OwnerId == userId && up.ExpireDate > DateOnly.FromDateTime(DateTime.Now));

                if (!userPackage.Any())
                {
                    return ErrorResp.BadRequest("No active membership package found.");
                }

                var activePackage = userPackage.First();

                //Cập nhật trạng thái gói thành viên thành Canceled
                activePackage.ExpireDate = DateOnly.FromDateTime(DateTime.UtcNow);
                activePackage.Status = GeneralEnum.UserPackageStatusEnum.Cancel;

                await _userPackageRepo.UpdateAsync(activePackage);

                return SuccessResp.Ok("Membership package canceled successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError(ex.Message);
            }
        }

        public async Task<IActionResult> UpdatePackage(Guid packageId, PackageUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var package = await _packageRepo.FindByIdAsync(packageId);

            if (package == null)
            {
                return ErrorResp.NotFound("Package not found");
            }

            _mapper.Map(dto, package);
            package.UpdatedAt = DateTime.Now;

            await _packageRepo.UpdateAsync(package);

            return SuccessResp.Ok("Package updated successfully");
        }

        public async Task<IActionResult> DeletePackage(Guid packageId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var package = await _packageRepo.FindByIdAsync(packageId);

            if (package == null)
            {
                return ErrorResp.NotFound("Package not found");
            }

            package.Status = GeneralEnum.PackageStatusEnum.Deleted;
            package.UpdatedAt = DateTime.Now;

            await _packageRepo.UpdateAsync(package);

            return SuccessResp.Ok("Package has been deleted");
        }
    }
}
