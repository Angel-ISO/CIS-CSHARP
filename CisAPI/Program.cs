using System.Reflection;
using AspNetCoreRateLimit;
using CisAPI.Extensions;
using CisAPI.Helpers;
using iText.Kernel.XMP.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cis API", Version = "v1", Description="Api que se autentica con JWT proveniente del modulo de usuarios en spring", Contact = new OpenApiContact
            {
                Name = "Soporte",
                Email = "angel.ortega@jala.university, Catriel.Pereira@jala.university, Steven.Balaguera@jala.university, Enrique.Tarqui@jala.university, Fernanda.Escobar@jala.university, Sebastian.Bartolo@jala.university"
            } });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,  
        Scheme = "bearer",               
        BearerFormat = "JWT"             
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

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();


builder.Services.AddAuthentication();


builder.Services.AddDbContext<CisContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("ConexDb");

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Connection string 'ConexDb' is not configured in appsettings.json.");

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAuthorization();
builder.Services.AddAplicacionServices();
builder.Services.ConfigureCors();
builder.Services.ConfigurationRatelimiting();
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.AddJwt(builder.Configuration);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseIpRateLimiting();
app.MapControllers();
app.Run();

