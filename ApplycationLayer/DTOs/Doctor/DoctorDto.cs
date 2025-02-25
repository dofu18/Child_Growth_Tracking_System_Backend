using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Doctor
{
    public class DoctorDto
    {
        public Guid Id { get; set; }

        public string Certificate { get; set; }

        public string LicenseNumber { get; set; }

        public string Biography { get; set; }

        public string MetaData { get; set; }

        public string Specialize { get; set; }

        public string ProfileImg { get; set; }

        public DoctorLicenseStatusEnum Status { get; set; }
    }
}
