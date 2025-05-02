using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IDeviceRepository Devices { get; }
        IAuditLogRepository AuditLogs { get; }
        IRoleRepository Roles { get; }  // If you have role-specific repository

        // Methods
        Task<int> CompleteAsync();
        Task<bool> SaveChangesAsync();
        void BeginTransaction();
        Task CommitAsync();
        Task RollbackAsync();

        // Optional - for more granular control
        Task ExecuteInTransactionAsync(Func<Task> operation);
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
    }
}
