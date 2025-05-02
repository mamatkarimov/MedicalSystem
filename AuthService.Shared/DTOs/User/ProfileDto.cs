namespace AuthService.Core.Interfaces
{
    public class ProfileDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public bool IsActive { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Additional profile information
        public Dictionary<string, string> Claims { get; set; } = new();
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}