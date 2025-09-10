namespace careliteBackend.DTOs
{
    public class VisitDto
    {
        public int VisitNoteID { get; set; }
        public int AppointmentID { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string AppointmentStatus { get; set; } = string.Empty;
    }
}
