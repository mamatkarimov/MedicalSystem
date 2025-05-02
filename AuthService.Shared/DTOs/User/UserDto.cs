namespace AuthService.Shared.DTOs.User
{
    /// <summary>
    /// Core user data transfer object
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
