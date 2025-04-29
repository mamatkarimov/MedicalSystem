using MedicalSystem.AuthService;
using MedicalSystem.AuthService.Models;
using MedicalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

//// Add Identity Server
//builder.Services.AddIdentityServer()
//    .AddDeveloperSigningCredential()
//    .AddInMemoryIdentityResources(Config.IdentityResources)
//    .AddInMemoryApiScopes(Config.ApiScopes)
//    .AddInMemoryClients(Config.GetClients(builder.Configuration))
//    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryClients(Config.GetClients(builder.Configuration));

// Configure MSSQL Storage
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseIdentityServer();
app.Run();