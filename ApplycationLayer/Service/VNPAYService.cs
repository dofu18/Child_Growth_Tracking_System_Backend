using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public interface IVNPAYService
    {
        string CreatePaymentUrl(Guid userId, Guid packageId, decimal amount, string returnUrl);
    }

    public class VNPayService : IVNPAYService
    {
        private readonly IConfiguration _configuration;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(Guid userId, Guid packageId, decimal amount, string returnUrl)
        {
            // Lấy mã website do VNPAY cấp từ appsettings.json
            var vnp_TmnCode = _configuration["VNPAY:TmnCode"];

            // Lấy URL cổng thanh toán của VNPAY từ appsettings.json
            var vnp_Url = _configuration["VNPAY:BaseUrl"];

            var time = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tạo URL thanh toán VNPAY
            var url = $"{vnp_Url}?vnp_Version=2.1.0" +  // Phiên bản VNPAY API
                      $"&vnp_Command=pay" +            // Lệnh thanh toán
                      $"&vnp_TmnCode={vnp_TmnCode}" +  // Mã website do VNPAY cấp
                      $"&vnp_Amount={amount * 100}" +   // Số tiền thanh toán (VNPAY yêu cầu *100)
                      $"&vnp_OrderInfo=Payment for package {packageId}" + // Nội dung đơn hàng
                      $"&vnp_CreateDate={time}" +       // Thời gian tạo giao dịch
                      $"&vnp_ReturnUrl={returnUrl}";    // URL callback sau khi thanh toán

            return url;
        }
    }
}


