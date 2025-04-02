using Application.ResponseCode;
using ApplicationLayer.DTOs.Package;
using ApplicationLayer.DTOs.Payments;
using ApplicationLayer.DTOs.Users;
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
        Task<IActionResult> RenewPackage(RenewPackageDto dto);
        Task<IActionResult> CancelMembership();
        Task<IActionResult> UpdatePackage(PackageUpdateDto dto);
        Task<IActionResult> DeletePackage(Guid packageId);
        Task<IActionResult> GetAllPackages();
        Task<IActionResult> GetUserPackageByToken();
        //admin
        Task<IActionResult> GetNumberUsingPackage();
        Task<IActionResult> UpdatePackageStatus(Guid packageId, [FromBody] PackageStatusEnum newStatus);
        //check duration
        Task<IActionResult> CheckDuration();
    }
    public class UserPackageService : BaseService, IUserPackageService
    {
        private readonly IGenericRepository<UserPackage> _userPackageRepo;
        private readonly IGenericRepository<Package> _packageRepo;
        private readonly IGenericRepository<Transaction> _transactionRepo;

        public UserPackageService(IGenericRepository<UserPackage> userPackageRepo, IGenericRepository<Package> packageRepo, IGenericRepository<Transaction> transactionRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _userPackageRepo = userPackageRepo;
            _packageRepo = packageRepo;
            _transactionRepo = transactionRepo;
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
                    BillingCycle = dto.BillingCycle,
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

        public async Task<IActionResult> RenewPackage(RenewPackageDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            try
            {
                var userId = payload.UserId;

                // Kiểm tra package có tồn tại không
                var package = await _packageRepo.FindByIdAsync(dto.PackageId);
                if (package == null)
                {
                    return ErrorResp.NotFound("Package not found");
                }

                //Kiểm tra user đã có gói đang hoạt động chưa
                var currentPackage = await _userPackageRepo
                                .WhereAsync(up => up.OwnerId == userId && up.Status == UserPackageStatusEnum.OnGoing);
                if (!currentPackage.Any())
                {
                    return ErrorResp.BadRequest("No active package found for renewal.");
                }

                // Lấy phần tử đầu tiên của danh sách
                var activePackage = currentPackage.FirstOrDefault();
                if (activePackage == null)
                {
                    return ErrorResp.BadRequest("No active package found.");
                }

                // Lấy ngày hiện tại dưới dạng DateOnly
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

                // Nếu gói vẫn còn hạn
                if (activePackage.ExpireDate > currentDate)
                {
                    return ErrorResp.BadRequest("Your package is still active, renewal is not necessary.");
                }

                // Xác định thời gian gia hạn dựa trên BillingCycleEnum
                DateOnly newExpireDate;
                if (dto.BillingCycle == BillingCycleEnum.Monthly)
                {
                    newExpireDate = activePackage.ExpireDate.AddMonths(1); // Gia hạn thêm 1 tháng
                }
                else if (dto.BillingCycle == BillingCycleEnum.Yearly)
                {
                    newExpireDate = activePackage.ExpireDate.AddYears(1); // Gia hạn thêm 1 năm
                }
                else
                {
                    return ErrorResp.BadRequest("Invalid billing cycle selection.");
                }

                // Cập nhật ngày hết hạn
                activePackage.ExpireDate = newExpireDate;

                // Cập nhật vào database
                await _userPackageRepo.UpdateAsync(activePackage);

                //Tạo transaction cho lần gia hạn
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PackageId = dto.PackageId,
                    Amount = package.Price,
                    Currency = "VND",
                    TransactionType = "Renewal",
                    PaymentMethod = "VNPAY",
                    TransactionDate = DateTime.UtcNow,
                    PaymentStatus = GeneralEnum.PaymentStatusEnum.Successfully,
                    CreatedAt = DateTime.UtcNow,
                    MerchantTransactionId = Guid.NewGuid().ToString(),
                    Description = "Renewal payment for package"
                };

                await _transactionRepo.CreateAsync(transaction);

                return SuccessResp.Ok("Membership renewed successfully");

            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError($"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}");
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

        public async Task<IActionResult> UpdatePackage(PackageUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var package = await _packageRepo.FindByIdAsync(dto.PackageId);

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

            // Lấy danh sách người dùng đang sử dụng package này
            var activeUsers = await _userPackageRepo.FindAllAsync(up => up.PackageId == packageId && up.Status == UserPackageStatusEnum.OnGoing,"Owner");

            if (activeUsers.Any())
            {
                var userList = activeUsers.Select(up => new
                {
                    up.Owner.Id,
                    up.Owner.UserName,
                    up.Owner.Email
                }).ToList();

                // Cập nhật trạng thái package thành Archived
                package.Status = GeneralEnum.PackageStatusEnum.Archived;
                package.UpdatedAt = DateTime.Now;
                await _packageRepo.UpdateAsync(package);

                return new JsonResult(new
                {
                    Message = "Cannot delete package because it is currently in use.",
                    ActiveUserCount = userList.Count(),
                    ActiveUsers = userList
                })
                {
                    StatusCode = 400
                };
            }

            // Xóa package (soft delete)
            package.Status = GeneralEnum.PackageStatusEnum.Deleted;
            package.UpdatedAt = DateTime.Now;

            await _packageRepo.UpdateAsync(package);

            return SuccessResp.Ok("Package has been deleted");
        }

        public async Task<IActionResult> GetAllPackages()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            try
            {
                var packages = await _packageRepo.ListAsync();

                var result = _mapper.Map<List<PackageDto>>(packages);

                return SuccessResp.Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError(ex.Message);
            }
        }

        public async Task<IActionResult> GetUserPackageByToken()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            try
            {
                var userPackages = await _userPackageRepo.WhereAsync(up => up.OwnerId == userId && up.Status == UserPackageStatusEnum.OnGoing, "Package");

                if (!userPackages.Any())
                {
                    return ErrorResp.NotFound("No package found for this user");
                }

                var result = _mapper.Map<List<UserPackageDto>>(userPackages);

                return SuccessResp.Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorResp.InternalServerError(ex.Message);
            }
        }

        public async Task<IActionResult> GetNumberUsingPackage()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var userPackages = await _userPackageRepo.ListAsync("Owner", "Package");

            var summary = userPackages
                .GroupBy(up => new { up.PackageId, up.OwnerId })
                .Select(g => new
                {
                    Packages = new PackageRespDto
                    {
                        Id = g.First().Package.Id,
                        PackageName = g.First().Package.PackageName
                    },
                    Owner = new UserRespDto
                    {
                        Id = g.First().Owner.Id,
                        Name = g.First().Owner.Name
                    },
                    TotalPackages = g.Count(),
                    ActivePackages = g.Count(up => up.ExpireDate >= today),
                    ExpiredPackages = g.Count(up => up.ExpireDate < today),
                    TotalRevenuePerPackage = g.Sum(up => up.PriceAtSubscription)
                })
                .OrderBy(g => g.Owner.Id)
                .ToList();

            var totalRevenueAllPackages = summary.Sum(x => x.TotalRevenuePerPackage);

            return SuccessResp.Ok(new
            {
                PackagesSummary = summary,
                TotalRevenueAllPackages = totalRevenueAllPackages
            });
        }

        public async Task<IActionResult> UpdatePackageStatus(Guid packageId, [FromBody] PackageStatusEnum newStatus)
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

            // Cập nhật trạng thái package
            package.Status = newStatus;
            package.UpdatedAt = DateTime.Now;
            await _packageRepo.UpdateAsync(package);

            return SuccessResp.Ok(new
            {
                Message = $"Package status has been updated to {newStatus}",
                PackageId = package.Id,
                NewStatus = newStatus
            });
        }

        public async Task<IActionResult> CheckDuration()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;
            var onGoingPack = await _userPackageRepo.FirstOrDefaultAsync(up => up.OwnerId == userId && up.Status == UserPackageStatusEnum.OnGoing);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (onGoingPack == null)
            {
                return SuccessResp.Ok("No package");
            }

            if (onGoingPack.ExpireDate >= today)
            {
                onGoingPack.Status = UserPackageStatusEnum.OnGoing;
            }
            else
            {
                onGoingPack.Status = UserPackageStatusEnum.Expired;
                onGoingPack.UpdatedAt = DateTime.UtcNow;
            }

            await _userPackageRepo.UpdateAsync(onGoingPack);
            return SuccessResp.Ok("Checked package duration");
        }
    }
}
