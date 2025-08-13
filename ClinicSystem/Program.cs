using ClinicSystem.BLL;
using ClinicSystem.BLL.Interfaces;
using ClinicSystem.BLL.Services;
using ClinicSystem.DAL;
using ClinicSystem.DAL.Data;
using ClinicSystem.DAL.Model;
using ClinicSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region DBcontext

var ConnectionStrings = builder.Configuration.GetConnectionString("SQLDataBase");
builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(ConnectionStrings);
});

#endregion

#region Default
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion
  
#region Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();
#endregion

#region  Authentication

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = "ClinicSystem";
    options.DefaultChallengeScheme = "ClinicSystem";

})
    .AddJwtBearer("ClinicSystem", options =>
    {
        var SecretKeyString = builder.Configuration.GetValue<string>("JWT:SecretKey") ?? "";
        var SecretKeyInByte = Encoding.ASCII.GetBytes(SecretKeyString);
        var SecretKey = new SymmetricSecurityKey(SecretKeyInByte);

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = SecretKey,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", builder => builder.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("PatientOnly", builder => builder.RequireClaim(ClaimTypes.Role, "Patient"));
    options.AddPolicy("DoctorOnly", builder => builder.RequireClaim(ClaimTypes.Role, "Doctor"));
});
#endregion

#region Inject Services
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IRoleService,RoleService>();
builder.Services.AddScoped<IAdminService,AdminService>();
builder.Services.AddScoped<ISpecialtyService,SpecialtyService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
#endregion

#region UintOfWork
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
#endregion

#region Inject Repository
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IWorkingHourRepository, WorkingHourRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
#endregion

var app = builder.Build();

#region Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion