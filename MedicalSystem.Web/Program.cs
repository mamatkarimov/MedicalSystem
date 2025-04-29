
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
})
.AddUserAccessTokenHandler();

builder.Services.AddAccessTokenManagement(options =>
{
    options.Client.Clients.Add("auth", new ClientCredentialsTokenRequest
    {
        Address = $"{builder.Configuration["Auth:Authority"]}/connect/token",
        ClientId = "clinicweb",
        ClientSecret = builder.Configuration["Auth:ClientSecret"]
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "clinicweb.auth";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.ClientId = "clinicweb";
    options.ClientSecret = builder.Configuration["Auth:ClientSecret"];
    options.ResponseType = "code";

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("roles");
    options.Scope.Add("permissions");
    options.Scope.Add("clinicapi");
    options.Scope.Add("offline_access");

    options.ClaimActions.MapJsonKey("role", "role");
    options.ClaimActions.MapJsonKey("permission", "permission");

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
