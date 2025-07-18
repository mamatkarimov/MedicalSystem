
using MedicalSystem.Services;
using MedicalSystem.Staff.HttpClients;
using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>());
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient<AuthService>(client => client.BaseAddress = new Uri("http://localhost:5074/"));

builder.Services.AddScoped(sp =>
{
    var js = sp.GetRequiredService<IJSRuntime>();
    var handler = new AuthorizationMessageHandler(js);
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http//localhost:5074/") // твой WebAPI URL
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();