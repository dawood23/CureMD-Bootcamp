using System.Data;

namespace careliteBackend.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool Active { get; set; }
        public int RoleID { get; set; }
    }
}
