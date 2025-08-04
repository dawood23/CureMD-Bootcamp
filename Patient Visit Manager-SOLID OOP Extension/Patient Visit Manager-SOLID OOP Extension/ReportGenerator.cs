using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly INotificationService _notificationService;

        public ReportGenerator(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void GenerateVisitSummary(int visitId, List<IVisit> visits)
        {
            var visit = visits.FirstOrDefault(v => v.Id == visitId);

            if (visit != null)
            {
                _notificationService.ShowMessage("\n=== VISIT SUMMARY ===");
                _notificationService.ShowMessage($"Visit ID: {visit.Id}");
                _notificationService.ShowMessage($"Patient Name: {visit.PatientName}");
                _notificationService.ShowMessage($"Visit Date: {visit.VisitDate:yyyy-MM-dd}");
                _notificationService.ShowMessage($"Visit Type: {visit.VisitType}");
                _notificationService.ShowMessage($"Description: {visit.Description}");
                _notificationService.ShowMessage($"Doctor: {visit.DoctorName}");
                _notificationService.ShowMessage($"Duration: {visit.VisitDuration} minutes");
                _notificationService.ShowMessage($"Fee: ${visit.Fee}");
                _notificationService.ShowMessage("=====================");
            }
            else
            {
                _notificationService.ShowError("Visit not found!");
            }
        }

        public void GenerateVisitCountByType(List<IVisit> visits)
        {
            _notificationService.ShowMessage("\n=== VISIT COUNT BY TYPE ===");

            var consultationCount = visits.Count(v => v.VisitType == VisitType.Consultation);
            var followUpCount = visits.Count(v => v.VisitType == VisitType.FollowUp);
            var emergencyCount = visits.Count(v => v.VisitType == VisitType.Emergency);

            _notificationService.ShowMessage($"Consultation: {consultationCount} visits");
            _notificationService.ShowMessage($"Follow-up: {followUpCount} visits");
            _notificationService.ShowMessage($"Emergency: {emergencyCount} visits");
            _notificationService.ShowMessage("===========================");
        }

        public void GenerateWeeklySummary(List<IVisit> visits)
        {
            DateTime today = DateTime.Now.Date;
            DateTime weekStart = today.AddDays(-(int)today.DayOfWeek);
            DateTime weekEnd = weekStart.AddDays(6);

            var weeklyVisits = visits.Where(v => v.VisitDate >= weekStart && v.VisitDate <= weekEnd).ToList();

            _notificationService.ShowMessage($"\n=== WEEKLY SUMMARY ({weekStart:yyyy-MM-dd} to {weekEnd:yyyy-MM-dd}) ===");
            _notificationService.ShowMessage($"Total visits this week: {weeklyVisits.Count}");

            for (DateTime day = weekStart; day <= weekEnd; day = day.AddDays(1))
            {
                var dayCount = weeklyVisits.Count(v => v.VisitDate.Date == day.Date);
                if (dayCount > 0)
                {
                    _notificationService.ShowMessage($"{day:yyyy-MM-dd}: {dayCount} visits");
                }
            }
            _notificationService.ShowMessage("===========================================");
        }
    }
}
