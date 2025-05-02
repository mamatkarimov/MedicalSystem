using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Identity
{
    public class EmailConfirmationTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser>
        where TUser : class
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false); // Only for email confirmation
        }

        public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var token = await manager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            return await manager.VerifyUserTokenAsync(
                user,
                Options.Tokens.EmailConfirmationTokenProvider,
                purpose,
                token);
        }
    }
}