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

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.ConfigureRateLimiting();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureSwagger();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.ConfigureCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseIpRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
