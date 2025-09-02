namespace Web_API_Assignment.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
