using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.RatingFeedback
{
    public class RatingFeedbackCreateDto
    {
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public Guid? DoctorId { get; set; }
    }
}
