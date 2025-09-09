namespace careliteBackend.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; } = "Scheduled";
        public DateTime CreatedAt { get; set; }
    }
}
