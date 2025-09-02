using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_API_Assignment.Models;

namespace Web_API_Assignment.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            string roleName = user.RoleID switch
            {
                1 => "Admin",
                2 => "Receptionist",
                3 => "Doctor",
                _ => "User" 
            };

            var claims = new List<Claim>
            {
                new Claim("userName", user.Username),
                new Claim("userId", user.UserID.ToString()),
                new Claim(ClaimTypes.Role, roleName) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
