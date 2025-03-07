using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ResponseCode;
using ApplicationLayer.DTOs.Auth;
using ApplicationLayer.DTOs.User;
using AutoMapper;
using DomainLayer.Constants;
using DomainLayer.Entities;
using DomainLayer.Helpers;
using InfrastructureLayer.Core.Cache;
using InfrastructureLayer.Core.Crypto;
using InfrastructureLayer.Core.JWT;
using InfrastructureLayer.Core.Mail;
using InfrastructureLayer.Database;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.Service
{
    public interface IAuthService
    {
        public Task<GgAuthResp> HandleGoogleLogin(string redirect, string state, GgAuthInfo info);
        public Task<IActionResult> HandleRegister(RegisterReq req);
        public Task<IActionResult> HandleLoginEmail(LoginReq req);
        public Task<IActionResult> HandleLoginGoogle(string redirect);
        //public Task<IActionResult> HandleLoginFirebase(string token);
        public Task<IActionResult> HandleVerifyGgToken(string token);
        public Task<IActionResult> HandleRefreshToken(RefreshReq req);
        public Task<IActionResult> HandleOTPForgotPassword(RequestOTP email);
        public Task<IActionResult> HandleVerifyOTPChangePassword(VerifyOTPChangePassword req);
    }

    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ICryptoService _cryptoService;
        private readonly IGenericRepository<User> _userRepo;
        private readonly ICacheService _cacheService;
        protected readonly IHttpContextAccessor _httpCtx;
        protected readonly IMailService _mailService;
        //protected readonly IFirebaseAdminService _firebaseService;

        public AuthService(
          IMapper mapper,
          ChildGrowthDbContext dbContext,
          IJwtService jwtService,
          IGenericRepository<User> userRepo,
          ICryptoService cryptoService,
          ICacheService cacheService,
          IHttpContextAccessor httpCtx,
          IMailService mailService,
          IRoleService roleService,
          IGenericRepository<Role> roleRepo
          //IFirebaseAdminService firebaseService
        )
        {
            _roleRepo = roleRepo;
            _mapper = mapper;
            _jwtService = jwtService;
            _userRepo = userRepo;
            _cryptoService = cryptoService;
            _cacheService = cacheService;
            _httpCtx = httpCtx;
            _mailService = mailService;
            _roleService = roleService;
            //_firebaseService = firebaseService;
        }

        private string GenerateAccessTk(Guid userId, string role, Guid sessionId, string email, UserStatusEnum status)
        {
            return _jwtService.GenerateToken(userId, role, sessionId, email, status, JwtConst.ACCESS_TOKEN_EXP);
        }

        public async Task<GgAuthResp> HandleGoogleLogin(string redirect, string state, GgAuthInfo info)
        {
            var user = await _userRepo.FirstOrDefaultAsync(x => x.Email.Equals(info.Email));

            if (user == null)
            {
                var newUser = new User
                {
                    Email = info.Email,
                    Name = info.FullName ?? "Guest",
                    Status = UserStatusEnum.NotVerified,
                    AuthType = AuthTypeEnum.Google,
                    UserName = info.Email,
                    RoleId = Guid.Parse(GeneralConst.ROLE_USER_GUID),
                };

                user = await _userRepo.CreateAsync(newUser);
                if (user == null)
                {
                    return new GgAuthResp
                    {
                        Success = false,
                        Message = "Cannot create user",
                    };
                }
            }

            var sessionId = Guid.NewGuid();
            var accessTk = GenerateAccessTk(user.Id, user.Role.RoleName, sessionId, user.Email, UserStatusEnum.NotVerified);

            var redisKey = $"local:state:{state}";
            await _cacheService.Set(redisKey, user, TimeSpan.FromMinutes(15));

            var redirectLink = redirect + "?access_token=" + accessTk + "&access_token_exp=" + DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds();

            return new GgAuthResp
            {
                Success = true,
                RedirectLink = redirectLink,
            };
        }

        public async Task<IActionResult> HandleRegister(RegisterReq req)
        {
            var hashedPassword = _cryptoService.HashPassword(req.Password);

            var user = await _userRepo.FirstOrDefaultAsync(x => x.Email.Equals(req.Email));

            if (user != null)
            {
                return ErrorResp.BadRequest("Email is already taken");
            }

            var newUser = new User
            {
                Email = req.Email,
                Name = req.Name,
                Phone = req.Phone,
                Password = hashedPassword,
                Status = UserStatusEnum.Active,
                AuthType = AuthTypeEnum.Email,
                UserName = req.Email,
                RoleId = Guid.Parse(GeneralConst.ROLE_USER_GUID)
            };

            var userAdded = await _userRepo.CreateAsync(newUser) ?? throw new Exception("Cannot create user");

            return SuccessResp.Ok(_mapper.Map<UserDto>(userAdded));
        }

        public async Task<IActionResult> HandleLoginEmail(LoginReq req)
        {

            if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
            {
                return ErrorResp.BadRequest("Email & Password are required");
            }

            var user = await _userRepo.FirstOrDefaultAsync(x => x.Email.Equals(req.Email));
            if (user == null)
            {
                return ErrorResp.NotFound("User not found");
            }

            if (user.Password == null || !_cryptoService.VerifyPassword(req.Password, user.Password))
            {
                return ErrorResp.BadRequest("Email or password is incorrect");
            }

            var role =_roleRepo.FoundOrThrowAsync(user.RoleId);

            var sessionId = Guid.NewGuid();
             var accessTk = GenerateAccessTk(user.Id, role.Result.RoleName, sessionId, user.Email, user.Status);
            var accessTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds();
            var refreshTk = _cryptoService.GenerateRandomToken(JwtConst.REFRESH_TOKEN_LENGTH);
            var refreshTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.REFRESH_TOKEN_EXP).ToUnixTimeSeconds();

            var userDto = _mapper.Map<UserDto>(user);
            // // cache refresh token
            var redisKey = $"users:{user.Id}:refresh_token:{refreshTk}";
            await _cacheService.Set(redisKey, userDto, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

            return SuccessResp.Ok(new LoginEmailResp
            {
                UserId = user.Id,
                AccessToken = accessTk,
                AccessTokenExpAt = accessTkExpAt,
                RefreshToken = refreshTk,
                RefreshTokenExpAt = refreshTkExpAt,
            });
        }

        public async Task<IActionResult> HandleLoginGoogle(string redirect)
        {
            var ctx = _httpCtx.HttpContext;
            if (ctx == null) return ErrorResp.BadRequest("Cannot get request");

            var request = ctx.Request;
            var state = _cryptoService.GenerateRandomToken(JwtConst.REFRESH_TOKEN_LENGTH);
            var host = $"{request.Scheme}://{request.Host}";

            return SuccessResp.Ok(new LoginGoogleResp
            {
                Token = state,
                RedirectLink = $"{host}/api/v1/auth/google/login?redirect={redirect}&state={state}",
            });
        }

        public async Task<IActionResult> HandleVerifyGgToken(string token)
        {
            var state = await _cacheService.Get<User>($"local:state:{token}");
            if (state == null)
            {
                return ErrorResp.BadRequest("Cannot verify token");
            }

            var sessionId = Guid.NewGuid();
            var accessTk = GenerateAccessTk(state.Id, state.Role.RoleName, sessionId, state.Email, state.Status);
            var accessTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds();
            var refreshTk = _cryptoService.GenerateRandomToken(JwtConst.REFRESH_TOKEN_LENGTH);
            var refreshTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.REFRESH_TOKEN_EXP).ToUnixTimeSeconds();

            var userDto = _mapper.Map<UserDto>(state);
            // // cache refresh token
            var redisKey = $"users:{state.Id}:refresh_token:{refreshTk}";
            await _cacheService.Set(redisKey, userDto, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

            return SuccessResp.Ok(new VerifyTokenResp
            {
                UserId = state.Id,
                AccessToken = accessTk,
                AccessTokenExpAt = accessTkExpAt,
                RefreshToken = refreshTk,
                RefreshTokenExpAt = refreshTkExpAt,
            });
        }

        public async Task<IActionResult> HandleRefreshToken(RefreshReq req)
        {

            if (string.IsNullOrEmpty(req.RefreshToken) || string.IsNullOrEmpty(req.UserId))
            {
                return ErrorResp.BadRequest("Refresh token & User ID are required");
            }

            var redisKey = $"users:{req.UserId}:refresh_token:{req.RefreshToken}";
            var user = await _cacheService.Get<UserDto>(redisKey);
            if (user == null)
            {
                return ErrorResp.BadRequest("Refresh token is invalid");
            }

            var sessionId = Guid.NewGuid();
            var accessTk = GenerateAccessTk(user.Id, user.Role.RoleName, sessionId, user.Email, user.Status);
            var accessTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds();

            return SuccessResp.Ok(new RefreshResp
            {
                AccessToken = accessTk,
                AccessTokenExpAt = accessTkExpAt,
            });
        }

        public async Task<IActionResult> HandleOTPForgotPassword(RequestOTP req)
        {
            var user = await _userRepo.WhereAsync(u => u.Email == req.Email);
            if (user == null || !user.Any()) // Ensure user exists before proceeding
            {
                return ErrorResp.NotFound("User not found in system");
            }
            var otp = StrHelper.GenerateRandomOTP();

            var redisKey = $"local:otp:{req.Email}:forgot_password";
            await _cacheService.Set(redisKey, otp, TimeSpan.FromMinutes(3));

            var subject = "Forgot Password OTP";
            var message = $"Your OTP is: {otp}";

            await _mailService.SendEmailAsync(req.Email, subject, message);

            return SuccessResp.Ok("OTP sent to your email");
        }

        public async Task<IActionResult> HandleVerifyOTPChangePassword(VerifyOTPChangePassword req)
        {
            var redisKey = $"local:otp:{req.Email}:forgot_password";
            var otp = await _cacheService.Get<string>(redisKey);

            if (otp == null)
            {
                return ErrorResp.BadRequest("OTP is invalid");
            }

            if (otp.Equals(req.Otp))
            {
                var user = await _userRepo.FirstOrDefaultAsync(x => x.Email.Equals(req.Email));
                if (user == null)
                {
                    return ErrorResp.NotFound("User not found");
                }

                user.Password = _cryptoService.HashPassword(req.NewPassword);

                await _userRepo.UpdateAsync(user);

                return SuccessResp.Ok("Password changed successfully");
            }
            else
            {
                return ErrorResp.BadRequest("OTP is incorrect");
            }
        }

        //public async Task<IActionResult> HandleLoginFirebase(string token)
        //{
        //    var user = await _firebaseService.VerifyIdTokenAsync(token);

        //    if (user == null)
        //    {
        //        return ErrorResp.BadRequest("Cannot verify token");
        //    }

        //    var email = user.Claims["email"].ToString();
        //    var name = user.Claims["name"].ToString();
        //    var picture = user.Claims["picture"].ToString();

        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return ErrorResp.BadRequest("Cannot get email from token");
        //    }

        //    var userDb = await _userRepo.GetUserByEmail(email);

        //    if (userDb == null)
        //    {
        //        var newUser = new User
        //        {
        //            Email = email,
        //            FullName = name ?? "Guest" + user.Uid,
        //            Status = UserStatusEnum.Active,
        //            AuthType = AuthTypeEnum.Firebase,
        //            Username = email,
        //            Avatar = picture,
        //            IsAdmin = false,
        //        };

        //        userDb = await _userRepo.AddAsync(newUser);
        //        if (userDb == null)
        //        {
        //            return ErrorResp.BadRequest("Cannot create user");
        //        }
        //    }

        //    var sessionId = Guid.NewGuid();

        //    var accessTk = GenerateAccessTk(userDb.Id, sessionId, userDb.Email, UserStatusEnum.Active, userDb.IsAdmin);

        //    var accessTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds();
        //    var refreshTk = _cryptoService.GenerateRandomToken(JwtConst.REFRESH_TOKEN_LENGTH);
        //    var refreshTkExpAt = DateTimeOffset.UtcNow.AddSeconds(JwtConst.REFRESH_TOKEN_EXP).ToUnixTimeSeconds();

        //    var userDto = _mapper.Map<UserDto>(userDb);
        //    // // cache refresh token
        //    var redisKey = $"users:{userDb.Id}:refresh_token:{refreshTk}";
        //    await _cacheService.Set(redisKey, userDto, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

        //    return SuccessResp.Ok(new LoginEmailResp
        //    {
        //        UserId = userDb.Id,
        //        AccessToken = accessTk,
        //        AccessTokenExpAt = accessTkExpAt,
        //        RefreshToken = refreshTk,
        //        RefreshTokenExpAt = refreshTkExpAt,
        //    });
        //}
    }
}
