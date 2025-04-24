using System.Reflection;
using Application.UnitOfWork;
using AspNetCoreRateLimit;
using CisAPI.Services;
using Domain.Interfaces;
using MongoDB.Driver;
using Persistence;
using Persistence.seeds;

namespace CisAPI.Extensions;
    public static class ApplicationServiceExtensions
    {
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetEntryAssembly());


        // Unit of Work & context-related services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<UserContextService>();

        // DB Context
        string? connectionString = configuration.GetConnectionString("MongoDb");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'MongoDb' is not configured in appsettings.json.");

        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(connectionString));
        services.AddSingleton(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase("Cis")); 

        services.AddScoped<CisContext>();

            

        // Seeds
        services.AddSingleton<TopicSeed>();
        services.AddSingleton<IdeaSeed>();
        services.AddSingleton<VoteSeed>();

        services.AddHostedService<SeedOrchestrator>();
        }
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.EnableEndpointRateLimiting = true;
                opt.StackBlockedRequests = false;
                opt.HttpStatusCode = 429;
                opt.RealIpHeader = "X-Real-IP";
                opt.GeneralRules = new()
                {
                new()
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 15
                }
                };
            });
        }
    
}