using ApplicationLayer.DTOs.VNPAY;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("/api/v1/vnpay")]
    public class VNPAYController : ControllerBase
    {
        private readonly IVNPAYService _vnPayService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VNPAYController> _logger;

        public VNPAYController(IVNPAYService vnPayService, IConfiguration configuration, ILogger<VNPAYController> logger)
        {
            _vnPayService = vnPayService;
            _configuration = configuration;
            _logger = logger;
        }

        [Protected]
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] VNPayCreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Creating payment for package {PackageId} with amount {Amount}", model.PackageId, model.Amount);
            var paymentUrl = await _vnPayService.CreatePayment(model.PackageId, model.Amount);
            return Ok(new { Url = paymentUrl });
        }

        [Protected]
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VNPayReturn([FromQuery] VNPayReturnModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var secretKey = _configuration["Vnpay:HashSecret"];
            var payParams = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
            payParams.Remove("vnp_SecureHash");
            payParams.Remove("vnp_SecureHashType");

            var rawData = string.Join("&", payParams.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}"));
            var secureHash = _vnPayService.GenerateSignature(rawData, secretKey);

            if (secureHash != model.vnp_SecureHash)
            {
                _logger.LogWarning("Invalid secure hash detected for transaction {TxnRef}", model.vnp_TxnRef);
                return BadRequest(new { Message = "Invalid secure hash." });
            }

            _logger.LogInformation("Processing VNPay return for transaction {TxnRef}", model.vnp_TxnRef);
            var result = await _vnPayService.ProcessVNPayReturn(model);
            return Ok(new { Message = result });
        }
    }
}
