namespace Web_API_Assignment.Models
{
    public class Visit
    {
        public int VisitID { get; set; }
        public int PatientID { get; set; }
        public int? DoctorID { get; set; }
        public int VisitTypeID { get; set; }
        public DateTime VisitDate { get; set; }    
        public TimeSpan VisitTime { get; set; }     
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Scheduled"; 
        public decimal? Fee { get; set; }
        public int CreatedBy { get; set; } 
    }
}
