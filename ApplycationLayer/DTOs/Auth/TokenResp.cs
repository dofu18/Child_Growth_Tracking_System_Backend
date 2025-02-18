using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Auth
{
    public class TokenResp
    {
        public string? access_token { get; set; }
        public int access_token_exp { get; set; }
    }
}
