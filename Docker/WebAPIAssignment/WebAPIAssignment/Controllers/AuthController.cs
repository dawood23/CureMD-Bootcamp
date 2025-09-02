using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);

            var userId = await _userService.CreateUser(user, user.UserID);

            if (userId <= 0) return BadRequest("User could not be created");
            return Ok(new { userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUser(request.Username, request.Password);
            if (user == null) return Unauthorized("Invalid username or password");

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);

            var user = await _userService.GetById(userId);
            if (user == null) return NotFound();

            if (!PasswordHelper.VerifyPassword(request.OldPassword, user.PasswordHash))
                return Unauthorized("Old password incorrect");

            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            var updated = await _userService.UpdateUser(user, userId);

            if (!updated) return BadRequest("Password not changed");
            return Ok("Password changed successfully");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}

