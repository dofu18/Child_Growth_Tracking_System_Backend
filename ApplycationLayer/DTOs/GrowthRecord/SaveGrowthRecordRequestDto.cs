﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.GrowthRecord
{
    public class SaveGrowthRecordRequestDto
    {
        public Guid ChildId { get; set; }
        public decimal Height { get; set; } // cm
        public decimal Weight { get; set; } // kg
        public GenderEnum Gender { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
