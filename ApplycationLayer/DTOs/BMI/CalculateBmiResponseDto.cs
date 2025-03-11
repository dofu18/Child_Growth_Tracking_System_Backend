using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.BMI
{
    public class CalculateBmiResponseDto
    {
        public decimal Bmi { get; set; }
        public decimal BmiPercentile { get; set; }
        public string BmiCategory { get; set; }
    }
}
