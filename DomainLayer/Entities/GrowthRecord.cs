using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class GrowthRecord : BaseEntity
    {
        public Guid ChildrentId { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Bmi { get; set; }
        public Guid BmiCategory { get; set; }
        public decimal BmiPercentile { get; set; }
        public string Notes { get; set; }
        
        //Navigation Properties
        public User Children { get; set; }
        public User CreatedUser { get; set; }
    }
}
