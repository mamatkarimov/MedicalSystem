using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace MedicalSystem.Staff.Services
{
    //public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    //{
    //    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    //    private string _token;
    //    private string _username;

    //    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        if (string.IsNullOrEmpty(_token))
    //            return Task.FromResult(new AuthenticationState(_anonymous));

    //        var identity = new ClaimsIdentity(new[]
    //        {
    //            new Claim(ClaimTypes.Name, _username)
    //        }, "apiauth");

    //        var user = new ClaimsPrincipal(identity);
    //        return Task.FromResult(new AuthenticationState(user));
    //    }

    //    public void MarkUserAsAuthenticated(string username, string token)
    //    {
    //        _username = username;
    //        _token = token;

    //        var identity = new ClaimsIdentity(new[]
    //        {
    //            new Claim(ClaimTypes.Name, username)
    //        }, "apiauth");

    //        var user = new ClaimsPrincipal(identity);
    //        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    //    }

    //    public void MarkUserAsLoggedOut()
    //    {
    //        _username = null;
    //        _token = null;
    //        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    //    }


    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public ApiAuthenticationStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            var username = await _js.InvokeAsync<string>("localStorage.getItem", "username");
            var rolesJson = await _js.InvokeAsync<string>("localStorage.getItem", "userRoles");

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(username))
                return new AuthenticationState(_anonymous);

            var roles = string.IsNullOrWhiteSpace(rolesJson)
                ? Array.Empty<string>()
                : JsonSerializer.Deserialize<string[]>(rolesJson);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r))), "apiauth");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task MarkUserAsAuthenticated(string username, string token, string[] roles)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            await _js.InvokeVoidAsync("localStorage.setItem", "username", username);
            await _js.InvokeVoidAsync("localStorage.setItem", "userRoles", JsonSerializer.Serialize(roles));

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r))), "apiauth");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "username");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userRoles");

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}





