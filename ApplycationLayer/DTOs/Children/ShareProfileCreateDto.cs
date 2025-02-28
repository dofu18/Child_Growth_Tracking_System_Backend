using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Children
{
    public class ShareProfileCreateDto
    {
        public Guid ChildId { get; set; } // ID hồ sơ trẻ

        public string RecipientEmail { get; set; } // Email người nhận

        public string Message { get; set; } // Nội dung tin nhắn
    }
}
