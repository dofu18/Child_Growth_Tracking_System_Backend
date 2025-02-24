using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Package
{
    public class PackageUpdateDto
    {
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }
        public int TrialPeriodDays { get; set; }
        public int MaxChildrenAllowed { get; set; }
        public PackageStatusEnum Status { get; set; }
    }
}
