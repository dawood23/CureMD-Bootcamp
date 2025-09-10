namespace careliteBackend.DTOs
{
    public class CreateVisitRequest
    {
        public int AppointmentID { get; set; }
        public string Content { get; set;}=string.Empty;
    }
}
