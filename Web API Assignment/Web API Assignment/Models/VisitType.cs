namespace Web_API_Assignment.Models
{
    public class VisitType
    {
        public int VisitTypeID { get; set; }
        public string TypeName { get; set; } = "";
        public decimal BaseFee { get; set; }
        public int EstimatedDuration { get; set; } 
    }
}
