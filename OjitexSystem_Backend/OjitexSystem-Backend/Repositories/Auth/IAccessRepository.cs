using OjitexSystem_Backend.Dtos.Access;

namespace OjitexSystem_Backend.Repositories.Auth;

public interface IAccessRepository
{
    Task<IReadOnlyList<string>> GetRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(
        string userId, IReadOnlyCollection<string> roles, CancellationToken cancellationToken = default);
    Task<bool> IsActiveAdminAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasCategoryAsync(string userId, string categoryId, CancellationToken cancellationToken = default);
    Task<int> CountActiveAdminsAsync(CancellationToken cancellationToken = default);
}
