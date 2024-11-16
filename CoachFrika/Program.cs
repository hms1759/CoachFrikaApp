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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureSerilog();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<CoachFrikaUsers, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IPublicService, PublicService>();
builder.Services.AddTransient<IWebHelpers, WebHelpers>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IEmailService, EmailService>(); // Add email service
builder.Services.Configure<EmailConfigSettings>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.AddSingleton(typeof(GoogleSheetsHelper));
// Add JWT authentication service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });
// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CoachFrika API",
        Version = "v1"
    });
});
builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CoachFrikaUsers>>();

    await SeedRoles.Initialize(roleManager);
    await SeedUsers.Initialize(userManager);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger UI only in development
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();


app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();

// Default MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");  // Default route to Home/Index

app.Run();

