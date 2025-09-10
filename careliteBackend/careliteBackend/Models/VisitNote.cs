namespace careliteBackend.Models
{
    public class VisitNote
    {
        public int VisitNoteID { get; set; }
        public int AppointmentID { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
