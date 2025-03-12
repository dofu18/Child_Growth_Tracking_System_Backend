using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.Users;

namespace ApplicationLayer.DTOs.Feature
{
    public class FeatureDto
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public UserRespDto CreatedUser { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
