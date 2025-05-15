using AuthService.Core.Entities;
using AuthService.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IAuditLogRepository
    {
        Task CreateAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<AuditLog>> GetByActionTypeAsync(string actionType);
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> SearchAsync(string searchTerm);
        Task<int> CountAsync(string userId = null, string actionType = null, DateTime? startDate = null, DateTime? endDate = null);

        // For paginated results (compatible with PaginationParameters)
        Task<PaginatedResponse<AuditLog>> GetPagedAsync(PaginationParameters pagination,
            string userId = null, string actionType = null,
            DateTime? startDate = null, DateTime? endDate = null);

        // For cleanup/maintenance
        Task<int> DeleteOldLogsAsync(DateTime cutoffDate);
        Task ArchiveLogsAsync(DateTime cutoffDate);
    }
}