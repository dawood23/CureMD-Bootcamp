using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public interface IVisit
    {
        int Id { get; set; }
        string PatientName { get; set; }
        DateTime VisitDate { get; set; }
        string Description { get; set; }
        string DoctorName { get; set; }
        int VisitDuration { get; set; }
        int Fee { get; set; }
        VisitType VisitType { get; }

        void CalculateFee();
        bool ValidateVisit();
        string ToCsvString();
    }
}
