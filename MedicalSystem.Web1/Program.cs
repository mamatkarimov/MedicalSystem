using MedicalSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add auth services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
})
.AddOpenIdConnect("oidc", options =>
{
    // These are the default values - no need to set unless changing them
    options.CallbackPath = "/signin-oidc";
    options.SignedOutCallbackPath = "/signout-callback-oidc";
    options.SignedOutRedirectUri = "/signout-callback-oidc";
    options.Authority = builder.Configuration["Auth:Authority"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
    options.ClientSecret = builder.Configuration["Auth:ClientSecret"];
    options.ResponseType = "code";

    options.UsePkce = true;
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };

    // Scopes
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("medicalapi"); // Match your API scope
    options.Scope.Add("offline_access");

    // Events for better control
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProvider = context =>
        {
            // Add custom parameters if needed
            return Task.CompletedTask;
        },
        OnTokenResponseReceived = context =>
        {
            // Handle token response
            return Task.CompletedTask;
        }
    };
});

// Add HTTP client with token propagation
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
})
.AddHttpMessageHandler<BearerTokenHandler>();

// Add auth state provider (for Blazor)
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddHttpContextAccessor();

// Add controllers and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.Run();