using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Security;

namespace OjitexSystem_Backend.Services;

public sealed class AuthenticationService(
    AuthContext context,
    PasswordHasher<IeUser> passwordHasher,
    JwtTokenService tokenService)
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var userId = request.Username.Trim();
        var user = await context.IeUsers
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId
                && candidate.LogicalDelFlag == 0);
        if (user is null || user.UserLockFlag != 0 || string.IsNullOrEmpty(user.Password))
        {
            return null;
        }

        var verification = VerifyPassword(user, user.Password, request.Password);
        if (verification == PasswordVerificationResult.Failed)
        {
            return null;
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.Password = passwordHasher.HashPassword(user, request.Password);
        }

        user.LastLoginDate = DateTime.Now;
        user.ChgDate = DateTime.Now;
        await context.SaveChangesAsync();

        var roles = await context.IeUserRoles
            .Where(role => role.UserId == user.UserId)
            .Select(role => role.Role)
            .ToListAsync();
        var categories = await GetCategoriesAsync(user.UserId, roles);
        var (token, expiresAt) = tokenService.CreateToken(user.UserId, roles);
        var profile = new AuthenticatedUserDto(
            user.UserId,
            user.UserFamilyName,
            user.UserFirstName,
            roles,
            categories);

        return new LoginResponse(token, expiresAt, profile);
    }

    public async Task<AuthenticatedUserDto?> GetCurrentUserAsync(string userId)
    {
        var user = await context.IeUsers
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId
                && candidate.LogicalDelFlag == 0
                && candidate.UserLockFlag == 0);
        if (user is null)
        {
            return null;
        }

        var roles = await context.IeUserRoles
            .Where(role => role.UserId == userId)
            .Select(role => role.Role)
            .ToListAsync();
        var categories = await GetCategoriesAsync(userId, roles);

        return new AuthenticatedUserDto(
            user.UserId,
            user.UserFamilyName,
            user.UserFirstName,
            roles,
            categories);
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        if (request.NewPassword.Length < 8)
        {
            throw new ArgumentException("Mật khẩu mới phải có ít nhất 8 ký tự.");
        }

        var user = await context.IeUsers
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId
                && candidate.LogicalDelFlag == 0
                && candidate.UserLockFlag == 0);
        if (user is null || string.IsNullOrEmpty(user.Password)
            || VerifyPassword(user, user.Password, request.CurrentPassword)
                == PasswordVerificationResult.Failed)
        {
            return false;
        }

        user.PastPassword2 = user.PastPassword1;
        user.PastPassword1 = user.PastPassword;
        user.PastPassword = user.Password;
        user.Password = passwordHasher.HashPassword(user, request.NewPassword);
        user.PasswordUpdateDate = DateTime.Now;
        user.ChgDate = DateTime.Now;
        await context.SaveChangesAsync();
        return true;
    }

    private async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(
        string userId,
        IReadOnlyCollection<string> roles)
    {
        var permissionRows = await context.IeCategoryAuths
            .AsNoTracking()
            .Where(permission =>
                (permission.AuthType == 0 && permission.UserId == userId)
                || (permission.AuthType == 1 && roles.Contains(permission.Role)))
            .Select(permission => permission.CategoryId)
            .Distinct()
            .ToListAsync();
        if (permissionRows.Count == 0)
        {
            return [];
        }

        return await context.IeCategories
            .AsNoTracking()
            .Where(category => permissionRows.Contains(category.CategoryId))
            .OrderBy(category => category.CategoryName)
            .Select(category => new CategoryDto(category.CategoryId, category.CategoryName))
            .ToListAsync();
    }

    private PasswordVerificationResult VerifyPassword(IeUser user, string hash, string password)
    {
        try
        {
            return passwordHasher.VerifyHashedPassword(user, hash, password);
        }
        catch (FormatException)
        {
            return PasswordVerificationResult.Failed;
        }
    }
}
