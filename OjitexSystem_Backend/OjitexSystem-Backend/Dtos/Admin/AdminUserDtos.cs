using System.ComponentModel.DataAnnotations;
using OjitexSystem_Backend.Dtos.Access;

namespace OjitexSystem_Backend.Dtos.Admin;

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
    [Required, StringLength(100)] string UserId,
    [StringLength(40)] string? UserFamilyName,
    [StringLength(40)] string? UserFirstName,
    IReadOnlyList<string>? Roles,
    IReadOnlyList<string>? CategoryIds);

public sealed record UpdateUserRequest(
    [StringLength(40)] string? UserFamilyName,
    [StringLength(40)] string? UserFirstName,
    [Required] IReadOnlyList<string> Roles,
    [Required] IReadOnlyList<string> CategoryIds);
