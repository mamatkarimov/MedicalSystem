using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.Repositories
{
    public interface ITokenStore
    {
        Task StoreTokensAsync(string userId, string accessToken, string refreshToken, DateTime expiresAt);
        Task<AppToken?> GetTokensAsync(string userId);
        Task ClearTokensAsync(string userId);
    }

   
}
