using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class ConsultationResponse : BaseEntity
    {
        public Guid RequestId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }

        //Navigation Properties
        public ConsultationRequest ConsultationRequest { get; set; }
        public User Doctor {  get; set; }

    }
}
