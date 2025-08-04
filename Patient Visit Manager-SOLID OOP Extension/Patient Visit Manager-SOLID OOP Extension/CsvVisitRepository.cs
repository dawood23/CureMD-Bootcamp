using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class CsvVisitRepository : IVisitRepository
    {
        private readonly string _filePath;
        private readonly INotificationService _notificationService;

        public CsvVisitRepository(string filePath, INotificationService notificationService)
        {
            _filePath = filePath;
            _notificationService = notificationService;
        }

        public void SaveVisits(List<IVisit> visits)
        {
            try
            {
                var lines = new List<string>
                {
                    "Id,PatientName,VisitDate,VisitType,Description,DoctorName,VisitDuration,Fee"
                };

                lines.AddRange(visits.Select(visit => visit.ToCsvString()));
                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error saving data: {ex.Message}");
            }
        }

        public List<IVisit> LoadVisits()
        {
            var visits = new List<IVisit>();

            try
            {
                if (!File.Exists(_filePath))
                    return visits;

                string[] lines = File.ReadAllLines(_filePath);

                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrEmpty(lines[i].Trim()))
                    {
                        IVisit visit = BaseVisit.FromCsvString(lines[i]);
                        if (visit != null)
                        {
                            visits.Add(visit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error loading data: {ex.Message}");
            }

            return visits;
        }

        public bool FileExists()
        {
            return File.Exists(_filePath);
        }
    }
}
