using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Entities;

namespace ApplicationLayer.DTOs.Consultation.ConsultationResponses
{
    public class ConsultationResponseCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
    }
}
