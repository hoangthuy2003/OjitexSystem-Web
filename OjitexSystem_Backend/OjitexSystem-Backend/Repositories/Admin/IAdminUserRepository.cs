using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos.Access;
using OjitexSystem_Backend.Dtos.Admin;

namespace OjitexSystem_Backend.Repositories.Admin;

public interface IAdminUserRepository
{
    Task<IReadOnlyList<AdminUserDto>> GetUsersAsync(string? userId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetExistingRolesAsync(IReadOnlyCollection<string> roles, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetExistingCategoriesAsync(IReadOnlyCollection<string> categoryIds, CancellationToken cancellationToken = default);
    void Add(IeUser user);
    Task SynchronizeAccessAsync(string userId, IReadOnlyCollection<string> roles,
        IReadOnlyCollection<string> categoryIds, DateTime now, CancellationToken cancellationToken = default);
}
