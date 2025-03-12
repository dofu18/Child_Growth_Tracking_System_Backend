using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.Users;

namespace ApplicationLayer.DTOs.Feature
{
    public class FeatureCreateDto
    {
        public string FeatureName { get; set; }
        public string Description { get; set; }
    }
}
