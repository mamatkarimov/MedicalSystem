using MedicalSystem.Staff;
using MedicalSystem.Staff.Auth;
using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Identity.Client.Extensions.Msal;
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

builder.Services.AddHttpClient("AuthHttpClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5074");
}).AddHttpMessageHandler<HttpInterceptor>();

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5074") // adjust as needed
    };

    var storage = sp.GetRequiredService<SecureStorageService>();
    var token = storage.GetTokenAsync().GetAwaiter().GetResult();
    
    if (!string.IsNullOrEmpty(token))
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
