using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Navigation Properties
        public User CreatedUser { get; set; }
    }
}
