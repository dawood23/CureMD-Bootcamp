using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class ConsultationVisit : BaseVisit
    {
        public override VisitType VisitType => VisitType.Consultation;
        private const int BASE_FEE_PER_MINUTE = 500;

        public ConsultationVisit() : base() { }

        public ConsultationVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration)
            : base(id, patientName, visitDate, description, doctorName, duration)
        {
            CalculateFee();
        }

        public override void CalculateFee()
        {
            Fee = VisitDuration * BASE_FEE_PER_MINUTE;
        }

        public override bool ValidateVisit()
        {
            if (!base.ValidateVisit())
                return false;

            if (VisitDuration < 15)
                return false;

            return true;
        }
    }
}
