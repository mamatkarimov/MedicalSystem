using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Domain.Events;

var builder = WebApplication.CreateBuilder(args);

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = "medicalapi";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            NameClaimType = ClaimTypes.Name, // Map claims properly
            RoleClaimType = ClaimTypes.Role
        };
    });

// Add MSSQL Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// In API Program.cs

// Add the dispatcher and consumer

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();