using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Consultation.ConsultationResponses
{
    public class ConsultationResponseQuery : PaginationReq
    {
        public string? SearchKeywords { get; set; }
    }
}
