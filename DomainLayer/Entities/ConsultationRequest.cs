using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class ConsultationRequest : BaseEntity
    {
        public DateTime RequestDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ConsultationRequestStatusEnum? Status { get; set; }
        public string Attachments { get; set; }
        public Guid UserRequestId { get; set; }
        public Guid DoctorReceiveId {  get; set; }

        //Navigation Properties
        public User DoctorReceive { get; set; }
        public User UserRequest { get; set; }
    }
}
