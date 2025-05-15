using AuthService.Shared.DTOs.User;
using System.Collections.Generic;

namespace AuthService.Shared.DTOs.Roles
{
   

    public class RoleDetailResponse : RoleResponse
    {
        public IEnumerable<string> Permissions { get; set; }
        public int UserCount { get; set; }
    }

    public class RoleAssignmentResponse : BaseResponse
    {
        public int RolesAssigned { get; set; }
        public IEnumerable<string> CurrentRoles { get; set; }
    }

    public class PermissionResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

    public class PermissionAssignmentResponse : BaseResponse
    {
        public int PermissionsGranted { get; set; }
        public IEnumerable<string> CurrentPermissions { get; set; }
    }

    public class RoleAssignmentResult : BaseResponse
    {
        public int RolesAssigned { get; set; }
        public UserRolesResult CurrentRoles { get; set; }
    }

   

   

    

  

   
}