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

        public async Task<IActionResult> CheckAndSendHealthAlerts(Guid childId, decimal bmi)
        {
            // Lấy danh mục BMI từ cơ sở dữ liệu
            var bmiCategories = await _bmiRepo.FindAllAsync();
            if (bmiCategories == null || !bmiCategories.Any())
            {
                return ErrorResp.NotFound("No BMI categories configured in the database.");
            }

            // Tìm danh mục BMI phù hợp
            var bmiCategory = bmiCategories.FirstOrDefault(c => bmi >= c.BmiBottom && bmi <= c.BmiTop);

            // Kiểm tra token
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            // Không tìm thấy danh mục BMI phù hợp
            if (bmiCategory == null)
            {
                return ErrorResp.NotFound("No BMI category found for the given BMI.");
            }

            // Kiểm tra xem thông báo chưa đọc cho trẻ này đã tồn tại chưa
            var existingAlert = await _alertRepo.FindAsync(a => a.ChildrentId == childId && !a.IsRead);
            if (existingAlert != null)
            {
                return SuccessResp.Ok("An unread alert already exists for this child.");
            }

            // Tạo thông báo mới
            string alertMessage = GetAlertMessage(bmi);
            var alert = new Alert
            {
                Id = Guid.NewGuid(),
                ChildrentId = childId,
                AlertDate = DateTime.UtcNow, // Use UtcNow instead of Now
                Message = alertMessage,
                ReveivedUserId = userId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow // Use UtcNow instead of Now
            };


            // Lưu thông báo vào cơ sở dữ liệu
            await _alertRepo.CreateAsync(alert);

            // Trả về thông báo thành công
            return SuccessResp.Created(new { message = "Health alert created successfully.", data = alert });
        }

        // Helper method to get alert message based on BMI
        private string GetAlertMessage(decimal bmi)
        {
            if (bmi < 16.0m)
                return "Trẻ đang ở tình trạng gầy độ III (nguy cơ sức khỏe nghiêm trọng). Hãy tham khảo ý kiến bác sĩ ngay lập tức.";
            if (bmi >= 16.0m && bmi < 17.0m)
                return "Trẻ đang ở tình trạng gầy độ II. Hãy tham khảo ý kiến của bác sĩ.";
            if (bmi >= 17.0m && bmi < 18.5m)
                return "Trẻ đang ở tình trạng gầy độ I. Khuyến nghị tham khảo ý kiến bác sĩ.";
            if (bmi >= 18.5m && bmi < 25.0m)
                return "Trẻ có BMI bình thường. Tiếp tục duy trì chế độ dinh dưỡng và lối sống lành mạnh.";
            if (bmi >= 25.0m && bmi < 30.0m)
                return "Trẻ đang trong tình trạng thừa cân. Hãy điều chỉnh chế độ ăn uống và vận động hợp lý.";
            if (bmi >= 30.0m && bmi < 35.0m)
                return "Trẻ đang ở mức béo phì độ I. Khuyến nghị tham khảo ý kiến bác sĩ để có hướng dẫn cụ thể.";
            if (bmi >= 35.0m && bmi < 40.0m)
                return "Trẻ đang ở mức béo phì độ II. Hãy liên hệ với bác sĩ để có kế hoạch điều trị.";
            return "Trẻ đang ở mức béo phì độ III (nguy cơ sức khỏe nghiêm trọng). Hãy tham khảo ý kiến bác sĩ ngay lập tức.";
        }




        public async Task<IEnumerable<AlertDto>> GetHealthAlertsByUser(Guid userId)
        {
            var alerts = await _alertRepo.FindAllAsync(a => a.ReveivedUserId == userId);

            // Map các cảnh báo sang DTOs
            var alertDtos = new List<AlertDto>();
            foreach (var alert in alerts)
            {
                var childrenName = await GetChildrenNameAsync(alert.ChildrentId); // Refactored to async
                var userName = await GetUserNameAsync(alert.ReveivedUserId); // Refactored to async

                alertDtos.Add(new AlertDto
                {
                    ChildrentId = alert.ChildrentId,
                    ChildrenName = childrenName,
                    AlertDate = alert.AlertDate,
                    Message = alert.Message,
                    ReceivedUserId = alert.ReveivedUserId,
                    ReceivedUserName = userName,
                    IsRead = alert.IsRead,
                    CreatedAt = alert.CreatedAt,
                    UpdatedAt = alert.UpdatedAt
                });
            }
            return alertDtos;
        }

        private async Task<string> GetChildrenNameAsync(Guid childId)
        {
            var child = await _childrenRepo.FindByIdAsync(childId);
            return child?.Name ?? "Unknown Child";
        }

        private async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = await _userRepo.FindByIdAsync(userId);
            return user?.Name ?? "Unknown User";
        }


        // Đánh dấu cảnh báo là đã đọc
        public async Task MarkAlertAsRead(Guid alertId)
        {
            var alert = await _alertRepo.FindByIdAsync(alertId);
            if (alert == null)
            {
                throw new Exception($"Alert with ID {alertId} not found.");
            }

            alert.IsRead = true;
            alert.UpdatedAt = DateTime.Now;

            await _alertRepo.UpdateAsync(alert);
        }
    }
}
