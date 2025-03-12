using ApplicationLayer.DTOs.Transaction;
using ApplicationLayer.DTOs.Users;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("/api/v1/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITracsactionService _service;

        public TransactionController(ILogger<TransactionController> logger, ITracsactionService service)
        {
            _logger = logger;
            _service = service;
        }

        [Protected]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TransactionQuery req, PaymentStatusEnum? status)
        {
            _logger.LogInformation("Get my transactions history request received");

            return await _service.GetMyTransaction(req, status);
        }
    }
}
