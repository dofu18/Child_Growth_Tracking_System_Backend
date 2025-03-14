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

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnpayReturn()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    // Lấy các tham số từ query string
                    var response = await _paymentService.CallBack(Request.Query);

                    if (response.Success)
                    {
                        return Ok(response);
                    }

                    return BadRequest(response);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return NotFound("Không tìm thấy thông tin thanh toán");
        }

        [Protected]
        [HttpGet("all")]
        public async Task<ActionResult<List<PaymentListDto>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            return Ok(payments);
        }
    }
}