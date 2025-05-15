using AuthService.Shared.DTOs.User;
using System;

namespace AuthService.Shared.DTOs.Auth
{
    public class AuthenticationResponse : BaseResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
        public UserResponse User { get; set; }
    }

    public class TokenValidationResponse : BaseResponse
    {
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }
        public IEnumerable<ClaimResponse> Claims { get; set; }
    }

    public class ClaimResponse
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class RevocationResponse : BaseResponse
    {
        public int TokensRevoked { get; set; }
    }
}