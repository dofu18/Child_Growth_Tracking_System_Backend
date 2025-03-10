using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Payment
{
    public class PaymentListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PackageName { get; set; }  // Lấy từ bảng Package
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string TransactionType { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
