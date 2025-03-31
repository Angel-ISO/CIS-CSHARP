using System.Text;
using Application.UnitOfWork;
using AspNetCoreRateLimit;
using CisAPI.Helpers;
using CisAPI.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;

namespace CisAPI.Extensions;
    public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()    //WithOrigins("https://domain.com")
                    .AllowAnyMethod()       //WithMethods("GET","POST)
                    .AllowAnyHeader());     //WithHeaders("accept","content-type")
        });
    public static void AddAplicacionServices(this IServiceCollection services)
    {
       // services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        //services.AddScoped<IUserService, UserService>();
        services.AddAutoMapper(typeof(ApplicationServiceExtensions));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<UserContextService>();
    }


   public static void ConfigurationRatelimiting(this IServiceCollection services){
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();
        services.Configure<IpRateLimitOptions>(opt =>{
            opt.EnableEndpointRateLimiting = true;
            opt.StackBlockedRequests = false;
            opt.HttpStatusCode = 429;
            opt.RealIpHeader = "X-Real-IP";
            opt.GeneralRules = new(){
                new(){
                   Endpoint = "*",
                   Period = "10s",
                   Limit = 15
                }
            };
        });
    }

   



public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
{
    services.Configure<JWT>(configuration.GetSection("JWT"));

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            
            ValidIssuer = configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Key"])
            ),
            
            NameClaimType = "sub",      
            RoleClaimType = "authorities" 
        };
        
        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                foreach (var claim in context.Principal.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
                return Task.CompletedTask;
            }
        };
    });
}




}
