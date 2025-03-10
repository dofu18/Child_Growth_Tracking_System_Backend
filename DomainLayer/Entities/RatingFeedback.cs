using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class RatingFeedback : BaseEntity
    {
        public Guid UserId { get; set; }
        public int Rating {  get; set; }
        public string Feedback { get; set; }
        public RatingFeedbackStatusEnum? Status { get; set; }
        public RatingTypeEnum? RatingType { get; set; }
        public Guid? DoctorId { get; set; } = Guid.Empty;
            
        //Navigation Properties
        public User User { get; set; }
        public DoctorLicense Doctor { get; set; }
    }
}
