using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Constants;
using InfrastructureLayer.Core.JWT;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Service
{
    public abstract class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpCtx;

        public BaseService(IMapper mapper, IHttpContextAccessor httpCtx)
        {
            _mapper = mapper;
            _httpCtx = httpCtx;
        }

        protected Payload? ExtractPayload()
        {
            var ctx = _httpCtx.HttpContext;
            if (ctx == null) return null;
            var payload = ctx.Items[JwtConst.PAYLOAD_KEY] as Payload;
            return payload;
        }
    }
}
