namespace careliteBackend.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; } = "Scheduled";
        public DateTime CreatedAt { get; set; }
    }
}
