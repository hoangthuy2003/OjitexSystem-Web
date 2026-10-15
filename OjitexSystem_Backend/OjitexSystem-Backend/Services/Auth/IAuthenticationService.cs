using OjitexSystem_Backend.Dtos.Auth;

namespace OjitexSystem_Backend.Services.Auth;

public interface IAuthenticationService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<AuthenticatedUserDto?> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default);

    Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);
}
