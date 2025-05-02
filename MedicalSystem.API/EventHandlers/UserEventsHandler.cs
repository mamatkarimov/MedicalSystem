using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Events;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace MedicalSystem.API.EventHandlers
{
    // MedicalSystem.Api/EventHandlers/UserEventsHandler.cs
    public class UserEventsHandler :
        IEventHandler<UserCreatedEvent>,
    IEventHandler<UserUpdatedEvent>,
        IEventHandler<UserDeletedEvent>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserEventsHandler> _logger;

        public UserEventsHandler(ApplicationDbContext dbContext, ILogger<UserEventsHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserCreatedEvent @event)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                IdentityId = @event.IdentityId,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Created domain user for identity {IdentityId}", @event.IdentityId);
        }

        public async Task Handle(UserUpdatedEvent @event)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.IdentityId == @event.IdentityId);

            if (user != null)
            {
                if (!string.IsNullOrEmpty(@event.NewFirstName))
                    user.FirstName = @event.NewFirstName;

                if (!string.IsNullOrEmpty(@event.NewLastName))
                    user.LastName = @event.NewLastName;

                user.UpdatedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Handle(UserDeletedEvent @event)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.IdentityId == @event.IdentityId);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
