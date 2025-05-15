using System;

namespace AuthService.Core.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        // Token properties
        public string Token { get; set; }
        public string JwtId { get; set; }  // Maps to the JTI in the JWT

        // Usage tracking
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        // Date tracking
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ExpiryDate { get; set; }
        public DateTimeOffset? UsedDate { get; set; }

        // Device/context information
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string DeviceIdentifier { get; set; }

        // Navigation property
        public virtual User User { get; set; }

        // Helper methods
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired && !IsUsed;

        // Constructor for creating new refresh tokens
        public RefreshToken(
            string token,
            string jwtId,
            DateTimeOffset expiryDate,
            Guid userId,
            string ipAddress,
            string userAgent,
            string deviceIdentifier)
        {
            Token = token;
            JwtId = jwtId;
            ExpiryDate = expiryDate;
            UserId = userId;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            DeviceIdentifier = deviceIdentifier;
        }
    }
}