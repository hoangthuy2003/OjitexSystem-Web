using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Repositories.Auth;

public sealed class AuthUnitOfWork(AuthContext context) : IAuthUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task<IAuthTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel, CancellationToken cancellationToken = default) =>
        new AuthTransaction(await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken));

    private sealed class AuthTransaction(IDbContextTransaction transaction) : IAuthTransaction
    {
        public Task CommitAsync(CancellationToken cancellationToken = default) =>
            transaction.CommitAsync(cancellationToken);

        public ValueTask DisposeAsync() => transaction.DisposeAsync();
    }
}
