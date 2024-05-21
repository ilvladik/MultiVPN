using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Infrastructure.OutlineApi;
using System.Globalization;
using System.Text;
using OutlineServerApi.Endpoints.Handlers;
using OutlineServerApi.Endpoints;
using System.Security.Claims;
using Microsoft.Extensions.Http;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

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
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddDbContext<OutlineContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMediatR(c => 
    c.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ErrorDescriber>();
builder.Services.AddScoped<IAccessUriProvider, AccessUriProvider>();
builder.Services.AddScoped<IOutlineApi, OutlineApi>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpClient()
    .Configure<HttpClientFactoryOptions>(options =>
    {
        options.HttpMessageHandlerBuilderActions.Add(builder =>
        {
            builder.PrimaryHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, crt, chn, e) => true
            };
        });
    });
builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");


var app = builder.Build();

app.UseExceptionHandler(o => { });
//app.UseRouting();

//app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

var group = app.MapGroup("api/v1");
group.MapKeysEndpoints();
group.MapServersEndpoints();
group.MapCounriesEndpoints();

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

app.Run();
