using Microsoft.AspNetCore.Identity;
using OjitexSystem_Backend.Common;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Dtos.Auth;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Security;

namespace OjitexSystem_Backend.Services.Auth;

public sealed class AuthenticationService(
    IUserRepository users,
    IAccessRepository access,
    IAuthUnitOfWork unitOfWork,
    PasswordHasher<IeUser> passwordHasher,
    JwtTokenService tokenService,
    TimeProvider timeProvider) : IAuthenticationService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrEmpty(request.Password)) return null;
        var user = await users.FindActiveAsync(request.Username.Trim(), cancellationToken);
        if (user is null || user.UserLockFlag != 0 || string.IsNullOrEmpty(user.Password)) return null;

        var verification = VerifyPassword(user, user.Password, request.Password);
        if (verification == PasswordVerificationResult.Failed) return null;
        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            user.Password = passwordHasher.HashPassword(user, request.Password);

        var now = timeProvider.GetBusinessNow();
        user.LastLoginDate = now;
        user.ChgDate = now;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var profile = await GetProfileAsync(user, cancellationToken);
        var (token, expiresAt) = tokenService.CreateToken(user.UserId, profile.Roles);
        return new LoginResponse(token, expiresAt, profile);
    }

    public async Task<AuthenticatedUserDto?> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await users.FindActiveAsync(userId, cancellationToken);
        return user is null || user.UserLockFlag != 0 ? null : await GetProfileAsync(user, cancellationToken);
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.NewPassword) || request.NewPassword.Length < 8)
            throw new ArgumentException("Mật khẩu mới phải có ít nhất 8 ký tự.", nameof(request));

        var user = await users.FindActiveAsync(userId, cancellationToken);
        if (user is null || user.UserLockFlag != 0 || string.IsNullOrEmpty(user.Password)
            || string.IsNullOrEmpty(request.CurrentPassword)
            || VerifyPassword(user, user.Password, request.CurrentPassword) == PasswordVerificationResult.Failed)
            return false;

        user.PastPassword2 = user.PastPassword1;
        user.PastPassword1 = user.PastPassword;
        user.PastPassword = user.Password;
        user.Password = passwordHasher.HashPassword(user, request.NewPassword);
        var now = timeProvider.GetBusinessNow();
        user.PasswordUpdateDate = now;
        user.ChgDate = now;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<AuthenticatedUserDto> GetProfileAsync(IeUser user, CancellationToken cancellationToken)
    {
        var roles = await access.GetRolesAsync(user.UserId, cancellationToken);
        var categories = await access.GetCategoriesAsync(user.UserId, roles, cancellationToken);
        return new AuthenticatedUserDto(user.UserId, user.UserFamilyName, user.UserFirstName, roles, categories);
    }

    private PasswordVerificationResult VerifyPassword(IeUser user, string hash, string password)
    {
        try { return passwordHasher.VerifyHashedPassword(user, hash, password); }
        catch (FormatException) { return PasswordVerificationResult.Failed; }
    }
}
