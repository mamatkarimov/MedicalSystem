using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using AuthService.Shared.DTOs;
using AuthService.Shared.DTOs.Auth;
using AuthService.Shared.DTOs.Roles;
using AuthService.Shared.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            IUserRepository userRepository,
            IRoleService roleService,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<UserResult> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                // Validate email uniqueness
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new UserResult
                    {
                        Success = false,
                        Message = $"Email {request.Email} is already registered"
                    };
                }

                var newUser = new User
                {
                    Email = request.Email,
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    ProfilePictureUrl = request.ProfilePictureUrl,
                    IsActive = true,
                    CreatedDate = DateTimeOffset.UtcNow,
                    CreatedBy = request.RequestedBy
                };

                var createResult = await _userManager.CreateAsync(newUser, request.Password);
                if (!createResult.Succeeded)
                {
                    return new UserResult
                    {
                        Success = false,
                        Message = "User creation failed",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Assign roles if specified
                if (request.Roles != null && request.Roles.Any())
                {
                    await _roleService.AssignRolesToUserAsync(newUser.Id.ToString(), request.Roles.Distinct().AsEnumerable());
                    //new AssignRolesRequest
                    //{
                    //    Roles = request.Roles,
                    //    AssignedBy = request.RequestedBy
                    //});

                    //if (!roleResult.Success)
                    //{
                    //    _logger.LogWarning("User created but role assignment failed for {Email}", request.Email);
                    //}
                }

                _logger.LogInformation("New user created: {Email}", request.Email);

                return new UserResult
                {
                    Success = true,
                    User = MapToUserDto(newUser)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task<UserResult> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new UserResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                return new UserResult
                {
                    Success = true,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<UserResult> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || user.IsDeleted)
                {
                    return new UserResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                return new UserResult
                {
                    Success = true,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
                throw;
            }
        }

        public async Task<UserListResult> GetAllUsersAsync(UserQueryParameters queryParameters)
        {
            try
            {
                var query = _userRepository.GetAll();

                // Apply filters
                if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
                {
                    query = query.Where(u =>
                        u.Email.Contains(queryParameters.SearchTerm) ||
                        u.FirstName.Contains(queryParameters.SearchTerm) ||
                        u.LastName.Contains(queryParameters.SearchTerm));
                }

                if (queryParameters.IsActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == queryParameters.IsActive.Value);
                }

                // Apply sorting
                query = queryParameters.SortDescending
                    ? query.OrderByDescending(u => EF.Property<object>(u, queryParameters.SortBy))
                    : query.OrderBy(u => EF.Property<object>(u, queryParameters.SortBy));

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var users = await query
                    .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .ToListAsync();

                return new UserListResult
                {
                    Success = true,
                    Users = users.Select(MapToUserDto),
                    TotalCount = totalCount,
                    PageNumber = queryParameters.PageNumber,
                    PageSize = queryParameters.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                throw;
            }
        }

        public async Task<UserResult> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || user.IsDeleted)
                {
                    return new UserResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Update basic info
                user.FirstName = request.FirstName ?? user.FirstName;
                user.LastName = request.LastName ?? user.LastName;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.ProfilePictureUrl = request.ProfilePictureUrl ?? user.ProfilePictureUrl;
                user.ModifiedDate = DateTimeOffset.UtcNow;
                user.ModifiedBy = request.ModifiedBy;

                // Update email if changed
                if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
                {
                    var emailExists = await _userManager.FindByEmailAsync(request.Email);
                    if (emailExists != null)
                    {
                        return new UserResult
                        {
                            Success = false,
                            Message = "Email already in use"
                        };
                    }

                    user.Email = request.Email;
                    user.UserName = request.Email;
                    user.EmailConfirmed = false; // Require email confirmation
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new UserResult
                    {
                        Success = false,
                        Message = "User update failed",
                        Errors = updateResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                _logger.LogInformation("User updated: {UserId}", userId);

                return new UserResult
                {
                    Success = true,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", userId);
                throw;
            }
        }

        public async Task<DeleteUserResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || user.IsDeleted)
                {
                    return new DeleteUserResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Soft delete
                user.IsDeleted = true;
                user.DeletedDate = DateTimeOffset.UtcNow;
                await _userManager.UpdateAsync(user);

                // Revoke all tokens
                await _userManager.UpdateSecurityStampAsync(user);

                _logger.LogInformation("User deleted: {UserId}", userId);

                return new DeleteUserResult
                {
                    Success = true,
                    SoftDeleted = true,
                    DeletionDate = user.DeletedDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                throw;
            }
        }

        public async Task<ProfileResult> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdWithProfileAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ProfileResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                return new ProfileResult
                {
                    Success = true,
                    Profile = new ProfileDto
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        ProfileImageUrl = user.ProfilePictureUrl,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        LastLoginDate = user.LastLoginDate
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<RoleAssignmentResult> AssignRolesToUserAsync(Guid userId, AssignRolesRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || user.IsDeleted)
                {
                    return new RoleAssignmentResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Get existing roles
                var existingRoles = await _userManager.GetRolesAsync(user);

                // Add only new roles
                var rolesToAdd = request.Roles.Except(existingRoles);
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

                if (!addResult.Succeeded)
                {
                    return new RoleAssignmentResult
                    {
                        Success = false,
                        Message = "Failed to assign roles",
                        Errors = addResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                _logger.LogInformation("Assigned roles to user {UserId}: {Roles}", userId, string.Join(",", rolesToAdd));

                return new RoleAssignmentResult
                {
                    Success = true,
                    RolesAssigned = rolesToAdd.Count(),
                    CurrentRoles = new UserRolesResult
                    {
                        Roles = await _userManager.GetRolesAsync(user)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning roles to user: {UserId}", userId);
                throw;
            }
        }

        // Additional methods would follow the same pattern...
        // (RemoveRolesFromUserAsync, GetUserRolesAsync, IsInRoleAsync, etc.)

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }

        Task<UserListResult> IUserService.GetAllUsersAsync(UserQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        Task<DeleteUserResult> IUserService.DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileResult> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileResult> UpdateProfilePictureAsync(Guid userId, UpdateProfilePictureRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> LockUserAccountAsync(Guid userId, LockAccountRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> UnlockUserAccountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> DisableUserAccountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> EnableUserAccountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<RoleAssignmentResult> IUserService.AssignRolesToUserAsync(Guid userId, AssignRolesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RoleAssignmentResult> RemoveRolesFromUserAsync(Guid userId, RemoveRolesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserRolesResult> GetUserRolesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(Guid userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<PermissionResult> AddPermissionsToUserAsync(Guid userId, AddPermissionsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PermissionResult> RemovePermissionsFromUserAsync(Guid userId, RemovePermissionsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserPermissionsResult> GetUserPermissionsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceResult> RegisterUserDeviceAsync(Guid userId, RegisterDeviceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceResult> RemoveUserDeviceAsync(Guid userId, Guid deviceId)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceListResult> GetUserDevicesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<LoginAuditResult> GetUserLoginHistoryAsync(Guid userId, PaginationParameters pagination)
        {
            throw new NotImplementedException();
        }

        public Task<SecurityInfoResult> GetUserSecurityInfoAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordChangeResult> ForcePasswordChangeAsync(Guid userId, ForcePasswordChangeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}