using Application.ResponseCode;
using ApplicationLayer.DTOs.Payment;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [Protected]
    [HttpPost("vnpay")]
        public async Task<IActionResult> CreateVnpayPayment([FromBody] PaymentRequestDto request)
        {
            try
            {
                var response = await _paymentService.CreateVnpayPaymentAsync(HttpContext, request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"[VNPAY] Unauthorized: {ex.Message}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[VNPAY] Error: {ex.Message}");
                return BadRequest(new { message = "Error processing payment", details = ex.Message });
            }
        }

    [Protected]
    [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnpayReturn()
        {
            try
            {
                // Lấy các tham số từ query string
                var response = await _paymentService.CallBack(Request.Query);

                // Kiểm tra kết quả và trả về phản hồi tương ứng
                if (response != null && response.Success)
                {
                    return Ok(new { message = "Payment successful", data = response });
                }

                return BadRequest(new { message = "Payment failed", details = response?.VnPayResponseCode });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during VNPAY callback: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}