using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class RatingFeedback : BaseEntity
    {
        public Guid UserId { get; set; }
        public int Rating {  get; set; }
        public string Commnet { get; set; }
        public DateTime FeedbackDate { get; set; }

        //Navigation Properties
        public User User { get; set; }
    }
}
