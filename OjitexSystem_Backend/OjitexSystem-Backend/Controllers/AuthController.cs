using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Services;

namespace OjitexSystem_Backend.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Vui lòng nhập tên tài khoản và mật khẩu." });
        }

        var response = await authenticationService.LoginAsync(request);
        return response is null
            ? Unauthorized(new { message = "Thông tin đăng nhập không hợp lệ hoặc tài khoản bị khóa." })
            : Ok(response);
    }

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthenticatedUserDto>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var currentUser = await authenticationService.GetCurrentUserAsync(userId);
        return currentUser is null ? Unauthorized() : Ok(currentUser);
    }

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.CurrentPassword)
            || string.IsNullOrEmpty(request.NewPassword)
            || request.NewPassword.Length < 8)
        {
            return BadRequest(new { message = "Nhập mật khẩu hiện tại và mật khẩu mới tối thiểu 8 ký tự." });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var changed = await authenticationService.ChangePasswordAsync(userId, request);
        return changed
            ? NoContent()
            : BadRequest(new { message = "Mật khẩu hiện tại không chính xác." });
    }
}
