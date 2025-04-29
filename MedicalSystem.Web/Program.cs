
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MudBlazor.Services;
using System.Net.Http.Headers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

// Add these services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
    options.ClientSecret = builder.Configuration["Auth:ClientSecret"];
    options.ResponseType = "code";

    // Request offline_access for refresh tokens
    options.Scope.Add("offline_access");

    // Save tokens for later use
    options.SaveTokens = true;

    // Automatic token renewal
    options.Events = new OpenIdConnectEvents
    {
        OnTokenResponseReceived = context =>
        {
            // Store tokens in cookie
            return Task.CompletedTask;
        }
    };
});

// Add HTTP client with token propagation
builder.Services.AddHttpClient("ApiClient", (sp, client) =>
{
    var accessToken = sp.GetService<IHttpContextAccessor>()?
                       .HttpContext?
                       .GetTokenAsync("access_token")
                       .GetAwaiter()
                       .GetResult();

    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", accessToken);
});

// Add token store (for web to manage tokens)
builder.Services.AddScoped<ITokenStore, SqlTokenStore>();

// Configure OpenID Connect to use token store
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
    .AddOpenIdConnect(options =>
    {
        options.Events = new OpenIdConnectEvents
        {
            OnTokenResponseReceived = async context =>
            {
                var tokenStore = context.HttpContext.RequestServices.GetRequiredService<ITokenStore>();
                var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    await tokenStore.StoreTokensAsync(
                        userId,
                        context.TokenEndpointResponse.AccessToken,
                        context.TokenEndpointResponse.RefreshToken
                        //,
                        //DateTime.UtcNow.AddSeconds(context.TokenEndpointResponse.ExpiresIn)
                        );
                }
            }
        };
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
// Add this before app.UseAuthentication()
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var expiresAt = context.User.FindFirst("expires_at")?.Value;
        if (expiresAt != null && DateTime.Parse(expiresAt) < DateTime.UtcNow.AddMinutes(5))
        {
            var tokenClient = new TokenClient(
                new HttpClient(),
                new TokenClientOptions
                {
                    Address = $"{builder.Configuration["Auth:Authority"]}/connect/token",
                    ClientId = builder.Configuration["Auth:ClientId"],
                    ClientSecret = builder.Configuration["Auth:ClientSecret"]
                });

            var refreshToken = await context.GetTokenAsync("refresh_token");
            var response = await tokenClient.RequestRefreshTokenAsync(refreshToken);

            if (!response.IsError)
            {
                var authInfo = await context.AuthenticateAsync();
                authInfo.Properties.StoreTokens(new[]
                {
                    new AuthenticationToken { Name = "access_token", Value = response.AccessToken },
                    new AuthenticationToken { Name = "refresh_token", Value = response.RefreshToken },
                    new AuthenticationToken { Name = "expires_at", Value = DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToString("o") }
                });
                await context.SignInAsync(authInfo.Principal, authInfo.Properties);
            }
        }
    }
    await next();
});// Add this before app.UseAuthentication()
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var expiresAt = context.User.FindFirst("expires_at")?.Value;
        if (expiresAt != null && DateTime.Parse(expiresAt) < DateTime.UtcNow.AddMinutes(5))
        {
            var tokenClient = new TokenClient(
                new HttpClient(),
                new TokenClientOptions
                {
                    Address = $"{builder.Configuration["Auth:Authority"]}/connect/token",
                    ClientId = builder.Configuration["Auth:ClientId"],
                    ClientSecret = builder.Configuration["Auth:ClientSecret"]
                });

            var refreshToken = await context.GetTokenAsync("refresh_token");
            var response = await tokenClient.RequestRefreshTokenAsync(refreshToken);

            if (!response.IsError)
            {
                var authInfo = await context.AuthenticateAsync();
                authInfo.Properties.StoreTokens(new[]
                {
                    new AuthenticationToken { Name = "access_token", Value = response.AccessToken },
                    new AuthenticationToken { Name = "refresh_token", Value = response.RefreshToken },
                    new AuthenticationToken { Name = "expires_at", Value = DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToString("o") }
                });
                await context.SignInAsync(authInfo.Principal, authInfo.Properties);
            }
        }
    }
    await next();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
