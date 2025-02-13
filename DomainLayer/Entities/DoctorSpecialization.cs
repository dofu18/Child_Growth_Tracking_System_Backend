using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class DoctorSpecialization : BaseEntity
    {
        public Guid DoctorLicenseId { get; set; }
        public Guid SpecializtionId { get; set; }

        //Navigation Properties
        public DoctorLicense DoctorLicense { get; set; }
        public Specialization Specialization { get; set; }
    }
}
