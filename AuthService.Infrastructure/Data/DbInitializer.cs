using AuthService.Core.Entities;
using AuthService.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            ApplicationDbContext context,
        UserManager<User> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<DbInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            // Default roles
            var adminRole = new ApplicationRole
            {
                Name = "Admin",
                Description = "System Administrator with full access",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System"
            };

            var userRole = new ApplicationRole
            {
                Name = "User",
                Description = "Default role for authenticated users",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System"
            };

            if (!await _roleManager.RoleExistsAsync(adminRole.Name))
            {
                await _roleManager.CreateAsync(adminRole);
                _logger.LogInformation("Created admin role");
            }

            if (!await _roleManager.RoleExistsAsync(userRole.Name))
            {
                await _roleManager.CreateAsync(userRole);
                _logger.LogInformation("Created user role");
            }

            // Default permissions
            var permissions = new List<Permission>
            {
                new Permission { Name = "users.read", Description = "Read user information", Category = "Users" },
                new Permission { Name = "users.create", Description = "Create new users", Category = "Users" },
                new Permission { Name = "users.update", Description = "Update user information", Category = "Users" },
                new Permission { Name = "users.delete", Description = "Delete users", Category = "Users" },
                new Permission { Name = "roles.manage", Description = "Manage roles and permissions", Category = "Roles" }
            };

            foreach (var permission in permissions)
            {
                if (!_context.Permissions.Any(p => p.Name == permission.Name))
                {
                    _context.Permissions.Add(permission);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Created permission: {Permission}", permission.Name);
                }
            }

            // Assign permissions to admin role
            var adminPermissions = new List<RolePermission>
            {
                new RolePermission { RoleId = adminRole.Id, PermissionId = permissions[0].Id },
                new RolePermission { RoleId = adminRole.Id, PermissionId = permissions[1].Id },
                new RolePermission { RoleId = adminRole.Id, PermissionId = permissions[2].Id },
                new RolePermission { RoleId = adminRole.Id, PermissionId = permissions[3].Id },
                new RolePermission { RoleId = adminRole.Id, PermissionId = permissions[4].Id }
            };

            foreach (var adminPermission in adminPermissions)
            {
                if (!_context.RolePermissions.Any(rp =>
                    rp.RoleId == adminPermission.RoleId &&
                    rp.PermissionId == adminPermission.PermissionId))
                {
                    _context.RolePermissions.Add(adminPermission);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Assigned permission {PermissionId} to admin role", adminPermission.PermissionId);
                }
            }

            // Default admin user
            var adminUser = new User
            {
                Email = "admin@authservice.com",
                UserName = "admin@authservice.com",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System"
            };

            if (await _userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await _userManager.CreateAsync(adminUser, "Admin@1234");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
                    _logger.LogInformation("Created default admin user");

                    // Generate email confirmation token
                    var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                    await _userManager.ConfirmEmailAsync(adminUser, emailToken);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Error creating admin user: {Errors}", errors);
                }
            }

            // Ensure all migrations are applied
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                _logger.LogInformation("Applying pending migrations...");
                await _context.Database.MigrateAsync();
            }
        }
    }
}