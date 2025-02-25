using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Constants
{
    public static class JwtConst
    {
        public const int ACCESS_TOKEN_EXP = 60 * 60; // 60m
        public const int REFRESH_TOKEN_EXP = 3600 * 24 * 30; // 30 days
        public const int REFRESH_TOKEN_LENGTH = 24;

        public const string PAYLOAD_KEY = "payload";

    }
}
