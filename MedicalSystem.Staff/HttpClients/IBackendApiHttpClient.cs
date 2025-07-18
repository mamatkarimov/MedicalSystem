using MedicalSystem.Staff.Models;

namespace MedicalSystem.Staff.HttpClients;

public interface IBackendApiHttpClient
{
    Task<ApiResponse<string>> RegisterUserAsync(UserRegisterInput model, CancellationToken? cancellationToken = null);
    Task<ApiResponse<AuthResponse>> LoginUserAsync(LoginModel model, CancellationToken? cancellationToken = null);

    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken,
        CancellationToken? cancellationToken = null);
}
