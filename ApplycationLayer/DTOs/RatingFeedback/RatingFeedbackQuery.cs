using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.RatingFeedback
{
    public class RatingFeedbackQuery : PaginationReq
    {
        public string? SearchKeyword { get; set; }
    }
}
