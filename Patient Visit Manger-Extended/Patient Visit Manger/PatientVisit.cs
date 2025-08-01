using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class PatientVisit
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitType VisitType { get; set; }
        public string Description { get; set; }
        public string DoctorName { get; set; }

        public int VisitDuration { get; set; }

        public int Fee { get; set; }


        public PatientVisit()
        {
            PatientName = "";
            Description = "";
            DoctorName = "";
        }

        public PatientVisit(int id, string patientName, DateTime visitDate, VisitType visitType, string description, string doctorName)
        {
            Id = id;
            PatientName = patientName;
            VisitDate = visitDate;
            VisitType = visitType;
            Description = description;
            DoctorName = doctorName;
        }

        public PatientVisit(int id, string patientName, DateTime visitDate, VisitType visitType, string description, string doctorName, int duration)
        {
            Id = id;
            PatientName = patientName;
            VisitDate = visitDate;
            VisitType = visitType;
            Description = description;
            DoctorName = doctorName;
            VisitDuration = duration;
        }

        public PatientVisit(int id, string patientName, DateTime visitDate, VisitType visitType, string description, string doctorName, int duration, int fee)
        {
            Id = id;
            PatientName = patientName;
            VisitDate = visitDate;
            VisitType = visitType;
            Description = description;
            DoctorName = doctorName;
            VisitDuration = duration;
            Fee = fee;
        }

        public string ToCsvString()
        {
            return $"{Id},{PatientName},{VisitDate:yyyy-MM-dd HH:mm},{VisitType},{Description},{DoctorName},{VisitDuration},{Fee}";
        }

        public override string ToString()
        {
            return $"ID: {Id},Patient Name: {PatientName},Date: {VisitDate:yyyy-MM-dd HH:mm},Type: {VisitType},Doctor: {DoctorName},Duration: {VisitDuration},Fee: {Fee}";
        }


        public static PatientVisit FromCsvString(string csvLine)
        {
            string[] parts = csvLine.Split(',');

            if (parts.Length != 8)
                return null;

            try
            {
                return new PatientVisit
                {
                    Id = int.Parse(parts[0]),
                    PatientName = parts[1],
                    VisitDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    VisitType = (VisitType)Enum.Parse(typeof(VisitType), parts[3]),
                    Description = parts[4],
                    DoctorName = parts[5],
                    VisitDuration = int.Parse(parts[6]),
                    Fee = int.Parse(parts[7])
                };
            }
            catch
            {
                return null;
            }
        }

    }
}
