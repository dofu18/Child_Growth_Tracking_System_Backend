﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Children
{
    public class ChildrenDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly DoB { get; set; }
        public GenderEnum? Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Bmi { get; set; }
        public decimal BmiPercentile { get; set; }
        public string Notes { get; set; } 
        public GroupAgeEnum? GroupAge { get; set; }
        public ChildrentStatusEnum? Status { get; set; }
    }
}
