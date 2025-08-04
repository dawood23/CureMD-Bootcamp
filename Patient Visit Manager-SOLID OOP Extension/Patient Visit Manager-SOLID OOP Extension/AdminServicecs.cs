using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class AdminService : BaseRoleService
    {
        public AdminService(IVisitManager visitManager, IReportGenerator reportGenerator,
            INotificationService notificationService, ILogger logger)
            : base(visitManager, reportGenerator, notificationService, logger, Roles.Admin)
        {
        }

        public override void ShowMenu()
        {
            _notificationService.ShowMessage("\n=== ADMIN MAIN MENU ===");
            Console.WriteLine("1. Add New Visit");
            Console.WriteLine("2. Update Visit");
            Console.WriteLine("3. Delete Visit");
            Console.WriteLine("4. Search Visits (Filtering and Sorting)");
            Console.WriteLine("5. View All Visits");
            Console.WriteLine("6. Generate Reports");
            Console.WriteLine("7. Undo Last Action");
            Console.WriteLine("8. Redo Last Action");
            Console.WriteLine("9. Exit/Logout");
            Console.Write("Select an option (1-9): ");
        }

        public override void HandleMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    AddNewVisit();
                    break;
                case "2":
                    UpdateVisit();
                    break;
                case "3":
                    DeleteVisit();
                    break;
                case "4":
                    SearchVisits();
                    break;
                case "5":
                    ViewAllVisits();
                    break;
                case "6":
                    GenerateReports();
                    break;
                case "7":
                    _visitManager.Undo();
                    break;
                case "8":
                    _visitManager.Redo();
                    break;
                case "9":
                    _notificationService.ShowMessage("Thank you for using Patient Visit Manager!");
                    _notificationService.ShowMessage("Goodbye Admin!");
                    _logger.Logout(Roles.Admin);
                    break;
                default:
                    _notificationService.ShowError("Invalid option. Please try again.");
                    break;
            }
        }

        private void UpdateVisit()
        {
            _notificationService.ShowMessage("\n=== UPDATE VISIT ===");
            Console.Write("Enter Visit ID to update: ");

            string idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int id))
            {
                _notificationService.ShowError("Invalid ID format!");
                return;
            }

            IVisit existingVisit = _visitManager.GetVisitById(id);
            if (existingVisit == null)
            {
                _notificationService.ShowError("Visit not found!");
                _logger.LogActivity(_role, "Updating a visit", false);
                return;
            }

            try
            {
                _notificationService.ShowMessage("Enter new information (press Enter to keep current value):");

                Console.Write($"Patient Name [{existingVisit.PatientName}]: ");
                string patientName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(patientName))
                    patientName = existingVisit.PatientName;

                Console.Write($"Visit Date [{existingVisit.VisitDate:yyyy-MM-dd}]: ");
                string dateInput = Console.ReadLine();

                Console.Write($"Visit Time [{existingVisit.VisitDate:HH:mm}] (24-hr format): ");
                string timeInput = Console.ReadLine();

                DateTime visitDate = existingVisit.VisitDate;
                if (!string.IsNullOrWhiteSpace(dateInput) || !string.IsNullOrWhiteSpace(timeInput))
                {
                    try
                    {
                        string dateStr = string.IsNullOrWhiteSpace(dateInput) ? existingVisit.VisitDate.ToString("yyyy-MM-dd") : dateInput;
                        string timeStr = string.IsNullOrWhiteSpace(timeInput) ? existingVisit.VisitDate.ToString("HH:mm") : timeInput;
                        visitDate = DateTime.ParseExact($"{dateStr} {timeStr}", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        _notificationService.ShowWarning("Invalid date/time format, keeping original.");
                    }
                }

                Console.WriteLine($"Visit Type [{existingVisit.VisitType}]:");
                Console.WriteLine("1. Consultation");
                Console.WriteLine("2. Follow-up");
                Console.WriteLine("3. Emergency");
                Console.Write("Select (1-3) or press Enter to keep current: ");
                string typeChoice = Console.ReadLine();

                VisitType visitType = existingVisit.VisitType;
                if (typeChoice == "1") visitType = VisitType.Consultation;
                else if (typeChoice == "2") visitType = VisitType.FollowUp;
                else if (typeChoice == "3") visitType = VisitType.Emergency;

                Console.Write($"Description [{existingVisit.Description}]: ");
                string description = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description))
                    description = existingVisit.Description;

                Console.Write($"Doctor Name [{existingVisit.DoctorName}]: ");
                string doctorName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(doctorName))
                    doctorName = existingVisit.DoctorName;

                Console.Write($"Visit Duration [{existingVisit.VisitDuration}]: ");
                string durationInput = Console.ReadLine();
                int duration = existingVisit.VisitDuration;
                if (!string.IsNullOrWhiteSpace(durationInput))
                    int.TryParse(durationInput, out duration);

                IVisit updatedVisit = visitType switch
                {
                    VisitType.Consultation => new ConsultationVisit(id, patientName, visitDate, description, doctorName, duration),
                    VisitType.FollowUp => new FollowUpVisit(id, patientName, visitDate, description, doctorName, duration),
                    VisitType.Emergency => new EmergencyVisit(id, patientName, visitDate, description, doctorName, duration),
                    _ => throw new ArgumentException("Unknown visit type")
                };

                _visitManager.UpdateVisit(id, updatedVisit);
                _logger.LogActivity(_role, $"Updating Entry for {updatedVisit.PatientName}. Visit id: {updatedVisit.Id}", true);
                _notificationService.ShowSuccess("Visit updated successfully!");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error updating visit: {ex.Message}");
                _logger.LogActivity(_role, "Updating a visit", false);
            }
        }

        private void DeleteVisit()
        {
            _notificationService.ShowMessage("\n=== DELETE VISIT ===");
            Console.Write("Enter Visit ID to delete: ");

            string idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int id))
            {
                _notificationService.ShowError("Invalid ID format!");
                return;
            }

            IVisit visit = _visitManager.GetVisitById(id);
            if (visit == null)
            {
                _notificationService.ShowError("Visit not found!");
                _logger.LogActivity(_role, $"Deleting Entry for visit id: {id}", false);
                return;
            }

            _notificationService.ShowMessage($"Visit to delete: {visit}");
            bool confirmed = _notificationService.ConfirmAction("Are you sure?");

            if (confirmed)
            {
                _visitManager.DeleteVisit(id);
                _logger.LogActivity(_role, $"Deleting Entry for {visit.PatientName}. Visit id: {id}", true);
                _notificationService.ShowSuccess("Visit deleted successfully!");
            }
            else
            {
                _notificationService.ShowMessage("Delete cancelled.");
            }
        }
    }
}
