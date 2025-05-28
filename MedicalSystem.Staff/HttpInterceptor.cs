using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace MedicalSystem.Staff
{
    public class HttpInterceptor : DelegatingHandler
    {
        private readonly SecureStorageService _secureStorage;
        private readonly NavigationManager _navigation;

        public HttpInterceptor(
            SecureStorageService secureStorage,
            NavigationManager navigation)
        {
            _secureStorage = secureStorage;
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Add token to outgoing requests
            var token = await _secureStorage.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Handle 401 responses
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/logout");
            }

            return response;
        }
    }
}
