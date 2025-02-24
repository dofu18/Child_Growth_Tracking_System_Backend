using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Package
{
    public class RenewPackageDto
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
