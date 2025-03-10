using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public Guid PackageId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
