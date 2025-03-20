using ApplicationLayer.DTOs.Package;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControllerLayer.Controllers
{
    [ApiController]
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

        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PackageCreateDto dto)
        {
            return await _userPackageService.CreatePackage(dto);
        }

        [Protected]
        [HttpPost("renew")]
        public async Task<IActionResult> RenewMembership([FromBody] RenewPackageDto dto)
        {
            _logger.LogInformation($"Renewing membership for Package: {dto.PackageId}");
            return await _userPackageService.RenewPackage(dto);
        }

        [Protected]
        [HttpPut("edit")]
        public async Task<IActionResult> UpdatePackage([FromBody] PackageUpdateDto dto)
        {
            _logger.LogInformation($"Admin updating package {dto.PackageId}");

            return await _userPackageService.UpdatePackage(dto);
        }

        [Protected]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePackage(Guid packageId)
        {
            _logger.LogInformation($"Admin deleting package {packageId}");

            return await _userPackageService.DeletePackage(packageId);
        }

        [Protected]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPackages()
        {
            _logger.LogInformation("Fetching all packages");
            return await _userPackageService.GetAllPackages();
        }

    }
}
