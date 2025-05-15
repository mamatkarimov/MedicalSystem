using AuthService.Core.Entities;
using AuthService.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IPermissionRepository
    {
        // Basic CRUD Operations
        Task<Permission> GetByIdAsync(string permissionId);
        Task<Permission> GetByNameAsync(string permissionName);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task CreateAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(string permissionId);

        // Role-Permission Relationships
        Task AssignToRoleAsync(string roleId, string permissionId);
        Task RemoveFromRoleAsync(string roleId, string permissionId);
        Task<bool> RoleHasPermissionAsync(string roleId, string permissionId);
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId);
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(string userId);

        // Bulk Operations
        Task AssignMultipleToRoleAsync(string roleId, IEnumerable<string> permissionIds);
        Task RemoveMultipleFromRoleAsync(string roleId, IEnumerable<string> permissionIds);

        // Query Helpers
        Task<bool> ExistsAsync(string permissionId);
        Task<bool> NameExistsAsync(string permissionName);
        Task<int> CountRolePermissionsAsync(string roleId);

        // Pagination (using your existing PaginationParameters)
        Task<PaginatedResponse<Permission>> GetPagedPermissionsAsync(PaginationParameters pagination);
        Task<PaginatedResponse<Permission>> GetPagedRolePermissionsAsync(string roleId, PaginationParameters pagination);
    }
}