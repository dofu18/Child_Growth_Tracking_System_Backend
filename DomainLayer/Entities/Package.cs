using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class Package : BaseEntity
    {
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }
        public int TrialPeriodDays { get; set; }
        public int MaxChildrentAllowed {  get; set; }
        public Guid CreatedBy { get; set; }
        public PackageStatusEnum Status { get; set; }

        //Navigation Properties
        public User CreatedUser { get; set; }
    }
}
