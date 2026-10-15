using OjitexSystem_Backend.Repositories.Auth;

namespace OjitexSystem_Backend.Services.Auth;

public sealed class AccessService(IAccessRepository repository) : IAccessService
{
    public Task<bool> IsAdminAsync(string userId, CancellationToken cancellationToken = default) =>
        repository.IsActiveAdminAsync(userId, cancellationToken);

    public Task<bool> HasCategoryAsync(string userId, string categoryId, CancellationToken cancellationToken = default) =>
        repository.HasCategoryAsync(userId, categoryId, cancellationToken);
}
