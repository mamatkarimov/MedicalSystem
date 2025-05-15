using AuthService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IDeviceRepository
    {
        Task<Device> GetByIdAsync(string deviceId);
        Task<IEnumerable<Device>> GetByUserIdAsync(string userId);
        Task<Device> GetByDeviceIdAsync(Guid userId, string deviceId);
        Task AddAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(string deviceId);
        Task<bool> ExistsAsync(string deviceId);
    }
}