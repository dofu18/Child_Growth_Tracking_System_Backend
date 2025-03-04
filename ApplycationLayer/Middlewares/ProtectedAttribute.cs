using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using DomainLayer.Constants;
using InfrastructureLayer.Core.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApplicationLayer.Middlewares
{
    // Middleware to protect routes
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ProtectedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IJwtService _jwtService;

        public ProtectedAttribute()
        {
            _jwtService = new JwtService();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = ErrorResp.Unauthorized("No token provided");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length);

            try
            {
                var payload = _jwtService.ValidateToken(token);

                // add payload to context
                context.HttpContext.Items[JwtConst.PAYLOAD_KEY] = payload;
            }
            catch (Exception e)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = ErrorResp.Unauthorized(e.Message);
                return;
            }
        }
    }

}
