using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.WHOData
{
    public class WhoDataUpdateDto
    {
        public Guid Id { get; set; }
        public int AgeMonth { get; set; }
        public decimal Bmi { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
