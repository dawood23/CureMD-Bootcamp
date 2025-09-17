using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using careliteBackend.Models;

namespace careliteBackend.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user, string role);
        string GenerateRefreshToken(User user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool validateLifetime = false);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(User user, string role)
        {
            var claims = new List<Claim>
        {
            new Claim("name", user.Username),
            new Claim("userId", user.UserID.ToString()),
            new Claim("role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            return BuildToken(claims, minutes: double.Parse(_config["JwtSettings:ExpiresInMinutes"]!));
        }

        public string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim("userId", user.UserID.ToString()),
            new Claim("type", "refresh")
        };

            return BuildToken(claims, minutes: double.Parse(_config["JwtSettings:RefreshExpiresInMinutes"]!));
        }

        private string BuildToken(IEnumerable<Claim> claims, double minutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool validateLifetime = false)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!)),
                ValidateLifetime = validateLifetime 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return principal;
        }
    }

}
