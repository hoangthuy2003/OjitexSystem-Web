using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Services;

public interface IAuthenticationService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);

    Task<AuthenticatedUserDto?> GetCurrentUserAsync(string userId);

    Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
