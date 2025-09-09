using careliteBackend.Models;
using careliteBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace careliteBackend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Username and password are required"
                });
            }

            var result = await _authService.Authenticate(request.Username, request.Password);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Username and password are required"
                });
            }

            try
            {
                var newUserId = await _authService.CreateUser(user);

                if (newUserId < 1)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Failed to create user"
                    });
                }

                var loginResult = await _authService.Authenticate(user.Username, user.PasswordHash);
                return Ok(loginResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Username/Email Already Exists or Invalid Entries" ?? "Internal Server Error"
                });
            }
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
