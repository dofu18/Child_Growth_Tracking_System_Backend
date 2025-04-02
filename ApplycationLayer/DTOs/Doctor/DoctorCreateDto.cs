using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Doctor
{
    public class DoctorCreateDto
    {
        public string Certificate { get; set; } // Tên hoặc thông tin chứng chỉ
        public string LicenseNumber { get; set; } // Số giấy phép hành nghề
        public string Biography { get; set; } // Tiểu sử cá nhân
        public string Metadata { get; set; } // Thông tin bổ sung hoặc metadata
        public string Specialize { get; set; } // Lĩnh vực chuyên môn của bác sĩ
        public string ProfileImg { get; set; } // URL hình ảnh hồ sơ bác sĩ
        public string? Degrees { get; set; }
        public string Research { get; set; }
        public string Languages { get; set; }
    }
}
