using MedicalSystem.Staff.Auth;
using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient();

builder.Services.AddScoped<SecureStorageService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var accessToken = httpContextAccessor.HttpContext?.Session.GetString("access_token");

    var client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5074") // adjust as needed
    };

    if (!string.IsNullOrEmpty(accessToken))
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    return client;
});

builder.Services.AddSession();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
