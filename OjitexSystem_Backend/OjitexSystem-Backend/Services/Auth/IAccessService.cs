namespace OjitexSystem_Backend.Services.Auth;

public interface IAccessService
{
    Task<bool> IsActiveUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> IsAdminAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasCategoryAsync(string userId, string categoryId, CancellationToken cancellationToken = default);
}
