using Application.ResponseCode;
using ApplicationLayer.DTOs.Payment;
using ApplicationLayer.DTOs.Payments;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;


namespace ApplicationLayer.Service
{
    public interface IPaymentService
    {
        Task<string> CreateVnpayPaymentAsync(HttpContext context, PaymentRequestDto request);
        Task<PaymentResponseDto> CallBack(IQueryCollection queryParams);
    }

    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Package> _packageRepository;
        private readonly IGenericRepository<Transaction> _transactionRepository;

        public PaymentService(IConfiguration config, IGenericRepository<Package> packageRepository, IGenericRepository<Transaction> transactionRepository, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _config = config;
            _packageRepository = packageRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<string> CreateVnpayPaymentAsync(HttpContext context, PaymentRequestDto request)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            var userId = payload.UserId;


            var vnp_TmnCode = _config["Vnpay:TmnCode"];
            var vnp_HashSecret = _config["Vnpay:HashSecret"];
            var vnp_BaseUrl = _config["Vnpay:BaseUrl"];
            var vnp_ReturnUrl = _config["Vnpay:returnUrl"];

            var package = await _packageRepository.FindByIdAsync(request.PackageId);
            if (package == null)
                throw new Exception("Package not found");

            var random = new Random();
            // Tạo MerchantTransactionId (VNPAY không hỗ trợ GUID, nên tạo chuỗi số)
            var merchantTransactionId = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + random.Next(100, 999); ;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PackageId = request.PackageId,
                Amount = package.Price,
                Currency = "VND",
                TransactionType = "VNPAY",
                PaymentMethod = "VNPAY",
                TransactionDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatusEnum.Pending,
                Description = "Thanh toán gói " + package.PackageName,
                MerchantTransactionId = merchantTransactionId
            };

            await _transactionRepository.CreateAsync(transaction);

            string orderInfo = WebUtility.UrlEncode("Payment for package " + package.PackageName);


            // Sử dụng thư viện VNPAY.NET
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (package.Price * 100).ToString());            // VNPAY dùng đơn vị VND * 100
            vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
            vnpay.AddRequestData("vnp_OrderType", "orther");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", merchantTransactionId);
            

            if (string.IsNullOrEmpty(_config["Vnpay:BaseUrl"]))
            {
                throw new Exception("Vnpay:BaseUrl is missing in configuration.");
            }
            if (string.IsNullOrEmpty(_config["Vnpay:HashSecret"]))
            {
                throw new Exception("Vnpay:HashSecret is missing in configuration.");
            }

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<PaymentResponseDto> CallBack(IQueryCollection queryParams)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in queryParams)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            var vnp_merchantTransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = queryParams.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new PaymentResponseDto
                {
                    Success = false
                };

            }

            return new PaymentResponseDto
            {
                Success = true,
                PaymentMethod = "VNPAY",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_merchantTransactionId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                PaymentId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
