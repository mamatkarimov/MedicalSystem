using AuthService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IRoleRepository
    {
        Task<ApplicationRole> GetByIdAsync(Guid id);
        Task<ApplicationRole> GetByNameAsync(string name);
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task AddAsync(ApplicationRole role);
        Task UpdateAsync(ApplicationRole role);
        Task DeleteAsync(Guid id);
        Task AddPermissionAsync(Guid roleId, Guid permissionId);
        Task RemovePermissionAsync(Guid roleId, Guid permissionId);
    }
}