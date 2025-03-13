using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using static DomainLayer.Enum.GeneralEnum;
using Application.ResponseCode;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Agreement;

namespace ApplicationLayer.Service
{
    public interface IAlertService
    {
        Task<IActionResult> CheckAndSendHealthAlerts(Guid childId, decimal bmi);// Kiểm tra và gửi cảnh báo dựa vào BMI
        Task<IEnumerable<AlertDto>> GetHealthAlertsByUser(Guid userId); // Lấy danh sách cảnh báo sức khỏe theo UserId
        Task MarkAlertAsRead(Guid alertId); // Đánh dấu cảnh báo là đã đọc
    }

    public class AlertService : BaseService, IAlertService
    {
        private readonly IGenericRepository<BmiCategory> _bmiRepo;
        private readonly IGenericRepository<Alert> _alertRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Children> _childrenRepo;

        public AlertService(
            IGenericRepository<BmiCategory> bmiRepo,
            IGenericRepository<Alert> alertRepo,
            IGenericRepository<User> userRepo, IGenericRepository<Children> childrenRepo,
            IMapper mapper,
            IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _bmiRepo = bmiRepo;
            _alertRepo = alertRepo;
            _userRepo = userRepo;
            _childrenRepo = childrenRepo;
        }

        // Kiểm tra BMI và gửi cảnh báo
        public async Task<IActionResult> CheckAndSendHealthAlerts(Guid childId, decimal bmi)
        {
            // Kiểm tra token
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            // Kiểm tra xem thông báo chưa đọc cho trẻ này đã tồn tại chưa
            var existingAlert = await _alertRepo.FindAsync(a => a.ChildrentId == childId && !a.IsRead);
            if (existingAlert != null)
            {
                // Nếu đã có thông báo chưa đọc, không tạo thêm
                return SuccessResp.Ok("An unread alert already exists for this child.");
            }

            // Xác định thông báo dựa trên BMI tiêu chuẩn của WHO
            string alertMessage;

            if (bmi < 16.0m)
            {
                alertMessage = "Trẻ đang ở tình trạng gầy độ III (nguy cơ sức khỏe nghiêm trọng). Hãy tham khảo ý kiến bác sĩ ngay lập tức.";
            }
            else if (bmi >= 16.0m && bmi < 17.0m)
            {
                alertMessage = "Trẻ đang ở tình trạng gầy độ II. Hãy tham khảo ý kiến của bác sĩ.";
            }
            else if (bmi >= 17.0m && bmi < 18.5m)
            {
                alertMessage = "Trẻ đang ở tình trạng gầy độ I. Khuyến nghị tham khảo ý kiến bác sĩ.";
            }
            else if (bmi >= 18.5m && bmi < 25.0m)
            {
                alertMessage = "Trẻ có BMI bình thường. Tiếp tục duy trì chế độ dinh dưỡng và lối sống lành mạnh.";
            }
            else if (bmi >= 25.0m && bmi < 30.0m)
            {
                alertMessage = "Trẻ đang trong tình trạng thừa cân. Hãy điều chỉnh chế độ ăn uống và vận động hợp lý.";
            }
            else if (bmi >= 30.0m && bmi < 35.0m)
            {
                alertMessage = "Trẻ đang ở mức béo phì độ I. Khuyến nghị tham khảo ý kiến bác sĩ để có hướng dẫn cụ thể.";
            }
            else if (bmi >= 35.0m && bmi < 40.0m)
            {
                alertMessage = "Trẻ đang ở mức béo phì độ II. Hãy liên hệ với bác sĩ để có kế hoạch điều trị.";
            }
            else // bmi >= 40.0
            {
                alertMessage = "Trẻ đang ở mức béo phì độ III (nguy cơ sức khỏe nghiêm trọng). Hãy tham khảo ý kiến bác sĩ ngay lập tức.";
            }

            // Tạo thông báo mới
            var alert = new Alert
            {
                Id = Guid.NewGuid(),
                ChildrentId = childId,
                AlertDate = DateTime.Now,
                Message = alertMessage,
                ReveivedUserId = userId,
                IsRead = false, // Đánh dấu trạng thái chưa đọc
                CreatedAt = DateTime.Now
            };

            // Lưu thông báo vào cơ sở dữ liệu
            await _alertRepo.CreateAsync(alert);

            // Trả về thông báo thành công
            return SuccessResp.Created(new { message = "Health alert created successfully.", data = alert });
        }



        // Lấy danh sách cảnh báo sức khỏe theo UserId
        public async Task<IEnumerable<AlertDto>> GetHealthAlertsByUser(Guid userId)
        {
            // Truy xuất danh sách cảnh báo từ cơ sở dữ liệu
            var alerts = await _alertRepo.FindAllAsync(a => a.ReveivedUserId == userId);

            // Map các cảnh báo sang DTOs
            return alerts.Select(alert => new AlertDto
            {
                ChildrentId = alert.ChildrentId,
                ChildrenName = GetChildrenName(alert.ChildrentId), // Lấy tên trẻ từ hàm
                AlertDate = alert.AlertDate,
                Message = alert.Message,
                ReceivedUserId = alert.ReveivedUserId,
                ReceivedUserName = GetUserName(alert.ReveivedUserId), // Lấy tên người dùng từ hàm
                IsRead = alert.IsRead,
                CreatedAt = alert.CreatedAt,
                UpdatedAt = alert.UpdatedAt
            });
        }

        // Đánh dấu cảnh báo là đã đọc
        public async Task MarkAlertAsRead(Guid alertId)
        {
            var alert = await _alertRepo.FindByIdAsync(alertId);
            if (alert == null)
            {
                throw new Exception("Alert not found.");
            }

            alert.IsRead = true; // Đánh dấu là đã đọc
            alert.UpdatedAt = DateTime.Now;

            await _alertRepo.UpdateAsync(alert);
        }

        private string GetChildrenName(Guid childId)
        {
            // Tìm trẻ từ cơ sở dữ liệu bằng childId
            var child = _childrenRepo.FindByIdAsync(childId).Result;

            // Trả về tên trẻ nếu tìm thấy, ngược lại trả về giá trị mặc định
            return child?.Name ?? "Unknown Child";
        }

        private string GetUserName(Guid userId)
        {
            // Tìm người dùng từ cơ sở dữ liệu bằng userId
            var user = _userRepo.FindByIdAsync(userId).Result;

            // Trả về tên người dùng nếu tìm thấy, ngược lại trả về giá trị mặc định
            return user?.Name ?? "Unknown User";
        }

    }
}
