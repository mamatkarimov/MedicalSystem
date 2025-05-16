using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MedicalSystem.Staff;
using MedicalSystem.Staff.Auth;
using static MedicalSystem.Staff.Auth.CustomAuthStateProvider;
using Microsoft.JSInterop;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>()
);

builder.Services.AddScoped<CustomAuthStateProvider>(sp =>
    new CustomAuthStateProvider(
        sp.GetRequiredService<ILocalStorageService>(),
        sp.GetRequiredService<IJSRuntime>()
    ));

builder.Services.AddScoped<CustomAuthMessageHandler>();

builder.Services.AddScoped(sp => new HttpClient(sp.GetRequiredService<CustomAuthMessageHandler>())
{
    BaseAddress = new Uri("https://localhost:5074") // ⬅️ API base address
});

var app = builder.Build();

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
