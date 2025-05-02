namespace AuthService.Core.Interfaces
{
    public class LoginRecordDto
    {
        public DateTime LoginDate { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string OperatingSystem { get; set; }
        public string Browser { get; set; }
        public bool WasSuccessful { get; set; }
        public string FailureReason { get; set; }
        public bool IsCurrentSession { get; set; }

        // For MFA scenarios
        public bool UsedMfa { get; set; }
        public string MfaType { get; set; } // "SMS", "Authenticator", "Email", etc.
    }
}