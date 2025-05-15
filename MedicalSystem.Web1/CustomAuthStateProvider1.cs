using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
internal class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAuthStateProvider> _logger;

    public CustomAuthStateProvider(
        IHttpContextAccessor httpContextAccessor,
        ILogger<CustomAuthStateProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                // You can add additional claims processing here if needed
                _logger.LogInformation("User authenticated: {UserName}", user.Identity.Name);
            }

            return new AuthenticationState(user ?? new ClaimsPrincipal());

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting authentication state");
            return new AuthenticationState(new ClaimsPrincipal());
        }
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}