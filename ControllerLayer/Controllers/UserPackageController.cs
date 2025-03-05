using ApplicationLayer.DTOs.Package;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControllerLayer.Controllers
{
    [Route("api/v1/user-packages")]
    public class UserPackageController : ControllerBase
    {
        private readonly IUserPackageService _userPackageService;
        private readonly ILogger<UserPackageController> _logger;

        public UserPackageController(ILogger<UserPackageController> logger, IUserPackageService userPackageService)
        {
            _logger = logger;
            _userPackageService = userPackageService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PackageCreateDto dto)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            return await _userPackageService.CreatePackage(dto, adminId);
        }

        [HttpPost("renew/{userId}")]
        public async Task<IActionResult> RenewMembership([FromRoute] Guid userId, [FromBody] RenewPackageDto dto)
        {
            _logger.LogInformation($"Renew Membership request received for User: {userId}, Package: {dto.PackageId}");

            return await _userPackageService.RenewPackage(userId, dto.PackageId);
        }

        [HttpPut("edit/{packageId}")]
        public async Task<IActionResult> UpdatePackage(Guid packageId, [FromBody] PackageUpdateDto dto)
        {
            _logger.LogInformation($"Admin updating package {packageId}");

            return await _userPackageService.UpdatePackage(packageId, dto);
        }

        [HttpDelete("delete/{packageId}")]
        public async Task<IActionResult> DeletePackage(Guid packageId)
        {
            _logger.LogInformation($"Admin deleting package {packageId}");

            return await _userPackageService.DeletePackage(packageId);
        }

        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromQuery] Guid userId, [FromQuery] Guid packageId, [FromQuery] string paymentMethod, decimal money)
        {
            var result = await _userPackageService.ProcessPayment(userId, packageId, paymentMethod, money);
            return result;
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn([FromQuery] string vnp_ResponseCode, [FromQuery] Guid transactionId)
        {
            var result = await _userPackageService.VnPayReturn(vnp_ResponseCode, transactionId);
            return result;
        }
    }
}
