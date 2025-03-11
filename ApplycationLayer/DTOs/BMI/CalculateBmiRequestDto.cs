using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.BMI
{
    public class CalculateBmiRequestDto
    {
        public decimal Height { get; set; } // cm
        public decimal Weight { get; set; } // kg
        public int AgeInMonths { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
