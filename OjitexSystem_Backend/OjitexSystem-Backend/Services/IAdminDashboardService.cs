using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Services;

public interface IAdminDashboardService
{
    Task<IReadOnlyList<AdminUserDto>> GetUsersAsync();

    Task<AdminUserDto?> GetUserAsync(string userId);

    Task<AdminUserDto?> CreateUserAsync(CreateUserRequest request);

    Task<AdminUserDto?> UpdateUserAsync(string userId, UpdateUserRequest request);

    Task<bool> ResetPasswordAsync(string userId);

    Task<bool> DeleteUserAsync(string userId);

    Task<IReadOnlyList<RoleDto>> GetRolesAsync();

    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync();
}
