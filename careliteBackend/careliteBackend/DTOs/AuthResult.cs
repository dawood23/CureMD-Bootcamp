using careliteBackend.Models;

namespace careliteBackend.DTOs
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; } 
        public string? Message { get; set; }
        public int UserId { get; set; } 
        public string? Role { get; set; }
        public User? User { get; set; }
    }
}
