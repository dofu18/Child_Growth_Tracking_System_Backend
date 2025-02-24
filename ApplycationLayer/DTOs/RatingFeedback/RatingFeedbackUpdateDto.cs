using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.RatingFeedback
{
    public class RatingFeedbackUpdateDto
    {
        public int? Rating { get; set; }
        public string? Feedback { get; set; }
    }
}
