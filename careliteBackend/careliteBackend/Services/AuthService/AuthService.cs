using careliteBackend.DTOs;
using careliteBackend.Repository;
using careliteBackend.Services.Interfaces;
using System.Text;
using System.Security.Cryptography;
using careliteBackend.Models;

namespace careliteBackend.Services
{
    public class AuthService:IAuth
    {
        private readonly IAuthRepository _auth;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IAuthRepository auth, IJwtTokenService jwtTokenService,IRoleRepository roleRepository)
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

        public async Task<AuthResult> Authenticate(string username,string password)
        {
            var user = await _auth.GetByUsername(username);
            if (user == null) { return new AuthResult { Success = false, Token = null, Message = "Username doesn't exist" }; }
            else
            {
                var hash = HashPassword(password);

                if (user.PasswordHash == hash)
                {
                    var role = await _roleRepository.GetRoleById(user.RoleID);
                    if (role == null)
                    {
                        return new AuthResult { Success = false, Token = null, Message = "Role not found (Invalid RoleId)" };
                    }
                    var token = _jwtTokenService.GenerateToken(user, role.Name);
                    return new AuthResult { Success = true, Token = token, Message = "Login Successfull" };
                }
            }

            return new AuthResult { Success = false, Token = null, Message = "Login Failed" };
        }

        public async Task<int> CreateUser(User user)
        {
            var hash=HashPassword(user.PasswordHash);
            user.PasswordHash = hash;

            var res=await _auth.CreateUser(user);

            return (int) res;

        }

    }
}
