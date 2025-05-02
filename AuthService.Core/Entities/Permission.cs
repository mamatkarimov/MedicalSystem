using System;
using System.Collections.Generic;

namespace AuthService.Core.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}