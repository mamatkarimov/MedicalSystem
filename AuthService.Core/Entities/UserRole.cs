using Microsoft.AspNetCore.Identity;
using System;

namespace AuthService.Core.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTimeOffset AssignedDate { get; set; } = DateTimeOffset.UtcNow;
        public string AssignedBy { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }

        public virtual User User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

   
}