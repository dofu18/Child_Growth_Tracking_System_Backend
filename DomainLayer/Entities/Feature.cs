using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Feature : BaseEntity
    {
        public string FeatureName { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        
        //Navigation Properties
        public User CreatedUser { get; set; }
    }
}
