namespace careliteBackend.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = null!;
        public string? Specialization { get; set; }
    }
}
