using ApplicationLayer.DTOs.VNPAY;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enum;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public interface IVNPAYService
    {
        Task<IActionResult> CreatePayment(Guid packageId, decimal money);
        Task<IActionResult> ProcessVNPayReturn(VNPayReturnModel model);
        string CreatePaymentUrl(int amount, string transactionId);
        string GenerateSignature(string data, string secretKey);
    }

    public class VNPayService : BaseService, IVNPAYService
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<UserPackage> _userPackageRepo;
        private readonly IGenericRepository<Package> _packageRepo;
        private readonly IGenericRepository<Transaction> _transactionRepo;

        public VNPayService(IGenericRepository<UserPackage> userPackageRepo, IGenericRepository<Package> packageRepo, IGenericRepository<Transaction> transactionRepo, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpCtx)
        : base(mapper, httpCtx)
        {
            _userPackageRepo = userPackageRepo;
            _packageRepo = packageRepo;
            _transactionRepo = transactionRepo;
            _configuration = configuration;
        }

        public async Task<IActionResult> CreatePayment(Guid packageId, decimal money)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return new UnauthorizedResult();
            }
            var userId = payload.UserId;

            var package = await _packageRepo.FindByIdAsync(packageId);
            if (package == null)
                return new NotFoundObjectResult("Package not found.");

            if (money != package.Price)
                return new BadRequestObjectResult("Invalid amount for package.");

            var transaction = new Transaction
            {
                TransactionDate = DateTime.UtcNow,
                Amount = money,
                PaymentMethod = "VNPAY",
                Currency = "VND",
                TransactionType = "Payment",
                PaymentStatus = GeneralEnum.PaymentStatusEnum.Pending,
                PackageId = packageId,
                UserId = userId,
                MerchantTransactionId = Guid.NewGuid().ToString(),
                Description = $"Payment for package {package.PackageName}"
            };

            await _transactionRepo.CreateAsync(transaction);

            var paymentUrl = CreatePaymentUrl((int)money, transaction.MerchantTransactionId);
            return new OkObjectResult(new { url = string.IsNullOrEmpty(paymentUrl) ? "URL is empty" : paymentUrl });
        }

        public async Task<IActionResult> ProcessVNPayReturn(VNPayReturnModel model)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return new UnauthorizedResult();
            }
            var userId = payload.UserId;

            var transaction = await _transactionRepo.FoundOrThrowAsync(Guid.Parse(model.vnp_TxnRef), "Transaction not found");
            if (transaction == null)
                return new NotFoundObjectResult("Transaction not found.");

            if (model.vnp_ResponseCode == "00")
            {
                transaction.PaymentStatus = GeneralEnum.PaymentStatusEnum.Successfully;
                transaction.PaymentDate = DateTime.UtcNow;

                var userPackage = new UserPackage
                {
                    OwnerId = userId,
                    PackageId = transaction.PackageId,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    ExpireDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(1),
                    Status = GeneralEnum.UserPackageStatusEnum.OnGoing
                };
                await _userPackageRepo.CreateAsync(userPackage);
            }
            else
            {
                transaction.PaymentStatus = GeneralEnum.PaymentStatusEnum.Failed;
            }

            await _transactionRepo.UpdateAsync(transaction);
            return model.vnp_ResponseCode == "00" ? new OkObjectResult("Payment Successful") : new BadRequestObjectResult("Payment Failed");
        }

        public string CreatePaymentUrl(int amount, string transactionId)
        {
            string baseUrl = _configuration.GetValue<string>("Vnpay:BaseUrl");
            string returnUrl = _configuration.GetValue<string>("Vnpay:returnUrl");
            string tmnCode = _configuration.GetValue<string>("Vnpay:TmnCode");
            string hashSecret = _configuration.GetValue<string>("Vnpay:HashSecret");

            if (string.IsNullOrEmpty(baseUrl))
            {
                Console.WriteLine("BaseUrl is missing");
                return string.Empty;
            }

            var payParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", tmnCode },
                { "vnp_Amount", (amount * 100).ToString() },
                { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", "127.0.0.1" },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Payment for transaction {transactionId}" },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", returnUrl },
                { "vnp_TxnRef", transactionId }
            };

            string rawData = string.Join("&", payParams.Select(p => $"{p.Key}={p.Value}"));
            string secureHash = GenerateSignature(rawData, hashSecret);
            return $"{baseUrl}?{rawData}&vnp_SecureHash={secureHash}";
        }

        public string GenerateSignature(string data, string secretKey)
        {
            Console.WriteLine($"Data: {data}");
            Console.WriteLine($"SecretKey: {secretKey}");

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null or empty");
            }
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey), "SecretKey cannot be null or empty");
            }

            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
    }
}


