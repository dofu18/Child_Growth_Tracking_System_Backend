using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Package
{
    public class RenewPackageDto
    {
        public Guid PackageId { get; set; }
        public BillingCycleEnum BillingCycle { get; set; }
    }
}
