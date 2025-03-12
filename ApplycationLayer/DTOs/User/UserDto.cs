using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Entities;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime LastLogin { get; set; }
        public string? Address { get; set; }
        //public Guid? PackageId { get; set; }
        public UserStatusEnum Status { get; set; }
    }

    public class UserRespDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
