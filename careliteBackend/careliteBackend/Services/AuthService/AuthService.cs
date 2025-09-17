using careliteBackend.DTOs;
using careliteBackend.Repository;
using careliteBackend.Services.Interfaces;
using System.Text;
using System.Security.Cryptography;
using careliteBackend.Models;

namespace careliteBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _auth;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IAuthRepository auth, IJwtTokenService jwtTokenService, IRoleRepository roleRepository)
        {
            _auth = auth;
            _jwtTokenService = jwtTokenService;
            _roleRepository = roleRepository;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<AuthResult> Authenticate(string username, string password)
        {
            var user = await _auth.GetByUsername(username);
            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    Message = "Username doesn't exist"
                };
            }

            var hash = HashPassword(password);

            if (user.PasswordHash == hash)
            {
                var role = await _roleRepository.GetRoleById(user.RoleID);
                if (role == null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Token = null,
                        RefreshToken = null,
                        Message = "Role not found (Invalid RoleId)"
                    };
                }

                var token = _jwtTokenService.GenerateAccessToken(user, role.Name);
                var refreshToken = _jwtTokenService.GenerateRefreshToken(user);

                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    Message = "Login Successful",
                    UserId=user.UserID,
                    Role=role.Name,
                    User=user

                };
            }

            return new AuthResult
            {
                Success = false,
                Token = null,
                RefreshToken = null,
                Message = "Login Failed"
            };
        }

        public async Task<int> CreateUser(User user)
        {
            var hash = HashPassword(user.PasswordHash);
            user.PasswordHash = hash;

            var res = await _auth.CreateUser(user);
            return (int)res;
        }

        public async Task<AuthResult> RefreshToken(string refreshToken)
        {
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(refreshToken, validateLifetime: true);
            if (principal == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    Message = "Invalid refresh token"
                };
            }

            var userId = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new AuthResult
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    Message = "Invalid refresh token payload"
                };
            }

            var user = await _auth.GetById(int.Parse(userId));
            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    Message = "User not found"
                };
            }

            var role = await _roleRepository.GetRoleById(user.RoleID);
            if (role == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Token = null,
                    RefreshToken = null,
                    Message = "Role not found"
                };
            }

            var newAccessToken = _jwtTokenService.GenerateAccessToken(user, role.Name);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(user);

            return new AuthResult
            {
                Success = true,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Message = "Token refreshed successfully"
            };
        }
    }
}
