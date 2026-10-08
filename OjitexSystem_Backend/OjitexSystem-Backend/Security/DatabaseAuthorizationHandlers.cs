using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Security;

public sealed class AdminAccessHandler(AuthContext context)
    : AuthorizationHandler<AdminAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationContext,
        AdminAccessRequirement requirement)
    {
        var userId = authorizationContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var isActiveAdmin = await context.IeUsers
            .AnyAsync(user => user.UserId == userId
                && user.LogicalDelFlag == 0
                && user.UserLockFlag == 0
                && context.IeUserRoles.Any(role => role.UserId == userId && role.Role == "ADMIN"));

        if (isActiveAdmin)
        {
            authorizationContext.Succeed(requirement);
        }
    }
}

public sealed class CategoryAccessHandler(AuthContext context)
    : AuthorizationHandler<CategoryAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext authorizationContext,
        CategoryAccessRequirement requirement)
    {
        var userId = authorizationContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var isActiveUser = await context.IeUsers
            .AnyAsync(user => user.UserId == userId
                && user.LogicalDelFlag == 0
                && user.UserLockFlag == 0);
        if (!isActiveUser)
        {
            return;
        }

        var userRoles = context.IeUserRoles
            .Where(role => role.UserId == userId)
            .Select(role => role.Role);
        var canAccessCategory = await context.IeCategoryAuths
            .AnyAsync(permission => permission.CategoryId == requirement.CategoryId
                && ((permission.AuthType == 0 && permission.UserId == userId)
                    || (permission.AuthType == 1 && userRoles.Contains(permission.Role))));

        if (canAccessCategory)
        {
            authorizationContext.Succeed(requirement);
        }
    }
}
