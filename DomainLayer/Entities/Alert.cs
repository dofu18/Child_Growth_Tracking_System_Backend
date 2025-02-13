using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Alert : BaseEntity
    {
        public Guid ChildrentId { get; set; }
        public DateTime AlertDate { get; set; }
        public string Message { get; set; }
        public Guid ReveivedUserId { get; set; }
        public bool IsRead { get; set; }

        //Navigation Properties
        public Children Children { get; set; }
        public User ReceivedUser { get; set; }
    }
}
