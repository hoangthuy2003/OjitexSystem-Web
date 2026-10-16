using OjitexSystem_Backend.Dtos.Admin;
using OjitexSystem_Backend.Dtos.Access;

namespace OjitexSystem_Backend.Services.Admin;

public interface IAdminDashboardService
{
    Task<IReadOnlyList<AdminUserDto>> GetUsersAsync(CancellationToken cancellationToken = default);

    Task<AdminUserDto?> GetUserAsync(string userId, CancellationToken cancellationToken = default);

    Task<AdminUserDto?> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    Task<AdminUserDto?> UpdateUserAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken = default);

    Task<bool> ResetPasswordAsync(string userId, CancellationToken cancellationToken = default);

    Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}
