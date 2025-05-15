using AuthService.Shared.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Shared.DTOs.User
{
    // AuthService.Shared/DTOs/User/UserQueryParameters.cs
    public class UserQueryParameters
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public string SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string SortBy { get; set; } = "Email";
        public bool SortDescending { get; set; } = false;
    }

    // AuthService.Shared/DTOs/User/LockUserRequest.cs
    public class LockUserRequest
    {
        public DateTimeOffset? LockoutEnd { get; set; }
        public string LockoutReason { get; set; }
    }

    // AuthService.Shared/DTOs/User/UserResponse.cs
    public class UserResponse : BaseResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string LockoutReason { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    // AuthService.Shared/DTOs/User/UserListResponse.cs
    public class UserListResponse : BaseResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
