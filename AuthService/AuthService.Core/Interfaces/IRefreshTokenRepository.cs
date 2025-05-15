using AuthService.Core.Entities;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
        Task DeleteAsync(string token);
        Task DeleteAllForUserAsync(string userId);
        Task<bool> ExistsForUserAsync(string userId, string token);
    }
}