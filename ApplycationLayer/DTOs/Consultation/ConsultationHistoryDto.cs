using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Consultation
{
    public class ConsultationHistoryDto
    {
        public Guid RequestId { get; set; }
        public string Title { get; set; }
        public string DoctorName { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public string ResponseContent { get; set; }
    }

}
