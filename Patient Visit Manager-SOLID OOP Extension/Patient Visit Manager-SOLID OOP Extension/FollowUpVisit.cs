using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class FollowUpVisit : BaseVisit
    {
        public override VisitType VisitType => VisitType.FollowUp;
        private const int BASE_FEE_PER_MINUTE = 300;

        public FollowUpVisit() : base() { }

        public FollowUpVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration)
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

            if (VisitDuration < 10)
                return false;

            return true;
        }
    }

}
