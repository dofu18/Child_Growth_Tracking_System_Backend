using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public DoctorLicenseStatusEnum Status { get; set; }
        public Guid UserId { get; set; }

        //Navigation Properties
        public User User { get; set; }

    }
}
