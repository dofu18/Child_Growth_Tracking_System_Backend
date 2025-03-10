using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Doctor
{
    public class DoctorDto
    {
        public Guid Id { get; set; } // Id của hồ sơ bác sĩ

        public string Certificate { get; set; } // Tên hoặc thông tin chứng chỉ

        public string LicenseNumber { get; set; } // Số giấy phép hành nghề

        public string Biography { get; set; } // Tiểu sử cá nhân

        public string Metadata { get; set; } // Thông tin bổ sung hoặc metadata

        public string Specialize { get; set; } // Lĩnh vực chuyên môn của bác sĩ

        public string ProfileImg { get; set; } // URL hình ảnh hồ sơ bác sĩ

        public DoctorLicenseStatusEnum Status { get; set; } // Trạng thái giấy phép hành nghề

        public Guid UserId { get; set; } // Id của người dùng (user)
    }
}
