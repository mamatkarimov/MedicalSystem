using MedicalSystem.Staff.Auth;
using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient();

builder.Services.AddScoped<SecureStorageService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped<SecureStorageService>();
//builder.Services.AddScoped<JwtAuthenticationStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
//    sp.GetRequiredService<JwtAuthenticationStateProvider>());
//builder.Services.AddAuthorizationCore();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
