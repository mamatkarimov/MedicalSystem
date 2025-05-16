using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MedicalSystem.Staff;
using MedicalSystem.Staff.Auth;
using static MedicalSystem.Staff.Auth.CustomAuthStateProvider;
using Microsoft.JSInterop;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// ✅ Blazored Local Storage
builder.Services.AddBlazoredLocalStorage();

// ✅ Authorization Core
builder.Services.AddAuthorizationCore();

// ✅ Register CustomAuthStateProvider
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>()
);

builder.Services.AddScoped<CustomAuthStateProvider>(sp =>
    new CustomAuthStateProvider(
        sp.GetRequiredService<ILocalStorageService>(),
        sp.GetRequiredService<IJSRuntime>()
    ));

// ✅ Custom message handler to inject token into API requests
builder.Services.AddScoped<CustomAuthMessageHandler>();

// ✅ HttpClient for talking to your API
builder.Services.AddScoped(sp => new HttpClient(sp.GetRequiredService<CustomAuthMessageHandler>())
{
    BaseAddress = new Uri("https://localhost:5074") // ⬅️ API base address
});

var app = builder.Build();

// 🔧 Configure the HTTP request pipeline.
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
