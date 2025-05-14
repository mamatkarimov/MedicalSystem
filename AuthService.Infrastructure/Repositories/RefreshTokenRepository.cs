using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<RefreshToken> GetByJwtIdAsync(string jwtId)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.JwtId == jwtId);
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string token, string ipAddress, string reason = null)
        {
            var refreshToken = await GetByTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.IpAddress = ipAddress;
                //refreshToken.ReasonRevoked = reason;
                await UpdateAsync(refreshToken);
            }
        }

        public async Task RevokeAllTokensForUserAsync(Guid userId, string ipAddress, string reason = null)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.IsActive)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.IpAddress = ipAddress;
                //token.ReasonRevoked = reason;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            return await _context.RefreshTokens
                .AnyAsync(rt => rt.Token == token && !rt.IsRevoked && !rt.IsUsed && rt.ExpiryDate > DateTime.UtcNow);
        }

        public Task DeleteAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsForUserAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}