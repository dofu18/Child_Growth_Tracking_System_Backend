using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.RatingFeedback
{
    public class RatingFeedbackDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public RatingFeedbackStatusEnum Status { get; set; }
    }
}
