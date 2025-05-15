using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.User
{
    public class RegisterDeviceRequest
    {
        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string DeviceName { get; set; }

        public string Platform { get; set; }
        public string OsVersion { get; set; }
        public string PushNotificationToken { get; set; }
        public string IpAddress { get; set; }
    }

    

    public class TerminateSessionRequest
    {
        [Required]
        public string SessionId { get; set; }
        public string TerminatedBy { get; set; }
    }
}