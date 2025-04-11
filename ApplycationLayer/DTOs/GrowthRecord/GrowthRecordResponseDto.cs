using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.GrowthRecord
{
    public class GrowthRecordResponseDto
    {
        public Guid RecordId { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal Bmi { get; set; }
        public decimal BmiPercentile { get; set; }
        public string BmiCategory { get; set; }
        public string Notes { get; set; }
        public int ageInMonth { get; set; }
        public int ageInYear { get; set; }
    }
}
