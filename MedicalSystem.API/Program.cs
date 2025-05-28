using MedicalSystem.Infrastructure;
using MedicalSystem.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

// Authentication setup
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5074"; // Change as needed
        options.RequireHttpsMetadata = false;
        options.Audience = "medical_api";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key // ✅ MUST MATCH the key used in AuthEndpoints
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Patient", policy =>
    {
        policy.RequireRole("Patient");
    });

    // You may add other roles too for later use
    options.AddPolicy("Doctor", policy =>
    {
        policy.RequireRole("Doctor");
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // for attribute routing if needed
// Temporary minimal endpoint to test
app.MapGet("/", () => "MedicalSystem API is running").AllowAnonymous();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var roles = new[] {   "Admin",
"Doctor",
"Nurse",
"Reception",
"Cashier",
"Laboratory",
"ChefDoctor",
"Patient" };

    foreach (var roleName in roles)
    {
        if (!await db.Roles.AnyAsync(r => r.Name == roleName))
        {
            db.Roles.Add(new Role { Name = roleName });
        }
    }

    await db.SaveChangesAsync();
}

app.Run();
