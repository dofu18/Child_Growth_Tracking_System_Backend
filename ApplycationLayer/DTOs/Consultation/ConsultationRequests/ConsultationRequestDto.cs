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
    public class ConsultationRequestDto
    {
        public Guid ChildrentId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ConsultationRequestStatusEnum? Status { get; set; }
        public string Attachments { get; set; }
        public Guid UserRequestId { get; set; }
        public Guid DoctorReceiveId { get; set; }
        public User DoctorReceive { get; set; }
        public User UserRequest { get; set; }
        public ChildrentResponseDto Children { get; set; }
    }
}
