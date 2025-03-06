using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Payments
{
    public class PaymentResponseDto
    {
        public string Message { get; set; }
        public string PaymentUrl { get; set; }
    }
}
