namespace careliteBackend.DTOs
{
    public class WeeklyCalendarDto
    {
        public int AppointmentID { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
