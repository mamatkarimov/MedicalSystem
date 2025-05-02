using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.Auth
{
    /// <summary>
    /// Request model for token operations (refresh and revocation)
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// The expired or about-to-expire JWT access token
        /// </summary>
        [Required(ErrorMessage = "Access token is required")]
        public string Token { get; set; }

        /// <summary>
        /// Valid refresh token associated with the user session
        /// </summary>
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Client IP address (auto-populated by server)
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Device identifier for multi-device tracking (optional)
        /// </summary>
        public string DeviceId { get; set; }
    }    
}