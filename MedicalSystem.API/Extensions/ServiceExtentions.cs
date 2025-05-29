using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace MedicalSystem.API.Extensions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                var securityDef = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT", //JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below."
                };
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDef);
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },new String[]{ }
                    }
                });
            });
            // Register application services here
            // Example: services.AddScoped<IMyService, MyService>();
            return services;
        }
    }
}
