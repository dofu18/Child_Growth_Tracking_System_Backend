using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class PackageFeature : BaseEntity
    {
        public Guid PackageId { get; set; }
        public Guid FeatureId { get; set; }
    }
}
