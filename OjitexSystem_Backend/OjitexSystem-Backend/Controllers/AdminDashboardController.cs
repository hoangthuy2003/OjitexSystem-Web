using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services;

namespace OjitexSystem_Backend.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.AdminOnly)]
[Route("api/admin/dashboard")]
public sealed class AdminDashboardController(IAdminDashboardService adminDashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminUserDto>>> GetUsers()
    {
        return Ok(await adminDashboardService.GetUsersAsync());
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<AdminUserDto>> GetUser(string userId)
    {
        var user = await adminDashboardService.GetUserAsync(userId);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles()
    {
        return Ok(await adminDashboardService.GetRolesAsync());
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
    {
        return Ok(await adminDashboardService.GetCategoriesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<AdminUserDto>> CreateUser(CreateUserRequest request)
    {
        try
        {
            var user = await adminDashboardService.CreateUserAsync(request);
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
        UpdateUserRequest request)
    {
        try
        {
            var user = await adminDashboardService.UpdateUserAsync(userId, request);
            return user is null ? NotFound() : Ok(user);
        }
        catch (AdminOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPost("{userId}/reset-password")]
    public async Task<IActionResult> ResetPassword(string userId)
    {
        var reset = await adminDashboardService.ResetPasswordAsync(userId);
        return reset
            ? Ok(new { message = "Mật khẩu đã được đặt lại về 123456." })
            : NotFound();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            var deleted = await adminDashboardService.DeleteUserAsync(userId);
            return deleted ? NoContent() : NotFound();
        }
        catch (AdminOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}
