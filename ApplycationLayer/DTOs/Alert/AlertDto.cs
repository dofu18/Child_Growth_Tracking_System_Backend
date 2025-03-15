using System;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs
{
    public class AlertDto
    {
        public Guid ChildrentId { get; set; }
        public string? ChildrenName { get; set; } // Tên trẻ em
        public DateTime AlertDate { get; set; }
        public string Message { get; set; }
        public Guid ReceivedUserId { get; set; }
        public bool IsRead { get; set; }
        public string? ReceivedUserName { get; set; } // Tên người nhận
        public DateTime? CreatedAt { get; set; } // Thời gian tạo
        public DateTime? UpdatedAt { get; set; } // Thời gian cập nhật (nếu có)
    }
}
