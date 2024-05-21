using IdentityServerApi.Application.Errors;
using IdentityServerApi.Core;
using IdentityServerApi.Core.Models;
using IdentityServerApi.Endpoints;
using IdentityServerApi.Endpoints.Handlers;
using IdentityServerApi.Infrastructure.Email;
using IdentityServerApi.Infrastructure.Jwt;
using IdentityServerApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("UserPolicy", policy =>
            policy
                .RequireRole("User")
                .RequireClaim(ClaimTypes.NameIdentifier))
    .AddPolicy("AdminPolicy", policy =>
            policy
                .RequireRole("Admin")
                .RequireClaim(ClaimTypes.NameIdentifier))
    .AddPolicy("AuthenticatedUser", policy =>
            policy
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.NameIdentifier));

builder.Services.AddCors();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
        
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddSignInManager<SignInManager<User>>()
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetRequiredSection(JwtOptions.Name));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetRequiredSection(EmailOptions.Name));
builder.Services.Configure<ExternalLinksOptions>(builder.Configuration.GetRequiredSection(ExternalLinksOptions.Name));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IEmailLinkProvider, EmailLinkProvider>();
builder.Services.AddScoped<IEmailSender<User>, EmailSender>();
builder.Services.AddScoped<ErrorDescriber>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");

builder.Services.AddCors();

var app = builder.Build();
app.UseExceptionHandler(o => { });
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

CultureInfo[] supportedCultures =
[
    new CultureInfo("en"),
    new CultureInfo("ru")
];

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    ApplyCurrentCultureToResponseHeaders = true
});

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityEndpoints();

app.Run();
