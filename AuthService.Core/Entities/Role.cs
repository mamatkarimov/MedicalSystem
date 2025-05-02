using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AuthService.Core.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        // Extended Properties
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; } = DateTimeOffset.UtcNow;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        // Role Hierarchy
        public Guid? ParentRoleId { get; set; }
        public virtual ApplicationRole ParentRole { get; set; }
        public virtual ICollection<ApplicationRole> ChildRoles { get; set; } = new List<ApplicationRole>();

        // Permission Management
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Soft Delete
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        // Constructor
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName, string description = null) : base(roleName)
        {
            Description = description;
        }
    }

    public class RolePermission
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Permission { get; set; }  // e.g., "users.read", "users.write"
        public DateTimeOffset GrantedDate { get; set; } = DateTimeOffset.UtcNow;
        public string GrantedBy { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }

   
}