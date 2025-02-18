using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Auth
{
    public class RefreshReq
    {
        public string RefreshToken { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }

    public class RefreshResp
    {
        public string AccessToken { get; set; } = null!;
        public long AccessTokenExpAt { get; set; }
    }
}
