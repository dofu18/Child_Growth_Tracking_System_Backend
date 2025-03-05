using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Doctor
{
    public class DoctorUpdateDto
    {
        public string? Certificate { get; set; } // Chứng chỉ của bác sĩ
        public string? LicenseNumber { get; set; } // Số giấy phép hành nghề
        public string? Biography { get; set; } // Tiểu sử, thông tin cá nhân
        public string? Metadata { get; set; } // Dữ liệu bổ sung
        public string? Specialize { get; set; } // Lĩnh vực chuyên môn
        public string? ProfileImg { get; set; } // Ảnh đại diện của bác sĩ
    }
}
