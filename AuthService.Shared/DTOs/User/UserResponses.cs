using AuthService.Shared.DTOs;
using System;
using System.Collections.Generic;

namespace AuthService.Core.Models.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class UserDetailResponse : UserResponse
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }

    public class UserListResponse : PaginatedResponse<UserResponse>
    {
        // Inherits all pagination properties
    }

    public class ProfileResponse : BaseResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }

    public class PasswordChangeResponse : BaseResponse
    {
        public DateTime? PasswordExpiresOn { get; set; }
    }
}