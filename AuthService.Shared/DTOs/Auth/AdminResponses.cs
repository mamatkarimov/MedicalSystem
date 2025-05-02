using System;

namespace AuthService.Shared.DTOs.Auth
{
    public class SystemStatusResponse : BaseResponse
    {
        public DateTime ServerTime { get; set; }
        public string Version { get; set; }
        public DatabaseStatus Database { get; set; }
        public CacheStatus Cache { get; set; }
    }

    public class DatabaseStatus
    {
        public string Status { get; set; }
        public string Provider { get; set; }
        public DateTime LastMigration { get; set; }
    }

    public class CacheStatus
    {
        public string Status { get; set; }
        public int ItemsCached { get; set; }
        public string Provider { get; set; }
    }

    public class AuditLogResponse
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string IpAddress { get; set; }
    }
}