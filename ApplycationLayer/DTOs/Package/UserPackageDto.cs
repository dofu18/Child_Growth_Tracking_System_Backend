using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Package
{
    public class UserPackageDto
    {
        public Guid PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal PriceAtSubscription { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly ExpireDate { get; set; }
        public int MaxChildrentAllowed { get; set; }
        public UserPackageStatusEnum? Status { get; set; }
    }
}
