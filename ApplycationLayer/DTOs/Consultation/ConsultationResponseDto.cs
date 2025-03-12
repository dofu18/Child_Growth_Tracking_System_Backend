using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Consultation
{
    public class ConsultationResponseDto
    {
        public Guid RequestId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; } // File đường dẫn hoặc URL
    }
}
