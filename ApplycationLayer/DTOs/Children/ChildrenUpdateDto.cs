using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Children
{
    public class ChildrenUpdateDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
    }
}
