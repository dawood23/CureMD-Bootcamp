namespace Web_API_Assignment.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int RoleID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
