using Microsoft.AspNetCore.Authorization;

namespace OjitexSystem_Backend.Security;

public sealed class AdminAccessRequirement : IAuthorizationRequirement;

public sealed class CategoryAccessRequirement(string categoryId) : IAuthorizationRequirement
{
    public string CategoryId { get; } = categoryId;
}
