using AuthService.Core.Entities;
using AuthService.Shared.DTOs;
using AuthService.Shared.DTOs.Auth;
using AuthService.Shared.DTOs.Roles;
using AuthService.Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IUserService
    {
        // User CRUD Operations
        Task<UserResult> CreateUserAsync(CreateUserRequest request);
        Task<UserResult> GetUserByIdAsync(Guid userId);
        Task<UserResult> GetUserByEmailAsync(string email);
        Task<UserListResult> GetAllUsersAsync(UserQueryParameters queryParameters);
        Task<UserResult> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task<DeleteUserResult> DeleteUserAsync(Guid userId);

        // Profile Management
        Task<ProfileResult> GetUserProfileAsync(Guid userId);
        Task<ProfileResult> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
        Task<ProfileResult> UpdateProfilePictureAsync(Guid userId, UpdateProfilePictureRequest request);

        // Account Status
        Task<AccountStatusResult> LockUserAccountAsync(Guid userId, LockAccountRequest request);
        Task<AccountStatusResult> UnlockUserAccountAsync(Guid userId);
        Task<AccountStatusResult> DisableUserAccountAsync(Guid userId);
        Task<AccountStatusResult> EnableUserAccountAsync(Guid userId);

        // Role Management
        Task<RoleAssignmentResult> AssignRolesToUserAsync(Guid userId, AssignRolesRequest request);
        Task<RoleAssignmentResult> RemoveRolesFromUserAsync(Guid userId, RemoveRolesRequest request);
        Task<UserRolesResult> GetUserRolesAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string roleName);

        // Permission Management
        Task<PermissionResult> AddPermissionsToUserAsync(Guid userId, AddPermissionsRequest request);
        Task<PermissionResult> RemovePermissionsFromUserAsync(Guid userId, RemovePermissionsRequest request);
        Task<UserPermissionsResult> GetUserPermissionsAsync(Guid userId);
        Task<bool> HasPermissionAsync(Guid userId, string permission);

        // Device Management
        Task<DeviceResult> RegisterUserDeviceAsync(Guid userId, RegisterDeviceRequest request);
        Task<DeviceResult> RemoveUserDeviceAsync(Guid userId, Guid deviceId);
        Task<DeviceListResult> GetUserDevicesAsync(Guid userId);

        // Audit & Security
        Task<LoginAuditResult> GetUserLoginHistoryAsync(Guid userId, PaginationParameters pagination);
        Task<SecurityInfoResult> GetUserSecurityInfoAsync(Guid userId);
        Task<PasswordChangeResult> ForcePasswordChangeAsync(Guid userId, ForcePasswordChangeRequest request);
    }

    

   
}