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
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId)
        {
            return await _context.AuditLogs
                .Where(al => al.UserId == userId)
                .OrderByDescending(al => al.ActionTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.AuditLogs
                .Where(al => al.ActionTime >= startDate && al.ActionTime <= endDate)
                .OrderByDescending(al => al.ActionTime)
                .ToListAsync();
        }

        public async Task<PaginatedResponse<AuditLog>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.AuditLogs
                .OrderByDescending(al => al.ActionTime);

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<AuditLog>
            {
                Data = items,
                TotalCount = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public Task CreateAsync(AuditLog auditLog)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditLog>> GetByActionTypeAsync(string actionType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditLog>> SearchAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(string userId = null, string actionType = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResponse<AuditLog>> GetPagedAsync(PaginationParameters pagination, string userId = null, string actionType = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteOldLogsAsync(DateTime cutoffDate)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveLogsAsync(DateTime cutoffDate)
        {
            throw new NotImplementedException();
        }
    }
}