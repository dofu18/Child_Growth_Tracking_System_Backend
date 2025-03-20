using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Alert
{
    //DTO để truyền dữ liệu vào endpoint
    public class CheckHealthAlertRequestDto
    {
        public Guid ChildId { get; set; } // ID của trẻ
        public decimal Bmi { get; set; } // Chỉ số BMI
    }

}
