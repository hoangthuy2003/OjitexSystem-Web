namespace OjitexSystem_Backend.Dtos;

public sealed record LoginRequest(string Username, string Password);

public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public sealed record LoginResponse(
    string AccessToken,
    DateTime ExpiresAt,
    AuthenticatedUserDto User);

public sealed record AuthenticatedUserDto(
    string UserId,
    string? UserFamilyName,
    string? UserFirstName,
    IReadOnlyList<string> Roles,
    IReadOnlyList<CategoryDto> Categories);

public sealed record CategoryDto(string CategoryId, string CategoryName);

public sealed record AdminUserDto(
    string UserId,
    string? UserFamilyName,
    string? UserFirstName,
    bool IsLocked,
    DateTime? LastLoginDate,
    IReadOnlyList<string> Roles,
    IReadOnlyList<CategoryDto> Categories,
    IReadOnlyList<string> DirectCategoryIds);

public sealed record RoleDto(string Role, string? Description);

public sealed record CreateUserRequest(
    string UserId,
    string? UserFamilyName,
    string? UserFirstName,
    IReadOnlyList<string>? Roles,
    IReadOnlyList<string>? CategoryIds);

public sealed record UpdateUserRequest(
    string? UserFamilyName,
    string? UserFirstName,
    IReadOnlyList<string>? Roles,
    IReadOnlyList<string>? CategoryIds);
