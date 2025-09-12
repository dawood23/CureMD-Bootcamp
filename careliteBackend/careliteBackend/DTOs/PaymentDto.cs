namespace careliteBackend.DTOs
{
    public class PaymentDto
    {
        public int PaymentID { get; set; }
        public int BillID { get; set; }
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; }
        public int RecordedBy { get; set; }
    }

}
