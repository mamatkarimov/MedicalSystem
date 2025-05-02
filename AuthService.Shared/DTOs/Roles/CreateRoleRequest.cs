using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Shared.DTOs.Roles
{
    // AuthService.Shared/DTOs/Roles/CreateRoleRequest.cs   

    public class CreateRoleRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string RoleName { get; set; }

        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }

    // AuthService.Shared/DTOs/Roles/RoleAssignmentRequest.cs
    public class RoleAssignmentRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }

    // AuthService.Shared/DTOs/BaseResponse.cs
   

    // AuthService.Shared/DTOs/Roles/RoleResponse.cs
    public class RoleResponse : BaseResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AssignRolesRequest
    {
        [Required]
        public List<string> Roles { get; set; } = new List<string>();
        public string AssignedBy { get; set; }
    }

    public class RemoveRolesRequest
    {
        [Required]
        public List<string> Roles { get; set; } = new List<string>();
        public string RemovedBy { get; set; }
    }

    public class AddPermissionsRequest
    {
        [Required]
        public List<string> Permissions { get; set; } = new List<string>();
        public string GrantedBy { get; set; }
    }

    public class RemovePermissionsRequest
    {
        [Required]
        public List<string> Permissions { get; set; } = new List<string>();
        public string RevokedBy { get; set; }
    }
}
