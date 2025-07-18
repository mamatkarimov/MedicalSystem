using MedicalSystem.API.Extensions;
using MedicalSystem.API.Services;
using MedicalSystem.Application;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

//var jwtKey = builder.Configuration["Jwt:Key"];
//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));


//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "http://localhost:5074"; // Change as needed
//options.RequireHttpsMetadata = false;
//options.Audience = "medical_api";
//options.TokenValidationParameters = new TokenValidationParameters
//{
//    ValidateIssuer = true,
//    ValidateAudience = true,
//    ValidateLifetime = true,
//    ValidateIssuerSigningKey = true,

//    ValidIssuer = builder.Configuration["Jwt:Issuer"],
//    ValidAudience = builder.Configuration["Jwt:Audience"],
//    IssuerSigningKey = key
//};
//    });


builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();






builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuth();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

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
