using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DomainLayer.Entities
{
    public class UserPackage : BaseEntity
    {
        public Guid PackageId { get; set; }
        public Guid OwnerId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly ExpireDate { get; set; }

        //Navigation Properties
        public User CreatedUser { get; set; }
        public Package Package { get; set; }
        public User Owner { get; set; }
    }
}
