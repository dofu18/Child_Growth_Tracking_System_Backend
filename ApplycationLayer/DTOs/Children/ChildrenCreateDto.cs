using DomainLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Childrens
{
    public class ChildrenCreateDto
    {
        public string Name { get; set; }
        public DateOnly DoB { get; set; }
        public GenderEnum Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string Notes { get; set; }
    }
}
