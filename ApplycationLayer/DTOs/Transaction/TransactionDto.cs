using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Transaction
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string TransactionType { get; set; }
        public string PaymentMethod { get; set; }
        //Ngày tạo giao dịch
        public DateTime TransactionDate { get; set; }
        // Ngày thực hiện giao dịch thành công
        public DateTime PaymentDate { get; set; }

        public string MerchantTransactionId { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public string Description { get; set; }
    }
}
