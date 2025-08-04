using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class EmergencyVisit : BaseVisit
    {
        public override VisitType VisitType => VisitType.Emergency;
        private const int BASE_FEE_PER_MINUTE = 1000;

        public EmergencyVisit() : base() { }

        public EmergencyVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration)
            : base(id, patientName, visitDate, description, doctorName, duration)
        {
            CalculateFee();
        }

        public override void CalculateFee()
        {
            Fee = VisitDuration * BASE_FEE_PER_MINUTE;
            if (Fee < 2000)
                Fee = 2000;
        }

        public override bool ValidateVisit()
        {
            if (!base.ValidateVisit())
                return false;

            if (string.IsNullOrWhiteSpace(DoctorName))
                return false;

            return true;
        }
    }
}
