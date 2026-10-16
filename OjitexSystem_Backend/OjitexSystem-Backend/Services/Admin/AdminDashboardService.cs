using System.Data;
using Microsoft.AspNetCore.Identity;
using OjitexSystem_Backend.Common;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos.Access;
using OjitexSystem_Backend.Dtos.Admin;
using OjitexSystem_Backend.Repositories.Admin;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Security;

namespace OjitexSystem_Backend.Services.Admin;

public sealed class AdminDashboardService(
    IAdminUserRepository repository,
    IUserRepository users,
    IAccessRepository access,
    IAuthUnitOfWork unitOfWork,
    PasswordHasher<IeUser> passwordHasher,
    TimeProvider timeProvider) : IAdminDashboardService
{
    public Task<IReadOnlyList<AdminUserDto>> GetUsersAsync(CancellationToken cancellationToken = default) =>
        repository.GetUsersAsync(cancellationToken: cancellationToken);

    public async Task<AdminUserDto?> GetUserAsync(string userId, CancellationToken cancellationToken = default) =>
        (await repository.GetUsersAsync(userId, cancellationToken)).SingleOrDefault();

    public Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default) =>
        repository.GetRolesAsync(cancellationToken);

    public Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        repository.GetCategoriesAsync(cancellationToken);

    public async Task<AdminUserDto?> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var userId = request.UserId?.Trim();
        if (string.IsNullOrWhiteSpace(userId) || userId == "*" || userId.Length > 100)
            throw new AdminOperationException("Mã tài khoản phải có từ 1 đến 100 ký tự.");

        await using var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        if (await users.ExistsAsync(userId, cancellationToken)) return null;
        var roles = await ValidateRolesAsync(request.Roles ?? [], cancellationToken);
        var categoryIds = await ValidateCategoriesAsync(request.CategoryIds ?? [], cancellationToken);
        var now = timeProvider.GetBusinessNow();
        var user = new IeUser
        {
            UserId = userId,
            UserFamilyName = NormalizeName(request.UserFamilyName),
            UserFirstName = NormalizeName(request.UserFirstName),
            PasswordMissCount = 0, UserLockFlag = 0, LogicalDelFlag = 0,
            CommandHideFlag = 0, EntryDate = now, ChgDate = now
        };
        user.Password = passwordHasher.HashPassword(user, UserPasswordReset.DefaultPassword);
        repository.Add(user);
        await repository.SynchronizeAccessAsync(userId, roles, categoryIds, now, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return await GetUserAsync(userId, cancellationToken);
    }

    public async Task<AdminUserDto?> UpdateUserAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        // Hold the admin count and the write in the same serializable transaction.
        await using var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var user = await users.FindActiveAsync(userId, cancellationToken);
        if (user is null) return null;
        if (request.Roles is null || request.CategoryIds is null)
            throw new AdminOperationException("Roles và CategoryIds là bắt buộc.");

        var roles = await ValidateRolesAsync(request.Roles, cancellationToken);
        var categoryIds = await ValidateCategoriesAsync(request.CategoryIds, cancellationToken);
        await EnsureAdminRemainsAsync(userId, roles, cancellationToken);
        var now = timeProvider.GetBusinessNow();
        user.UserFamilyName = NormalizeName(request.UserFamilyName);
        user.UserFirstName = NormalizeName(request.UserFirstName);
        user.ChgDate = now;
        await repository.SynchronizeAccessAsync(userId, roles, categoryIds, now, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return await GetUserAsync(userId, cancellationToken);
    }

    public async Task<bool> ResetPasswordAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await users.FindActiveAsync(userId, cancellationToken);
        if (user is null) return false;
        UserPasswordReset.Apply(user, passwordHasher, timeProvider.GetBusinessNow());
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var user = await users.FindActiveAsync(userId, cancellationToken);
        if (user is null) return false;
        await EnsureAdminRemainsAsync(userId, [], cancellationToken);
        user.LogicalDelFlag = 1;
        user.ChgDate = timeProvider.GetBusinessNow();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    private async Task<string[]> ValidateRolesAsync(IEnumerable<string> requested, CancellationToken cancellationToken)
    {
        var roles = NormalizeIds(requested);
        if (roles.Any(role => role.Length > 20))
            throw new AdminOperationException("Mã role không được dài quá 20 ký tự.");
        var existing = await repository.GetExistingRolesAsync(roles, cancellationToken);
        if (existing.Count != roles.Length)
            throw new AdminOperationException("Có role không tồn tại trong IE_ROLE.");
        return existing.ToArray();
    }

    private async Task<string[]> ValidateCategoriesAsync(IEnumerable<string> requested, CancellationToken cancellationToken)
    {
        var ids = NormalizeIds(requested);
        if (ids.Any(id => id.Length > 10))
            throw new AdminOperationException("CategoryId không được dài quá 10 ký tự.");
        var existing = await repository.GetExistingCategoriesAsync(ids, cancellationToken);
        if (existing.Count != ids.Length)
            throw new AdminOperationException("Có category không tồn tại trong IE_CATEGORY.");
        return existing.ToArray();
    }

    private async Task EnsureAdminRemainsAsync(string userId, IReadOnlyCollection<string> newRoles, CancellationToken cancellationToken)
    {
        if (newRoles.Contains(AccessConstants.AdminRole, StringComparer.OrdinalIgnoreCase)
            || !await access.IsActiveAdminAsync(userId, cancellationToken)) return;
        if (await access.CountActiveAdminsAsync(cancellationToken) <= 1)
            throw new AdminOperationException("Không thể xóa hoặc hạ quyền admin cuối cùng.");
    }

    private static string[] NormalizeIds(IEnumerable<string> values) => values
        .Where(value => !string.IsNullOrWhiteSpace(value)).Select(value => value.Trim())
        .Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

    private static string? NormalizeName(string? name)
    {
        var normalized = name?.Trim();
        if (normalized?.Length > 40) throw new AdminOperationException("Tên người dùng không được dài quá 40 ký tự.");
        return string.IsNullOrEmpty(normalized) ? null : normalized;
    }
}
