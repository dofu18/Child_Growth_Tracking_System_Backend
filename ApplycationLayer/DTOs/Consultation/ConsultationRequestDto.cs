using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Consultation
{
    public class ConsultationRequestDto
    {
        public Guid ChildrenId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Symptoms { get; set; }
        public string Attachments { get; set; } // File đường dẫn hoặc URL
    }

}
