using System;

namespace AuthService.Core.Entities
{
    public class Device
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string Platform { get; set; }
        public string OsVersion { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string IpAddress { get; set; }
        public string PushNotificationToken { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrusted { get; set; }

        // For geo-location tracking (optional)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // For security purposes
        public string LastLoginLocation { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}