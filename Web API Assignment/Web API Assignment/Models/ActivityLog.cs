namespace Web_API_Assignment.Models
{
    public class ActivityLog
    {
        public int LogID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; } = "";
        public string TableAffected { get; set; } = "";
        public int? RecordID { get; set; }
        public string Status { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}
