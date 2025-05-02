using MedicalSystem.AuthService.Models;
using MedicalSystem.Domain.Events;
using MedicalSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MedicalSystem.AuthService.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IEventPublisher eventPublisher)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                // Publish domain event after successful creation
                await _eventPublisher.Publish(new UserCreatedEvent(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName));
            }

            return result;
        }
    }
}
