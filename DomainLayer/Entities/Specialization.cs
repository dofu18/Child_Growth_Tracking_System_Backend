using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Specialization : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual User CreatedUser { get; set; }
    }
}
