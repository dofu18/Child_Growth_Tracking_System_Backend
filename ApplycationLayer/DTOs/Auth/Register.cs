using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.Users;

namespace ApplicationLayer.DTOs.Auth
{
    public class RegisterReq
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }

    public class RegisterResp : BaseResp
    {
        public UserDto User { get; set; } = null!;
    }
}
