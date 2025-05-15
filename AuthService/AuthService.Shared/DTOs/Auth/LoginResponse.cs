using AuthService.Shared.DTOs.User;

namespace AuthService.Shared.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
        public UserDto User { get; set; }
    }
}