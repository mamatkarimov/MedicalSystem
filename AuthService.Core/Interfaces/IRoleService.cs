using AuthService.Core.Entities;
using AuthService.Shared.DTOs;
using AuthService.Shared.DTOs.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IRoleService
    {
        // Role CRUD Operations
        Task<ApplicationRole> CreateRoleAsync(CreateRoleRequest request);
        //Task<ApplicationRole> UpdateRoleAsync(string roleId, UpdateRoleRequest request);
        Task DeleteRoleAsync(string roleId);
        Task<ApplicationRole> GetRoleByIdAsync(string roleId);
        Task<ApplicationRole> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        Task<PaginatedResponse<ApplicationRole>> GetRolesPagedAsync(PaginationParameters pagination);

        // Role-Permission Management
        Task AssignPermissionsToRoleAsync(string roleId, IEnumerable<string> permissionIds);
        Task RemovePermissionsFromRoleAsync(string roleId, IEnumerable<string> permissionIds);
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId);
        Task<bool> RoleHasPermissionAsync(string roleId, string permissionName);

        // User-Role Management
        Task AssignRolesToUserAsync(string userId, IEnumerable<string> roleIds);
        Task RemoveRolesFromUserAsync(string userId, IEnumerable<string> roleIds);
        Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(string userId);
        Task<bool> UserHasRoleAsync(string userId, string roleName);
        Task<bool> UserHasAnyRoleAsync(string userId, IEnumerable<string> roleNames);
        Task<bool> UserHasAllRolesAsync(string userId, IEnumerable<string> roleNames);

        // Status Checks
        Task<bool> RoleExistsAsync(string roleId);
        Task<bool> RoleNameExistsAsync(string roleName);
    }

}