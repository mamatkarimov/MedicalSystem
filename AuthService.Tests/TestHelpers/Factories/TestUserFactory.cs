using AuthService.Core.Entities;

namespace AuthService.Tests.TestHelpers.Factories
{
    public static class TestUserFactory
    {
        public static User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                IsActive = true,
                PasswordHash = "hashed_password"
            };
        }

        public static User CreateLockedUser()
        {
            var user = CreateValidUser();
            user.LockoutEnd = DateTimeOffset.UtcNow.AddHours(1);
            return user;
        }
    }
}