using Application.ResponseCode;
using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DomainLayer.Enum.GeneralEnum;



namespace ApplicationLayer.Service
{
    public interface IChildrenService
    {
        Task<IActionResult> Create(ChildrenCreateDto dto);
        Task<IActionResult> GetAll();
        Task<IActionResult> Update(Guid id, ChildrenUpdateDto dto);
        Task<IActionResult> GetChildByParent(Guid parentId);
        Task<IActionResult> GetChildByToken();
        Task<IActionResult> Delete(Guid id);
        Task<IActionResult> HideChildren(Guid childId, bool isHidden);
        Task<IActionResult> SharingProfile(Guid childId, string recipientEmail);
        Task<IActionResult> GetSharedChildren();
        Task<IActionResult> GetMyHideChildren();
        Task<IActionResult> UnHideChild(Guid childId);

    }
    public class ChildrenService : BaseService, IChildrenService
    {
        private readonly IGenericRepository<Children> _childrenRepo;
        private readonly IGenericRepository<BmiCategory> _bmiCategoryRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<SharingProfile> _sharingRepo;
        private readonly IGenericRepository<UserPackage> _userPackageRepo;
        private readonly IGenericRepository<Package> _packageRepo;



        public ChildrenService(IGenericRepository<Children> childrenRepo, IGenericRepository<User> userRepo, IGenericRepository<SharingProfile> sharingRepo, IGenericRepository<BmiCategory> bmiCategoryRepo, IGenericRepository<UserPackage>userPackageRepo, IGenericRepository<Package> packageRepo, IMapper mapper, IHttpContextAccessor httpCtx) : base(mapper, httpCtx)
        {
            _childrenRepo = childrenRepo;
            _userRepo = userRepo;
            _sharingRepo = sharingRepo;
            _bmiCategoryRepo = bmiCategoryRepo;
            _userPackageRepo = userPackageRepo;
            _packageRepo = packageRepo;
        }

        public async Task<IActionResult> Create(ChildrenCreateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            // Lấy gói đăng ký hiện tại của user (nếu có)
            var activePackage = await _userPackageRepo.FirstOrDefaultAsync(up => up.OwnerId == userId && up.Status == UserPackageStatusEnum.OnGoing);

            // Nếu không có gói nào hoặc gói đã hết hạn, set max = 1
            int maxChildrenAllowed = 1;
            if (activePackage != null)
            {
                var package = await _packageRepo.FindByIdAsync(activePackage.PackageId);
                if (package != null)
                {
                    maxChildrenAllowed = package.MaxChildrentAllowed;
                }
            }

            // Kiểm tra số lượng trẻ hiện tại của user - sử dụng Count
            int currentChildrenCount = await _childrenRepo.CountAsync(c => c.ParentId == userId);


            // Kiểm tra nếu user còn slot để tạo con mới
            if (currentChildrenCount < maxChildrenAllowed)
            {
                // Tạo mới trẻ em
                var child = _mapper.Map<Children>(dto);
                child.ParentId = userId;
                child.Status = ChildrentStatusEnum.Active;
                child.CreatedAt = DateTime.UtcNow;

                await _childrenRepo.CreateAsync(child);
                return SuccessResp.Created("Children information added successfully");
            }
            else
            {
                return ErrorResp.BadRequest("You have reached the maximum number of children allowed.");
            }
        }


        public async Task<IActionResult> GetAll()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            var children = await _childrenRepo.WhereAsync(c => c.Status == ChildrentStatusEnum.Active);
            var result = _mapper.Map<List<ChildrentResponseDto>>(children);

            return SuccessResp.Ok(result);
        }

        public async Task<IActionResult> Update(Guid id, ChildrenUpdateDto dto)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var children = await _childrenRepo.FindByIdAsync(id);

            if (children == null)
            {
                return ErrorResp.NotFound("Children Not Found");
            }

            if (children.ParentId != userId)
            {
                return ErrorResp.Forbidden("You do not have permission to update this child.");
            }

            _mapper.Map(dto, children);

            await _childrenRepo.UpdateAsync(children);

            return SuccessResp.Ok("Children information updated successfully");
        }

        public async Task<IActionResult> GetChildByToken()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var userId = payload.UserId;

            var chidren = await _childrenRepo.WhereAsync(c => c.ParentId == userId && c.Status == ChildrentStatusEnum.Active);

            if (!chidren.Any())
            {
                return ErrorResp.NotFound("No children found for this parent");
            }
            var result = _mapper.Map<List<ChildrentResponseDto>>(chidren);

            return SuccessResp.Ok(chidren);
        }

        public async Task<IActionResult> GetChildByParent(Guid parentid)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            var chidren = await _childrenRepo.WhereAsync(c => c.ParentId == parentid && c.Status == ChildrentStatusEnum.Active);

            if (!chidren.Any())
            {
                return ErrorResp.NotFound("No children found for this parent");
            }
            var result = _mapper.Map<List<ChildrentResponseDto>>(chidren);

            return SuccessResp.Ok(chidren);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var children = await _childrenRepo.FindByIdAsync(id);

            if (children == null)
            {
                return ErrorResp.NotFound("Children Not Found");
            }

            if (children.ParentId != userId)
            {
                return ErrorResp.Forbidden("You do not have permission to delete this child.");
            }

            await _childrenRepo.DeleteAsync(id);

            return SuccessResp.Ok("Children information deleted successfully");
        }

        public async Task<IActionResult> HideChildren(Guid childId, bool isHidden)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var child = await _childrenRepo.FindByIdAsync(childId);
            if (child == null)
            {
                return ErrorResp.NotFound("Children Not Found");
            }

            if (child.ParentId != userId)
            {
                return ErrorResp.Forbidden("You do not have permission to hide this child's information.");
            }

            child.Status = isHidden ? ChildrentStatusEnum.Archived : ChildrentStatusEnum.Active;

            await _childrenRepo.UpdateAsync(child);

            return SuccessResp.Ok(isHidden ? "Child information is now hidden." : "Child information is now visible.");
        }

        public async Task<IActionResult> SharingProfile(Guid childId, string recipientEmail)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }

            // Kiểm tra trẻ có tồn tại không
            var child = await _childrenRepo.FindByIdAsync(childId);
            if (child == null)
            {
                return ErrorResp.NotFound("Child not found.");
            }

            // Kiểm tra người nhận có tồn tại không
            var receiver = await _userRepo.FirstOrDefaultAsync(u => u.Email == recipientEmail);
            if (receiver == null)
            {
                return ErrorResp.NotFound("Recipient not found.");
            }

            // Tạo bản ghi mới trong bảng SharingProfiles
            var shareProfile = new SharingProfile
            {
                Id = Guid.NewGuid(),
                UserId = receiver.Id,
                ChildrentId = childId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _sharingRepo.CreateAsync(shareProfile);

            return SuccessResp.Ok("Child's development information has been shared successfully.");
        }

        public async Task<IActionResult> GetSharedChildren()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            // Lấy danh sách trẻ đã được chia sẻ cho userId
            var sharedProfiles = await _sharingRepo.WhereAsync(
                filter: s => s.UserId == userId,
                navigationProperties: "Children"
            );

            var sharedChildren = sharedProfiles
                .Where(s => s.Children != null) // Lọc ra các bản ghi có dữ liệu trẻ
                .Select(s => s.Children)
                .ToList();

            if (!sharedChildren.Any())
            {
                return ErrorResp.NotFound("No shared children found.");
            }

            var childrenDto = _mapper.Map<List<ChildrenDto>>(sharedChildren);

            return SuccessResp.Ok(childrenDto);
        }

        public async Task<IActionResult> GetMyHideChildren()
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var myChild = await _childrenRepo.WhereAsync(c => c.ParentId == userId && c.Status == ChildrentStatusEnum.Archived);

            if (myChild.Count() == 0)
            {
                return ErrorResp.NotFound("Your Children Archived list is empty");
            }

            return SuccessResp.Ok(myChild);
        }

        public async Task<IActionResult> UnHideChild(Guid childId)
        {
            var payload = ExtractPayload();
            if (payload == null)
            {
                return ErrorResp.Unauthorized("Invalid token");
            }
            var userId = payload.UserId;

            var myChild = await _childrenRepo.WhereAsync(c => c.ParentId == userId && c.Status == ChildrentStatusEnum.Archived && c.Id == childId);
            if (myChild.Count() == 0)
            {
                return ErrorResp.NotFound("This child not found or child is not in Archived list");
            }

            var child = await _childrenRepo.FoundOrThrowAsync(childId);

            child.Status = ChildrentStatusEnum.Active;
            await _childrenRepo.UpdateAsync(child);

            return SuccessResp.Ok("Un-archive child successfully");
        }
    }
}
