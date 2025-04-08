using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class BmiCategory : BaseEntity
    {
        public string Name { get; set; }
        public decimal BmiTop { get; set; }
        public decimal BmiBottom { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
    }
}
