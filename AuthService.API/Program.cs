using AuthService.API.Extensions;
using AuthService.API.Middleware;
using AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/authservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting AuthService API");

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

    app.UseSerilogRequestLogging();
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
                    Description = e.Value.Description
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
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

//using AuthService.API.Extensions;
//using AuthService.API.Middleware;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Add services to the container
//builder.Services.ConfigureDatabaseContext(builder.Configuration);
//builder.Services.ConfigureIdentity();
//builder.Services.ConfigureJwtAuthentication(builder.Configuration);
//builder.Services.ConfigureApplicationServices();
//builder.Services.ConfigureCors(builder.Configuration);
//builder.Services.ConfigureSwagger();
//builder.Services.ConfigureHealthChecks(builder.Configuration);

//builder.Services.AddControllers();

//// Add JWT authentication scheme
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//        };
//    });

//// Register the middleware
//builder.Services.AddTransient<JwtMiddleware>();


//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

//app.UseMiddleware<JwtMiddleware>();
//// Add error handling middleware early in the pipeline
//app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
