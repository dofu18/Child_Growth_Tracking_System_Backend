using ApplicationLayer.DTOs.Package;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [Route("api/user-packages")]
    //[ApiController]
    public class UserPackageController : ControllerBase
    {
        private readonly IUserPackageService _userPackageService;
        private readonly ILogger<UserPackageController> _logger;

        public UserPackageController(ILogger<UserPackageController> logger, IUserPackageService userPackageService)
        {
            _logger = logger;
            _userPackageService = userPackageService;
        }

        [HttpPost("{userId}/renew")]
        public async Task<IActionResult> RenewMembership([FromRoute] Guid userId, [FromBody] RenewPackageDto dto)
        {
            _logger.LogInformation($"Renew Membership request received for User: {userId}, Package: {dto.PackageId}");

            return await _userPackageService.RenewPackage(userId, dto.PackageId);
        }

        [HttpPut("{packageId}")]
        public async Task<IActionResult> UpdatePackage(Guid packageId, [FromBody] PackageUpdateDto dto)
        {
            _logger.LogInformation($"Admin updating package {packageId}");

            return await _userPackageService.UpdatePackage(packageId, dto);
        }

        [HttpDelete("{packageId}")]
        public async Task<IActionResult> DeletePackage(Guid packageId)
        {
            _logger.LogInformation($"Admin deleting package {packageId}");

            return await _userPackageService.DeletePackage(packageId);
        }
    }
}
