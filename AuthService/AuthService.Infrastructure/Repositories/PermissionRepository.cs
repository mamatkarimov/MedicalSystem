using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using AuthService.Infrastructure.Data;
using AuthService.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Permission> GetByIdAsync(Guid id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<Permission> GetByNameAsync(string name)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByRoleAsync(Guid roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task AddAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await GetByIdAsync(id);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Permission> GetByIdAsync(string permissionId)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string permissionId)
        {
            throw new NotImplementedException();
        }

        public Task AssignToRoleAsync(string roleId, string permissionId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(string roleId, string permissionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleHasPermissionAsync(string roleId, string permissionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Permission>> GetUserPermissionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task AssignMultipleToRoleAsync(string roleId, IEnumerable<string> permissionIds)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMultipleFromRoleAsync(string roleId, IEnumerable<string> permissionIds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string permissionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NameExistsAsync(string permissionName)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountRolePermissionsAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResponse<Permission>> GetPagedPermissionsAsync(PaginationParameters pagination)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResponse<Permission>> GetPagedRolePermissionsAsync(string roleId, PaginationParameters pagination)
        {
            throw new NotImplementedException();
        }
    }
}