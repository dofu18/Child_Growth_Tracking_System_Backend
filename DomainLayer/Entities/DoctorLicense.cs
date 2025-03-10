using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class DoctorLicense : BaseEntity
    {
        public string Certificate { get; set; }
        public string LicenseNumber { get; set; }
        public string Biography { get; set; }
        public string Metadata { get; set; }
        public string Specialize {  get; set; }
        public string ProfileImg {  get; set; }
        public DoctorLicenseStatusEnum? Status { get; set; }
        public Guid UserId { get; set; }
        public int RatingAvg { get; set; }
        public string? Degrees { get; set; }
        public string Research { get; set; }
        public string Languages { get; set; }

        //Navigation Properties
        public User User { get; set; }

    }
}
