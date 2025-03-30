using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Package
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public BillingCycleEnum BillingCycle { get; set; }
        public int MaxChildrentAllowed { get; set; }
        public PackageStatusEnum? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PackageRespDto
    {
        public Guid Id { get; set; }
        public string PackageName { get; set; }
    }
}
