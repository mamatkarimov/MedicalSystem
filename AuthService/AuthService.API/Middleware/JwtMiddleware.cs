using AuthService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly ITokenService _tokenService;

        public JwtMiddleware(
            RequestDelegate next,
            ILogger<JwtMiddleware> logger,
            ITokenService tokenService)
        {
            _next = next;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var token = ExtractToken(context.Request);

                if (!string.IsNullOrEmpty(token))
                {
                    var (isValid, principal) = _tokenService.ValidateToken(token);

                    if (isValid && principal != null)
                    {
                        context.User = principal;

                        // Check if token is about to expire (within 5 minutes)
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                        if (jwtToken.ValidTo.AddMinutes(-5) <= DateTime.UtcNow)
                        {
                            // Add header to notify client to refresh token
                            context.Response.Headers.Add("X-Token-Expiring", "true");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing JWT token");
                // Don't throw here, let the actual authentication handle the failure
            }

            await _next(context);
        }

        private string ExtractToken(HttpRequest request)
        {
            // Try to get token from Authorization header first
            if (request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                var header = authHeader.FirstOrDefault();
                if (!string.IsNullOrEmpty(header) && header.StartsWith("Bearer "))
                {
                    return header.Substring("Bearer ".Length).Trim();
                }
            }

            // Fallback to getting token from cookie
            if (request.Cookies.TryGetValue("access_token", out string cookieToken))
            {
                return cookieToken;
            }

            // Try to get token from query string (for WebSocket connections)
            if (request.Query.TryGetValue("access_token", out var queryToken))
            {
                return queryToken.FirstOrDefault();
            }

            return null;
        }

        // Helper method to check if request is for an excluded path
        private bool IsExcludedPath(HttpContext context)
        {
            var path = context.Request.Path.Value;
            return path.StartsWith("/api/auth/login", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/api/auth/register", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/health", StringComparison.OrdinalIgnoreCase);
        }
    }
}