using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace CisAPI.Extensions;
public static class SwaggerServiceExtensions
{
     public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Cis API",
                Version = "v1",
                Description = "Api que se autentica con JWT proveniente del modulo de usuarios en Spring",
                Contact = new OpenApiContact
                {
                    Name = "Soporte",
                    Email = "angel.ortega@jala.university, Catriel.Pereira@jala.university, Steven.Balaguera@jala.university, Enrique.Tarqui@jala.university, Fernanda.Escobar@jala.university, Sebastian.Bartolo@jala.university"
                }
            });

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
    }
}
