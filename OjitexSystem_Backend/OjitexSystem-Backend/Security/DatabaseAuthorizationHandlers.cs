using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OjitexSystem_Backend.Services.Auth;

namespace OjitexSystem_Backend.Security;

public sealed class ActiveUserHandler(IAccessService access, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<ActiveUserRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(userId)
            && await access.IsActiveUserAsync(userId, httpContextAccessor.HttpContext?.RequestAborted ?? default))
            context.Succeed(requirement);
    }
}

public sealed class AdminAccessHandler(IAccessService access, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<AdminAccessRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAccessRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(userId)
            && await access.IsAdminAsync(userId, httpContextAccessor.HttpContext?.RequestAborted ?? default))
            context.Succeed(requirement);
    }
}

public sealed class CategoryAccessHandler(IAccessService access, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<CategoryAccessRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CategoryAccessRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(userId)
            && await access.HasCategoryAsync(userId, requirement.CategoryId, httpContextAccessor.HttpContext?.RequestAborted ?? default))
            context.Succeed(requirement);
    }
}
