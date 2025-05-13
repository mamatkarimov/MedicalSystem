using AuthService.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace AuthService.Infrastructure.Identity
{
    public class CustomPasswordHasher : PasswordHasher<User>
    {
        private readonly PasswordHasherOptions _options;

        public CustomPasswordHasher(IOptions<PasswordHasherOptions> optionsAccessor = null)
            : base(optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? new PasswordHasherOptions();
        }

        public override string HashPassword(User user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return base.HashPassword(user, password + user.SecurityStamp);
        }

        public override PasswordVerificationResult VerifyHashedPassword(
            User user, string hashedPassword, string providedPassword)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return base.VerifyHashedPassword(
                user,
                hashedPassword,
                providedPassword + user.SecurityStamp);
        }
    }
}