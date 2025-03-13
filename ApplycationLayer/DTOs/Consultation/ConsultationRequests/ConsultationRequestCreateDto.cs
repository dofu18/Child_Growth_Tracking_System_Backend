using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.Children;
using DomainLayer.Entities;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Consultation.ConsultationRequests
{
    public class ConsultationRequestCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Attachments { get; set; }
    }
}
