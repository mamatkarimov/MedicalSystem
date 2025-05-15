using AuthService.API.Extensions;
using AuthService.API.Middleware;
using AuthService.Infrastructure.Data;
using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        try
        {
            

            // Add services to the container
            builder.Services.ConfigureDatabaseContext(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJwtAuthentication(builder.Configuration);
            builder.Services.ConfigureApplicationServices();
            builder.Services.ConfigureCors(builder.Configuration);
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureHealthChecks(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = ConfigurationSection["Jwt:Issuer"],
            //        ValidAudience = app.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //    };
            //});

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireAdminRole",
            //        policy => policy.RequireRole("Admin"));
            //});


            var app = builder.Build();

            // Initialize database
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                await initializer.InitializeAsync();
                await initializer.SeedAsync();
            }

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");
                    c.RoutePrefix = "api-docs";
                });
            }


            app.UseHttpsRedirection();
            app.UseRouting();

            // Global CORS policy
            app.UseCors("CorsPolicy");

            // Custom middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Health checks
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(e => new
                        {
                            Component = e.Key,
                            Status = e.Value.Status.ToString(),
                            e.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message  + "Application terminated unexpectedly");
        }
        finally
        {
            
        }
    }
}

