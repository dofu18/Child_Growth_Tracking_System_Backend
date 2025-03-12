using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Consultation.ConsultationRequests
{
    public class ConsultationRequestQuery : PaginationReq
    {
        public string? SearchKeyword { get; set; }
    }
}
