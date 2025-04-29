using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Data.Repositories
{   
    public class SqlTokenStore : ITokenStore
    {
        private readonly ApplicationDbContext _context;

        public SqlTokenStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task StoreTokensAsync(string userId, string accessToken, string refreshToken, DateTime expiresAt)
        {
            var existing = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == userId);

            if (existing != null)
            {
                existing.AccessToken = accessToken;
                existing.RefreshToken = refreshToken;
                existing.ExpiresAt = expiresAt;
            }
            else
            {
                _context.Tokens.Add(new AppToken
                {
                    UserId = userId,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<AppToken?> GetTokensAsync(string userId)
        {
            return await _context.Tokens
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task ClearTokensAsync(string userId)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (token != null)
            {
                _context.Tokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
