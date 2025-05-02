using System;

namespace AuthService.Shared.DTOs.User
{
    public class DeviceResponse
    {
        public Guid Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Platform { get; set; }
        public string OsVersion { get; set; }
        public DateTime LastAccessed { get; set; }
        public string IpAddress { get; set; }
        public bool IsTrusted { get; set; }
    }

    public class SessionResponse
    {
        public string SessionId { get; set; }
        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LastActivity { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class SessionTerminationResponse : BaseResponse
    {
        public int SessionsTerminated { get; set; }
    }
}