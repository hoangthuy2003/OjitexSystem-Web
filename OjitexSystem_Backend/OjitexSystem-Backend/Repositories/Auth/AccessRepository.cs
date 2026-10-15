using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos.Access;
using OjitexSystem_Backend.Security;

namespace OjitexSystem_Backend.Repositories.Auth;

public sealed class AccessRepository(AuthContext context) : IAccessRepository
{
    public async Task<IReadOnlyList<string>> GetRolesAsync(
        string userId, CancellationToken cancellationToken = default) =>
        await context.IeUserRoles.AsNoTracking().Where(role => role.UserId == userId)
            .Select(role => role.Role).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(
        string userId, IReadOnlyCollection<string> roles, CancellationToken cancellationToken = default) =>
        await context.IeCategories.AsNoTracking()
            .Where(category => context.IeCategoryAuths.Any(permission =>
                permission.CategoryId == category.CategoryId
                && ((permission.AuthType == 0 && permission.UserId == userId)
                    || (permission.AuthType == 1 && roles.Contains(permission.Role)))))
            .OrderBy(category => category.CategoryName)
            .Select(category => new CategoryDto(category.CategoryId, category.CategoryName))
            .ToListAsync(cancellationToken);

    public Task<bool> IsActiveAdminAsync(string userId, CancellationToken cancellationToken = default) =>
        ActiveUsers().AnyAsync(user => user.UserId == userId
            && context.IeUserRoles.Any(role => role.UserId == user.UserId
                && role.Role == AccessConstants.AdminRole), cancellationToken);

    public Task<bool> HasCategoryAsync(
        string userId, string categoryId, CancellationToken cancellationToken = default) =>
        ActiveUsers().AnyAsync(user => user.UserId == userId
            && context.IeCategoryAuths.Any(permission => permission.CategoryId == categoryId
                && ((permission.AuthType == 0 && permission.UserId == user.UserId)
                    || (permission.AuthType == 1 && context.IeUserRoles.Any(role =>
                        role.UserId == user.UserId && role.Role == permission.Role)))), cancellationToken);

    public Task<int> CountActiveAdminsAsync(CancellationToken cancellationToken = default) =>
        ActiveUsers().CountAsync(user => context.IeUserRoles.Any(role =>
            role.UserId == user.UserId && role.Role == AccessConstants.AdminRole), cancellationToken);

    private IQueryable<IeUser> ActiveUsers() => context.IeUsers.AsNoTracking()
        .Where(user => user.LogicalDelFlag == 0 && user.UserLockFlag == 0);
}
