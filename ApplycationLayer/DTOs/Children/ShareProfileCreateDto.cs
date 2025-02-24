using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Children
{
    public class ShareProfileCreateDto
    {
        public Guid ChildId { get; set; }
        //public Guid UserId {get set}
        public string RecipientEmail { get; set; }
        public string Message { get; set; }
    }
}
