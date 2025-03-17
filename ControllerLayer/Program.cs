using ApplicationLayer.DTOs.Supabase;
using ApplicationLayer.Service;
using DomainLayer.Entities;
using InfrastructureLayer.Core.Cache;
using InfrastructureLayer.Core.Crypto;
using InfrastructureLayer.Core.JWT;
using InfrastructureLayer.Core.Mail;
using InfrastructureLayer.Database;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using static ApplicationLayer.Service.IUserPackageService;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddBearerToken();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Child Growth System API", Version = "v1" });

    // Add a bearer token to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Require the bearer token for all API operations
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
          new OpenApiSecurityScheme
          {
              Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
              }
          },
          new string[] {}
      }
    });

    //File upload support
    c.SupportNonNullableReferenceTypes();
});

// Configure CORS
// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Allow frontend domain
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
//Google OAuth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetValue<string>("GoogleClientId") ?? throw new ArgumentNullException("GoogleClientId");
    options.ClientSecret = builder.Configuration.GetValue<string>("GoogleClientSecret") ?? throw new ArgumentNullException("GoogleClientSecret");
});

// Get Connection String from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext with PostgreSQL
builder.Services.AddDbContext<ChildGrowthDbContext>(options =>
    options.UseNpgsql(connectionString));

//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection") ?? throw new ArgumentNullException("RedisConnection")));


// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Core
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<ICryptoService, CryptoService>();
builder.Services.AddScoped<ICacheService, CacheService>();

var smtpUsername = builder.Configuration.GetValue<string>("SMTPEmail") ?? "smtp_email";
var smtpPassword = builder.Configuration.GetValue<string>("SMTPPassword") ?? "smtp_password";
builder.Services.AddSingleton<IMailService>(new MailService("smtp.gmail.com", 587, smtpUsername, smtpPassword));
//Supabase
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection("SupabaseSettings"));

// Register Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBmiCategoryService, BmiCategoryService>();
builder.Services.AddScoped<IChildrenService, ChildrenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDoctorLicenseService, DoctorLicenseService>();
builder.Services.AddScoped<IRatingFeedbackService, RatingFeedbackService>();
builder.Services.AddScoped<ITracsactionService, TransactionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserPackageService, UserPackageService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IBmiService, BmiService>();
builder.Services.AddScoped<IConsultationRequestService, ConsultationRequestService>();
builder.Services.AddScoped<IConsultationResponseService, ConsultationResponseService>();
builder.Services.AddScoped<IGrowthTrackingService, GrowthTrackingService>();
builder.Services.AddScoped<ISupabaseStorageService, SupabaseStorageService>();
builder.Services.AddScoped<IAlertService, AlertService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
