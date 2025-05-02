using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.Auth
{
    /// <summary>
    /// Request model for user login
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// IP address of the client device (automatically captured)
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// User agent/browser information (automatically captured)
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Unique device identifier for multi-device tracking
        /// </summary>
        public string DeviceId { get; set; }
    }

   
}