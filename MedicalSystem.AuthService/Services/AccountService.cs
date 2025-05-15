using MedicalSystem.AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace MedicalSystem.AuthService.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(
            UserManager<ApplicationUser> userManager
)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                // Publish domain event after successful creation
              
            }

            return result;
        }
    }
}
