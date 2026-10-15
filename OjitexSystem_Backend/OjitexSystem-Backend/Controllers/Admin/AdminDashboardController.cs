using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Admin;
using OjitexSystem_Backend.Dtos.Access;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Admin;

namespace OjitexSystem_Backend.Controllers.Admin;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.AdminOnly)]
[Route("api/admin/dashboard")]
public sealed class AdminDashboardController(IAdminDashboardService adminDashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminUserDto>>> GetUsers(CancellationToken cancellationToken)
    {
        return Ok(await adminDashboardService.GetUsersAsync(cancellationToken));
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<AdminUserDto>> GetUser(string userId, CancellationToken cancellationToken)
    {
        var user = await adminDashboardService.GetUserAsync(userId, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles(CancellationToken cancellationToken)
    {
        return Ok(await adminDashboardService.GetRolesAsync(cancellationToken));
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        return Ok(await adminDashboardService.GetCategoriesAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<AdminUserDto>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await adminDashboardService.CreateUserAsync(request, cancellationToken);
            if (user is null)
            {
                return Conflict(new { message = "Mã tài khoản đã tồn tại." });
            }

            return CreatedAtAction(nameof(GetUser), new { userId = user.UserId }, user);
        }
        catch (AdminOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<AdminUserDto>> UpdateUser(
        string userId,
        UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await adminDashboardService.UpdateUserAsync(userId, request, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }
        catch (AdminOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPost("{userId}/reset-password")]
    public async Task<IActionResult> ResetPassword(string userId, CancellationToken cancellationToken)
    {
        var reset = await adminDashboardService.ResetPasswordAsync(userId, cancellationToken);
        return reset
            ? Ok(new { message = "Mật khẩu đã được đặt lại về 123456." })
            : NotFound();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await adminDashboardService.DeleteUserAsync(userId, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (AdminOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}
