using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.VNPAY
{
    public class VNPayCreateModel
    {
        public Guid PackageId { get; set; }
        public decimal Amount { get; set; }
    }
}
