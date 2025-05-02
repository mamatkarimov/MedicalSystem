using AuthService.Core.Entities;
using AuthService.Core.Models;
using AuthService.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        IQueryable<User> GetAll();
        Task<PaginatedResponse<User>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<User> GetByIdWithRolesAsync(Guid id);
        Task<User> GetByIdWithDevicesAsync(Guid id);
        Task<User> GetByIdWithProfileAsync(Guid id);
    }
}