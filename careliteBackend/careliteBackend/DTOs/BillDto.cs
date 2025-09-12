namespace careliteBackend.DTOs
{
    public class Bill
    {
        public int BillID { get; set; }
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal PendingAmount { get; set; }
    }
}
