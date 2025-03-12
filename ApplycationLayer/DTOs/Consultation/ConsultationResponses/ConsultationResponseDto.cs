using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Entities;

namespace ApplicationLayer.DTOs.Consultation.ConsultationResponses
{
    public class ConsultationResponseDto
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public ConsultationRequest ConsultationRequest { get; set; }
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
    }
}
