using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.User
{
    public class CreateUserRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public string RequestedBy { get; set; }
    }

    public class UpdateUserRequest
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ModifiedBy { get; set; }

        public bool? EmailConfirmed { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
    }

    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateProfilePictureRequest
    {
        [Required]
        public string ImageUrl { get; set; }
    } 

    public class LockAccountRequest
    {
        public DateTimeOffset? LockoutEnd { get; set; }
        public string Reason { get; set; }
        public string LockedBy { get; set; }
    }


}