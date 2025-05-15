using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Core.Entities
{   
    public class User : IdentityUser<Guid>
    {
        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Account Status
        public bool IsActive { get; set; } = true;
        public DateTimeOffset? LastLoginDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }

        // Audit Fields
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        // Soft Delete
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<UserDevice> Devices { get; set; } = new List<UserDevice>();
        public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
        public virtual ICollection<ApplicationRole> UserRoles { get; set; } = new List<ApplicationRole>();

        // Computed Properties
        public string FullName => $"{FirstName} {LastName}";

        // Constructor
        public User() : base()
        {
            EmailConfirmed = false;
            PhoneNumberConfirmed = false;
            TwoFactorEnabled = false;
            LockoutEnabled = true;
        }
    }


    

    public class UserDevice
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Platform { get; set; }
        public string OsVersion { get; set; }
        public string PushNotificationToken { get; set; }
        public DateTimeOffset LastAccessed { get; set; }
        public string IpAddress { get; set; }
        public bool IsTrusted { get; set; }

        public virtual User User { get; set; }
    }

    public class Profile
    {
        [Key]
        public string UserId { get; set; }  // Foreign key to IdentityUser
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Other custom fields...

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}