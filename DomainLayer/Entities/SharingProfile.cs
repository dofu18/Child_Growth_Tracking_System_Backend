using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class SharingProfile : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ChildrentId { get; set; }

        //Navigation Properties
        public User User { get; set; }
        public Children Children { get; set; }

    }
}
