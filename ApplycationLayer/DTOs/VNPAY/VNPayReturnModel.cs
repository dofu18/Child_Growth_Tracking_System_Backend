using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.VNPAY
{
    public class VNPayReturnModel
    {
        public string vnp_TxnRef { get; set; } // Mã giao dịch của bạn
        public string vnp_ResponseCode { get; set; } // Mã phản hồi của VNPay
        public string vnp_TransactionNo { get; set; } // Mã giao dịch của VNPay
        public string vnp_Amount { get; set; } // Số tiền thanh toán
        public string vnp_OrderInfo { get; set; } // Thông tin đơn hàng
        public string vnp_PayDate { get; set; } // Ngày thanh toán
        public string vnp_SecureHash { get; set; } // Chuỗi mã hóa dùng để xác minh giao dịch
    }
}
