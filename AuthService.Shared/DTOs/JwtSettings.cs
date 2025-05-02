namespace AuthService.Shared.DTOs
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 30;
        public int RefreshTokenExpirationDays { get; set; } = 7;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;

        // For clock skew (time synchronization tolerance)
        public int ClockSkewMinutes { get; set; } = 5;

        // For token refresh sliding expiration
        public bool AllowTokenRefresh { get; set; } = true;
        public int RefreshThresholdMinutes { get; set; } = 5;
       
    }
}
