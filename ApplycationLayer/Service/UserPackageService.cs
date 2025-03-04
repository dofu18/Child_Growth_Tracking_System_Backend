using Application.ResponseCode;
using ApplicationLayer.DTOs.Package;
using ApplicationLayer.DTOs.Payment;
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
        Task<IActionResult> CreatePackage(PackageCreateDto dto, Guid userId);
        Task<IActionResult> RenewPackage(Guid userId, Guid packageId);
        Task<IActionResult> UpdatePackage(Guid packageId, PackageUpdateDto dto);
        Task<IActionResult> DeletePackage(Guid packageId);
        Task<IActionResult> ProcessPayment(Guid userId, Guid packageId, string paymentMethod, decimal money);
        Task<IActionResult> VnPayReturn(string vnp_ResponseCode, Guid transactionId);
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

        public async Task<IActionResult> CreatePackage(PackageCreateDto dto, Guid userId)
        {
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



        public async Task<IActionResult> RenewPackage(Guid userId, Guid packageId)
        {
            try
            {
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

        public async Task<IActionResult> CancelMembership(CancelPackageDto dto)
        {
            try
            {
                // Tìm gói thành viên hiện tại của người dùng
                var userPackage = await _userPackageRepo.WhereAsync(up => up.OwnerId == dto.UserId && up.ExpireDate > DateOnly.FromDateTime(DateTime.Now));

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

        public async Task<IActionResult> ProcessPayment(Guid userId, Guid packageId, string paymentMethod, decimal money)
        {
            try
            {
                var package = await _packageRepo.FindByIdAsync(packageId);

                if (package == null)
                {
                    return ErrorResp.NotFound("Package not found");
                }

                var merchantTransactionId = Guid.NewGuid().ToString();

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PackageId = packageId,
                    Amount = money,
                    Currency = "USD",
                    TransactionType = "Membership",
                    PaymentMethod = paymentMethod,
                    TransactionDate = DateTime.UtcNow,
                    PaymentStatus = PaymentStatusEnum.Pending,
                    MerchantTransactionId = merchantTransactionId,
                    Description = $"Payment for package {packageId} by user {userId}"

                };

                await _transactionRepo.CreateAsync(transaction);

                // Tạo URL thanh toán VNPAY
                var returnUrl = _configuration["Vnpay:ReturnUrl"] + "?transactionId=" + transaction.Id; // URL callback
                var paymentUrl = _vnPayService.CreatePaymentUrl(userId, packageId, money, returnUrl);

                var response = new PaymentResponseDto
                {
                    Message = "Payment URL created",
                    PaymentUrl = paymentUrl
                };
                return SuccessResp.Ok(response);

            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError($"Exception: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<IActionResult> VnPayReturn(string vnp_ResponseCode, Guid transactionId)
        {
            try
            {
                var transaction = await _transactionRepo.FindByIdAsync(transactionId);
                if (transaction == null)
                {
                    return ErrorResp.NotFound("Transaction not found");
                }

                if (vnp_ResponseCode == "00") // 00 là mã thành công của VNPAY
                {
                    transaction.PaymentStatus = PaymentStatusEnum.Successfully;
                    transaction.PaymentDate = DateTime.UtcNow;

                    var package = await _packageRepo.FindByIdAsync(transaction.PackageId);

                    var userPackage = new UserPackage
                    {
                        OwnerId = transaction.UserId,
                        PackageId = package.Id,
                        StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                        ExpireDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(package.DurationMonths),
                        Status = UserPackageStatusEnum.OnGoing
                    };

                    await _userPackageRepo.CreateAsync(userPackage);
                }
                else
                {
                    // If payment failed
                    transaction.PaymentStatus = PaymentStatusEnum.Failed;
                }

                await _transactionRepo.UpdateAsync(transaction);

                return SuccessResp.Ok("Payment processed successfully");
            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError($"Exception: {ex.Message}");
            }
        }
    }
}
