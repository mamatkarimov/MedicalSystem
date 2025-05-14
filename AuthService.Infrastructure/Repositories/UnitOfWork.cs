using AuthService.Core.Interfaces;
using AuthService.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => new UserRepository(_context);
        public IRoleRepository Roles => new RoleRepository(_context);
        public IRefreshTokenRepository RefreshTokens => new RefreshTokenRepository(_context);
        public IPermissionRepository Permissions => new PermissionRepository(_context);
        public IAuditLogRepository AuditLogs => new AuditLogRepository(_context);

        public IDeviceRepository Devices { get; }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}