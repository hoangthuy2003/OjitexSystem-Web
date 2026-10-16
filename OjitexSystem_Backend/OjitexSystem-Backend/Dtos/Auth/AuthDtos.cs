using System.ComponentModel.DataAnnotations;
using OjitexSystem_Backend.Dtos.Access;

namespace OjitexSystem_Backend.Dtos.Auth;

public sealed record LoginRequest(
    [Required, StringLength(100)] string Username,
    [Required] string Password);

public sealed record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required, MinLength(8)] string NewPassword);

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAt, AuthenticatedUserDto User);

public sealed record AuthenticatedUserDto(
    string UserId,
    string? UserFamilyName,
    string? UserFirstName,
    IReadOnlyList<string> Roles,
    IReadOnlyList<CategoryDto> Categories);
