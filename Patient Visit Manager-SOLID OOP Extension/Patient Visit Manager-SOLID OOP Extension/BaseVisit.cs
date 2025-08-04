using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public abstract class BaseVisit : IVisit
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Description { get; set; }
        public string DoctorName { get; set; }
        public int VisitDuration { get; set; }
        public int Fee { get; set; }
        public abstract VisitType VisitType { get; }

        protected BaseVisit()
        {
            PatientName = "";
            Description = "";
            DoctorName = "";
        }

        protected BaseVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration = 0)
        {
            Id = id;
            PatientName = patientName;
            VisitDate = visitDate;
            Description = description;
            DoctorName = doctorName;
            VisitDuration = duration;
        }

        public abstract void CalculateFee();

        public virtual bool ValidateVisit()
        {
            if (string.IsNullOrWhiteSpace(PatientName))
                return false;
            if (VisitDuration <= 0)
                return false;
            if (VisitDate == default)
                return false;
            return true;
        }

        public virtual string ToCsvString()
        {
            return $"{Id},{PatientName},{VisitDate:yyyy-MM-dd HH:mm},{VisitType},{Description},{DoctorName},{VisitDuration},{Fee}";
        }

        public override string ToString()
        {
            return $"ID: {Id}, Patient: {PatientName}, Date: {VisitDate:yyyy-MM-dd HH:mm}, Type: {VisitType}, Doctor: {DoctorName}, Duration: {VisitDuration}min, Fee: ${Fee}";
        }

        public static IVisit FromCsvString(string csvLine)
        {
            string[] parts = csvLine.Split(',');
            if (parts.Length != 8)
                return null;

            try
            {
                var visitTypeEnum = (VisitType)Enum.Parse(typeof(VisitType), parts[3]);
                var visitDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                var duration = int.Parse(parts[6]);
                var fee = int.Parse(parts[7]);

                IVisit visit = visitTypeEnum switch
                {
                    VisitType.Consultation => new ConsultationVisit(),
                    VisitType.FollowUp => new FollowUpVisit(),
                    VisitType.Emergency => new EmergencyVisit(),
                    _ => throw new ArgumentException("Unknown visit type")
                };

                visit.Id = int.Parse(parts[0]);
                visit.PatientName = parts[1];
                visit.VisitDate = visitDate;
                visit.Description = parts[4];
                visit.DoctorName = parts[5];
                visit.VisitDuration = duration;
                visit.Fee = fee;

                return visit;
            }
            catch
            {
                return null;
            }
        }
    }
}
