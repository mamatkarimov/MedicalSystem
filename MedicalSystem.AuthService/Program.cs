using MedicalSystem.AuthService;
using MedicalSystem.AuthService.Models;
using MedicalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure MSSQL Storage
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    .AddInMemoryClients(Config.GetClients(builder.Configuration))
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();