using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos.Access;
using OjitexSystem_Backend.Dtos.Admin;

namespace OjitexSystem_Backend.Repositories.Admin;

public sealed class AdminUserRepository(AuthContext context) : IAdminUserRepository
{
    public async Task<IReadOnlyList<AdminUserDto>> GetUsersAsync(
        string? userId = null, CancellationToken cancellationToken = default)
    {
        var query = context.IeUsers.AsNoTracking().Where(user => user.LogicalDelFlag == 0);
        if (userId is not null) query = query.Where(user => user.UserId == userId);

        var users = await query.OrderBy(user => user.UserId).Select(user => new
        {
            user.UserId, user.UserFamilyName, user.UserFirstName, user.UserLockFlag, user.LastLoginDate
        }).ToListAsync(cancellationToken);
        if (users.Count == 0) return [];

        var userIds = users.Select(user => user.UserId).ToArray();
        var roles = await context.IeUserRoles.AsNoTracking()
            .Where(role => userIds.Contains(role.UserId)).ToListAsync(cancellationToken);
        var roleLookup = roles.ToLookup(role => role.UserId, StringComparer.OrdinalIgnoreCase);
        var roleIds = roles.Select(role => role.Role).Distinct().ToArray();
        var permissions = await context.IeCategoryAuths.AsNoTracking()
            .Where(permission => (permission.AuthType == 0 && userIds.Contains(permission.UserId))
                || (permission.AuthType == 1 && roleIds.Contains(permission.Role)))
            .Select(permission => new { permission.UserId, permission.Role, permission.CategoryId, permission.AuthType })
            .ToListAsync(cancellationToken);
        var directLookup = permissions.Where(permission => permission.AuthType == 0)
            .ToLookup(permission => permission.UserId, permission => permission.CategoryId, StringComparer.OrdinalIgnoreCase);
        var inheritedLookup = permissions.Where(permission => permission.AuthType == 1)
            .ToLookup(permission => permission.Role, permission => permission.CategoryId, StringComparer.OrdinalIgnoreCase);
        var categoryIds = permissions.Select(permission => permission.CategoryId).Distinct().ToArray();
        var categories = await context.IeCategories.AsNoTracking()
            .Where(category => categoryIds.Contains(category.CategoryId))
            .Select(category => new CategoryDto(category.CategoryId, category.CategoryName))
            .ToListAsync(cancellationToken);
        var categoryLookup = categories.ToDictionary(category => category.CategoryId, StringComparer.OrdinalIgnoreCase);

        return users.Select(user =>
        {
            var userRoles = roleLookup[user.UserId].Select(role => role.Role).ToArray();
            var directCategoryIds = directLookup[user.UserId].Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var userCategories = directCategoryIds.Concat(userRoles.SelectMany(role => inheritedLookup[role]))
                .Distinct(StringComparer.OrdinalIgnoreCase).Where(categoryLookup.ContainsKey)
                .Select(categoryId => categoryLookup[categoryId]).OrderBy(category => category.CategoryName).ToArray();
            return new AdminUserDto(user.UserId, user.UserFamilyName, user.UserFirstName,
                user.UserLockFlag != 0, user.LastLoginDate, userRoles, userCategories, directCategoryIds);
        }).ToArray();
    }

    public async Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default) =>
        await context.IeRoles.AsNoTracking().OrderBy(role => role.Role)
            .Select(role => new RoleDto(role.Role, role.RoleDesc1)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        await context.IeCategories.AsNoTracking().OrderBy(category => category.CategoryName)
            .Select(category => new CategoryDto(category.CategoryId, category.CategoryName)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<string>> GetExistingRolesAsync(
        IReadOnlyCollection<string> roles, CancellationToken cancellationToken = default) =>
        await context.IeRoles.AsNoTracking().Where(role => roles.Contains(role.Role))
            .Select(role => role.Role).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<string>> GetExistingCategoriesAsync(
        IReadOnlyCollection<string> categoryIds, CancellationToken cancellationToken = default) =>
        await context.IeCategories.AsNoTracking().Where(category => categoryIds.Contains(category.CategoryId))
            .Select(category => category.CategoryId).ToListAsync(cancellationToken);

    public void Add(IeUser user) => context.IeUsers.Add(user);

    public async Task SynchronizeAccessAsync(string userId, IReadOnlyCollection<string> roles,
        IReadOnlyCollection<string> categoryIds, DateTime now, CancellationToken cancellationToken = default)
    {
        var existingRoles = await context.IeUserRoles.Where(role => role.UserId == userId).ToListAsync(cancellationToken);
        var selectedRoles = roles.ToHashSet(StringComparer.OrdinalIgnoreCase);
        context.IeUserRoles.RemoveRange(existingRoles.Where(role => !selectedRoles.Contains(role.Role)));
        var existingRoleIds = existingRoles.Select(role => role.Role).ToHashSet(StringComparer.OrdinalIgnoreCase);
        context.IeUserRoles.AddRange(roles.Where(role => !existingRoleIds.Contains(role)).Select(role =>
            new IeUserRole { UserId = userId, Role = role, EntryDate = now, ChgDate = now }));

        // Only direct grants belong to this user; inherited grants belong to the role.
        var existingPermissions = await context.IeCategoryAuths
            .Where(permission => permission.AuthType == 0 && permission.UserId == userId).ToListAsync(cancellationToken);
        var selectedCategories = categoryIds.ToHashSet(StringComparer.OrdinalIgnoreCase);
        context.IeCategoryAuths.RemoveRange(existingPermissions.Where(permission => !selectedCategories.Contains(permission.CategoryId)));
        var existingCategoryIds = existingPermissions.Select(permission => permission.CategoryId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        context.IeCategoryAuths.AddRange(categoryIds.Where(categoryId => !existingCategoryIds.Contains(categoryId)).Select(categoryId =>
            new IeCategoryAuth
            {
                CategoryId = categoryId, AuthType = 0, UserId = userId, Role = "*",
                ExecutableType = 1, EntryDate = now, ChgDate = now
            }));
    }
}
