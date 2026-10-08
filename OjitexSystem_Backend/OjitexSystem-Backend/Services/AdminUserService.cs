using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Services;

public sealed class AdminUserService(
    AuthContext context,
    PasswordHasher<IeUser> passwordHasher)
{
    private const string DefaultPassword = "123456";

    public async Task<IReadOnlyList<AdminUserDto>> GetUsersAsync()
    {
        var users = await context.IeUsers
            .AsNoTracking()
            .Where(user => user.LogicalDelFlag == 0)
            .OrderBy(user => user.UserId)
            .ToListAsync();
        var userIds = users.Select(user => user.UserId).ToList();
        var roles = await context.IeUserRoles
            .AsNoTracking()
            .Where(role => userIds.Contains(role.UserId))
            .ToListAsync();
        var roleLookup = roles
            .GroupBy(role => role.UserId)
            .ToDictionary(group => group.Key, group => group.Select(role => role.Role).ToArray());
        var roleIds = roles.Select(role => role.Role).Distinct().ToList();
        var permissions = await context.IeCategoryAuths
            .AsNoTracking()
            .Where(permission => userIds.Contains(permission.UserId)
                || roleIds.Contains(permission.Role))
            .ToListAsync();
        var categoryIds = permissions.Select(permission => permission.CategoryId).Distinct().ToList();
        var categories = await context.IeCategories
            .AsNoTracking()
            .Where(category => categoryIds.Contains(category.CategoryId))
            .ToDictionaryAsync(category => category.CategoryId);

        return users.Select(user =>
        {
            var userRoles = roleLookup.GetValueOrDefault(user.UserId) ?? [];
            var authorizedCategoryIds = permissions
                .Where(permission =>
                    (permission.AuthType == 0 && permission.UserId == user.UserId)
                    || (permission.AuthType == 1
                        && userRoles.Contains(permission.Role, StringComparer.OrdinalIgnoreCase)))
                .Select(permission => permission.CategoryId)
                .Distinct();
            var userCategories = authorizedCategoryIds
                .Where(categories.ContainsKey)
                .Select(categoryId => new CategoryDto(categoryId, categories[categoryId].CategoryName))
                .OrderBy(category => category.CategoryName)
                .ToArray();
            var directCategoryIds = permissions
                .Where(permission => permission.AuthType == 0 && permission.UserId == user.UserId)
                .Select(permission => permission.CategoryId)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return new AdminUserDto(
                user.UserId,
                user.UserFamilyName,
                user.UserFirstName,
                user.UserLockFlag != 0,
                user.LastLoginDate,
                userRoles,
                userCategories,
                directCategoryIds);
        }).ToArray();
    }

    public async Task<AdminUserDto?> GetUserAsync(string userId)
    {
        var users = await GetUsersAsync();
        return users.SingleOrDefault(user => user.UserId == userId);
    }

    public async Task<AdminUserDto?> CreateUserAsync(CreateUserRequest request)
    {
        var userId = request.UserId.Trim();
        if (string.IsNullOrWhiteSpace(userId) || userId == "*" || userId.Length > 100)
        {
            throw new AdminOperationException("Mã tài khoản phải có từ 1 đến 100 ký tự.");
        }

        if (await context.IeUsers.AnyAsync(user => user.UserId == userId))
        {
            return null;
        }

        var roles = await ValidateRolesAsync(request.Roles ?? []);
        var categoryIds = await ValidateCategoriesAsync(request.CategoryIds ?? []);
        var now = DateTime.Now;
        var user = new IeUser
        {
            UserId = userId,
            UserFamilyName = NormalizeName(request.UserFamilyName),
            UserFirstName = NormalizeName(request.UserFirstName),
            PasswordMissCount = 0,
            UserLockFlag = 0,
            LogicalDelFlag = 0,
            CommandHideFlag = 0,
            EntryDate = now,
            ChgDate = now
        };
        user.Password = passwordHasher.HashPassword(user, DefaultPassword);

        await using var transaction = await context.Database.BeginTransactionAsync();
        context.IeUsers.Add(user);
        context.IeUserRoles.AddRange(roles.Select(role => NewUserRole(userId, role, now)));
        context.IeCategoryAuths.AddRange(categoryIds.Select(categoryId =>
            NewUserCategoryPermission(userId, categoryId, now)));
        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return await GetUserAsync(userId);
    }

    public async Task<AdminUserDto?> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        var user = await context.IeUsers
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId
                && candidate.LogicalDelFlag == 0);
        if (user is null)
        {
            return null;
        }

        if (request.Roles is null || request.CategoryIds is null)
        {
            throw new AdminOperationException("Roles và CategoryIds là bắt buộc.");
        }

        var roles = await ValidateRolesAsync(request.Roles);
        var categoryIds = await ValidateCategoriesAsync(request.CategoryIds);
        await EnsureAdminRemainsAsync(userId, roles);
        var now = DateTime.Now;

        await using var transaction = await context.Database.BeginTransactionAsync();
        user.UserFamilyName = NormalizeName(request.UserFamilyName);
        user.UserFirstName = NormalizeName(request.UserFirstName);
        user.ChgDate = now;

        var oldRoles = await context.IeUserRoles.Where(role => role.UserId == userId).ToListAsync();
        context.IeUserRoles.RemoveRange(oldRoles);
        context.IeUserRoles.AddRange(roles.Select(role => NewUserRole(userId, role, now)));

        var oldPermissions = await context.IeCategoryAuths
            .Where(permission => permission.AuthType == 0 && permission.UserId == userId)
            .ToListAsync();
        context.IeCategoryAuths.RemoveRange(oldPermissions);
        context.IeCategoryAuths.AddRange(categoryIds.Select(categoryId =>
            NewUserCategoryPermission(userId, categoryId, now)));

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return await GetUserAsync(userId);
    }

    public async Task<bool> ResetPasswordAsync(string userId)
    {
        var user = await context.IeUsers
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId
                && candidate.LogicalDelFlag == 0);
        if (user is null)
        {
            return false;
        }

        var now = DateTime.Now;
        user.Password = passwordHasher.HashPassword(user, DefaultPassword);
        user.PastPassword = null;
        user.PastPassword1 = null;
        user.PastPassword2 = null;
        user.PasswordUpdateDate = now;
        user.PasswordMissDate = null;
        user.PasswordMissCount = 0;
        user.UserLockFlag = 0;
        user.ChgDate = now;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await context.IeUsers
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId
                && candidate.LogicalDelFlag == 0);
        if (user is null)
        {
            return false;
        }

        await EnsureAdminRemainsAsync(userId, []);
        user.LogicalDelFlag = 1;
        user.ChgDate = DateTime.Now;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IReadOnlyList<RoleDto>> GetRolesAsync()
    {
        return await context.IeRoles
            .AsNoTracking()
            .OrderBy(role => role.Role)
            .Select(role => new RoleDto(role.Role, role.RoleDesc1))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
    {
        return await context.IeCategories
            .AsNoTracking()
            .OrderBy(category => category.CategoryName)
            .Select(category => new CategoryDto(category.CategoryId, category.CategoryName))
            .ToListAsync();
    }

    private async Task<string[]> ValidateRolesAsync(IEnumerable<string> requestedRoles)
    {
        var roles = requestedRoles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (roles.Any(role => role.Length > 20))
        {
            throw new AdminOperationException("Mã role không được dài quá 20 ký tự.");
        }

        var existingRoles = await context.IeRoles
            .Where(role => roles.Contains(role.Role))
            .Select(role => role.Role)
            .ToListAsync();
        if (existingRoles.Count != roles.Length)
        {
            throw new AdminOperationException("Có role không tồn tại trong IE_ROLE.");
        }

        return existingRoles.ToArray();
    }

    private async Task<string[]> ValidateCategoriesAsync(IEnumerable<string> requestedCategoryIds)
    {
        var categoryIds = requestedCategoryIds
            .Where(categoryId => !string.IsNullOrWhiteSpace(categoryId))
            .Select(categoryId => categoryId.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (categoryIds.Any(categoryId => categoryId.Length > 10))
        {
            throw new AdminOperationException("CategoryId không được dài quá 10 ký tự.");
        }

        var existingCategories = await context.IeCategories
            .Where(category => categoryIds.Contains(category.CategoryId))
            .Select(category => category.CategoryId)
            .ToListAsync();
        if (existingCategories.Count != categoryIds.Length)
        {
            throw new AdminOperationException("Có category không tồn tại trong IE_CATEGORY.");
        }

        return existingCategories.ToArray();
    }

    private async Task EnsureAdminRemainsAsync(string userId, IReadOnlyCollection<string> newRoles)
    {
        var targetIsAdmin = await context.IeUserRoles
            .AnyAsync(role => role.UserId == userId && role.Role == "ADMIN");
        var remainsAdmin = newRoles.Contains("ADMIN", StringComparer.OrdinalIgnoreCase);
        if (!targetIsAdmin || remainsAdmin)
        {
            return;
        }

        var activeAdminCount = await context.IeUsers
            .Where(user => user.LogicalDelFlag == 0
                && user.UserLockFlag == 0
                && context.IeUserRoles.Any(role => role.UserId == user.UserId && role.Role == "ADMIN"))
            .CountAsync();
        if (activeAdminCount <= 1)
        {
            throw new AdminOperationException("Không thể xóa hoặc hạ quyền admin cuối cùng.");
        }
    }

    private static string? NormalizeName(string? name)
    {
        var normalizedName = name?.Trim();
        if (normalizedName?.Length > 40)
        {
            throw new AdminOperationException("Tên người dùng không được dài quá 40 ký tự.");
        }

        return string.IsNullOrEmpty(normalizedName) ? null : normalizedName;
    }

    private static IeUserRole NewUserRole(string userId, string role, DateTime now) =>
        new()
        {
            UserId = userId,
            Role = role,
            EntryDate = now,
            ChgDate = now
        };

    private static IeCategoryAuth NewUserCategoryPermission(
        string userId,
        string categoryId,
        DateTime now) =>
        new()
        {
            CategoryId = categoryId,
            AuthType = 0,
            UserId = userId,
            Role = "*",
            ExecutableType = 1,
            EntryDate = now,
            ChgDate = now
        };
}

public sealed class AdminOperationException(string message) : Exception(message);
