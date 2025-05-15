using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Infrastructure.Services;
using MedicalSystem.API.EventHandlers;
using MedicalSystem.Domain.Events;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.API.BackgroundServices;
using MedicalSystem.API.Infrastructure;

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
builder.Services.AddScoped<IEventHandler<UserCreatedEvent>, UserEventsHandler>();
builder.Services.AddScoped<IEventHandler<UserUpdatedEvent>, UserEventsHandler>();
builder.Services.AddScoped<IEventHandler<UserDeletedEvent>, UserEventsHandler>();

// Add the dispatcher and consumer
builder.Services.AddSingleton<EventDispatcher>();
builder.Services.AddHostedService<RabbitMQEventConsumer>();

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