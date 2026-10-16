using System.Data;

namespace OjitexSystem_Backend.Repositories.Auth;

public interface IAuthUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IAuthTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
}

public interface IAuthTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
