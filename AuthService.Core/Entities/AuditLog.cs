using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Core.Entities
{
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string EntityId { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime ActionTime { get; set; }
        public string PerformedBy { get; set; }
        public string IpAddress { get; set; }
    }
}