using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Domin.Services;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.Extension;
using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureSerilog();

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();

//Seed roles and users
await SeedDataAsync(app);

// Configure HTTP request pipeline
ConfigureApp(app);

app.Run();

// --- Service Configuration ---
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Database Context & Identity
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddIdentity<CoachFrikaUsers, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
    // Disable cookie authentication (optional, but may be necessary if it's conflicting)
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = ""; // Prevent automatic redirection to login page
        options.AccessDeniedPath = ""; // Customize access denied path if needed
    });

    // Service injections
    services.AddHttpContextAccessor();
    services.AddTransient<IUnitOfWork, UnitOfWork>();
    services.AddTransient<ILogicService, LogicService>();
    services.AddTransient<IWebHelpers, WebHelpers>();
    services.AddTransient<IJwtService, JwtService>();
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<IEmailService, EmailService>();
    services.Configure<EmailConfigSettings>(configuration.GetSection("EmailConfig"));
    services.AddSingleton<GoogleSheetsHelper>();

    // Controllers with authorization
    services.AddControllersWithViews();

    // JWT Authentication
    ConfigureJwtAuthentication(services, configuration);

    // Authorization Policies
    ConfigureAuthorizationPolicies(services);

    // Swagger Configuration
    ConfigureSwagger(services);
}

// --- JWT Authentication ---
void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
            };
        });

}

// --- Authorization Policies ---
void ConfigureAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminPolicy", policy => policy.RequireRole(AppRoles.Admin));
        options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole(AppRoles.SuperAdmin));
        options.AddPolicy("CoachPolicy", policy => policy.RequireRole(AppRoles.Coach));
        options.AddPolicy("TeacherPolicy", policy => policy.RequireRole(AppRoles.Teacher));
    });
}

// --- Swagger Configuration ---
void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CoachFrika API",
            Version = "v1"
        });

        // Bearer token for Swagger UI
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

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
                Array.Empty<string>()
            }
        });
    });
}

// --- Seed Data Initialization ---
async Task SeedDataAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CoachFrikaUsers>>();

        await SeedRoles.Initialize(roleManager);
        await SeedUsers.Initialize(userManager);
    }
}

// --- Configure HTTP Request Pipeline ---
void ConfigureApp(WebApplication app)
{
  
        app.UseSwagger(); // Enable Swagger UI
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoachFrika API v1");
            options.RoutePrefix = "swagger"; // Swagger UI at /swagger
        });
   
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseStaticFiles();

    app.UseAuthentication();  // Enable authentication middleware
    app.UseAuthorization();   // Enable authorization middleware

    // Default MVC route
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}
