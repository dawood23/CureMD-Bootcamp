using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public abstract class BaseRoleService : IRoleService
    {
        protected readonly IVisitManager _visitManager;
        protected readonly IReportGenerator _reportGenerator;
        protected readonly INotificationService _notificationService;
        protected readonly ILogger _logger;
        protected readonly Roles _role;

        protected BaseRoleService(IVisitManager visitManager, IReportGenerator reportGenerator,
            INotificationService notificationService, ILogger logger, Roles role)
        {
            _visitManager = visitManager;
            _reportGenerator = reportGenerator;
            _notificationService = notificationService;
            _logger = logger;
            _role = role;
        }

        public abstract void ShowMenu();
        public abstract void HandleMenuChoice(string choice);

        protected void AddNewVisit()
        {
            _notificationService.ShowMessage("\n=== ADD NEW VISIT ===");

            try
            {
                Console.Write("Patient Name: ");
                string patientName = Console.ReadLine();

                Console.Write("Visit Date (yyyy-mm-dd) [Press Enter for today]: ");
                string dateInput = Console.ReadLine();
                DateTime datePart = string.IsNullOrWhiteSpace(dateInput)
                    ? DateTime.Now.Date
                    : DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.Write("Visit Time (HH:mm, 24-hr format) [Press Enter for current time]: ");
                string timeInput = Console.ReadLine();
                TimeSpan timePart = string.IsNullOrWhiteSpace(timeInput)
                    ? DateTime.Now.TimeOfDay
                    : TimeSpan.ParseExact(timeInput, "hh\\:mm", CultureInfo.InvariantCulture);

                DateTime visitDate = datePart.Add(timePart);

                Console.WriteLine("Visit Type:");
                Console.WriteLine("1. Consultation");
                Console.WriteLine("2. Follow-up");
                Console.WriteLine("3. Emergency");
                Console.Write("Select (1-3): ");
                string typeChoice = Console.ReadLine();

                VisitType visitType = VisitType.Consultation;
                if (typeChoice == "2") visitType = VisitType.FollowUp;
                else if (typeChoice == "3") visitType = VisitType.Emergency;

                Console.Write("Description/Notes: ");
                string description = Console.ReadLine();

                Console.Write("Doctor Name: ");
                string doctorName = Console.ReadLine() ?? "";

                Console.Write("Visit Duration (in minutes): ");
                string durationInput = Console.ReadLine();
                if (!int.TryParse(durationInput, out int duration) || duration <= 0)
                {
                    _notificationService.ShowError("Invalid duration. Please enter a positive number.");
                    _logger.LogActivity(_role, $"Adding Entry for {patientName}", false);
                    return;
                }

                if (_visitManager.HasConflictingVisit(patientName, visitDate))
                {
                    bool proceed = _notificationService.ConfirmAction(
                        "Warning: This patient has another visit within 30 minutes. Proceed?");
                    if (!proceed)
                    {
                        _notificationService.ShowMessage("Visit creation cancelled.");
                        _logger.LogActivity(_role, $"Adding Entry for {patientName}", false);
                        return;
                    }
                }

                IVisit visit = visitType switch
                {
                    VisitType.Consultation => new ConsultationVisit(0, patientName, visitDate, description, doctorName, duration),
                    VisitType.FollowUp => new FollowUpVisit(0, patientName, visitDate, description, doctorName, duration),
                    VisitType.Emergency => new EmergencyVisit(0, patientName, visitDate, description, doctorName, duration),
                    _ => throw new ArgumentException("Unknown visit type")
                };

                _visitManager.AddVisit(visit);
                _logger.LogActivity(_role, $"Adding Entry for {patientName}", true);
                _notificationService.ShowSuccess("Visit added successfully!");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error adding visit: {ex.Message}");
                _notificationService.ShowMessage("Please try again with valid input.");
            }
        }

        protected void SearchVisits()
        {
            _notificationService.ShowMessage("\n=== SEARCH VISITS ===");
            Console.WriteLine("1. Search by Patient Name");
            Console.WriteLine("2. Search by Doctor Name");
            Console.WriteLine("3. Search by Date");
            Console.WriteLine("4. Search by Visit Type");
            Console.WriteLine("5. Sort by Date");
            Console.WriteLine("6. Sort by Name");
            Console.WriteLine("7. Sort by Doctor Name");
            Console.Write("Select search type (1-7): ");

            string choice = Console.ReadLine();
            List<IVisit> results = new List<IVisit>();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter patient name (partial match): ");
                        string patientName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(patientName))
                        {
                            results = _visitManager.SearchByPatientName(patientName);
                            _logger.LogActivity(_role, $"Filtered Entries by patient name: {patientName}", true);
                        }
                        break;
                    case "2":
                        Console.Write("Enter doctor name (partial match): ");
                        string doctorName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(doctorName))
                        {
                            results = _visitManager.SearchByDoctorName(doctorName);
                            _logger.LogActivity(_role, $"Filtered Entries by Doctor name: {doctorName}", true);
                        }
                        break;
                    case "3":
                        Console.Write("Enter date (yyyy-mm-dd): ");
                        string dateInput = Console.ReadLine();
                        DateTime searchDate = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        results = _visitManager.SearchByDate(searchDate);
                        _logger.LogActivity(_role, $"Filtering Entries by Date: {searchDate}", true);
                        break;
                    case "4":
                        Console.WriteLine("Select visit type:");
                        Console.WriteLine("1. Consultation");
                        Console.WriteLine("2. Follow-up");
                        Console.WriteLine("3. Emergency");
                        Console.Write("Select (1-3): ");
                        string typeChoice = Console.ReadLine();

                        VisitType visitType = typeChoice switch
                        {
                            "1" => VisitType.Consultation,
                            "2" => VisitType.FollowUp,
                            "3" => VisitType.Emergency,
                            _ => VisitType.Consultation
                        };

                        results = _visitManager.SearchByVisitType(visitType);
                        _logger.LogActivity(_role, $"Filtering Entries by Visit Type: {visitType}", true);
                        break;
                    case "5":
                        results = _visitManager.SortByDate();
                        _logger.LogActivity(_role, "Sorted Entries by Date", true);
                        break;
                    case "6":
                        results = _visitManager.SortByName();
                        _logger.LogActivity(_role, "Sorted Entries by patient name", true);
                        break;
                    case "7":
                        results = _visitManager.SortByDoctorName();
                        _logger.LogActivity(_role, "Sorted Entries by Doctor name", true);
                        break;
                    default:
                        _notificationService.ShowError("Invalid choice!");
                        _logger.LogActivity(_role, "Searching Entry", false);
                        return;
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Search error: {ex.Message}");
                _logger.LogActivity(_role, "Searching Entry", false);
                return;
            }

            DisplaySearchResults(results);
        }

        protected void DisplaySearchResults(List<IVisit> results)
        {
            if (results.Count == 0)
            {
                _notificationService.ShowMessage("No visits found matching your criteria.");
            }
            else
            {
                _notificationService.ShowMessage($"\nFound {results.Count} visit(s):");
                _notificationService.ShowMessage(new string('-', 80));

                int countToShow = Math.Min(results.Count, 20);
                for (int i = 0; i < countToShow; i++)
                {
                    _notificationService.ShowMessage(results[i].ToString());
                }

                if (results.Count > 20)
                {
                    _notificationService.ShowMessage($"... and {results.Count - 20} more results.");
                }
            }
        }

        protected void ViewAllVisits()
        {
            List<IVisit> visits = _visitManager.GetAllVisits();
            _notificationService.ShowMessage($"\n=== ALL VISITS ({visits.Count} total) ===");

            if (visits.Count == 0)
            {
                _notificationService.ShowMessage("No visits found.");
                return;
            }

            _notificationService.ShowMessage("Showing first 20 visits:");
            _notificationService.ShowMessage(new string('-', 80));

            int countToShow = Math.Min(visits.Count, 20);
            for (int i = 0; i < countToShow; i++)
            {
                _notificationService.ShowMessage(visits[i].ToString());
            }

            if (visits.Count > 20)
            {
                _notificationService.ShowMessage($"... and {visits.Count - 20} more visits.");
            }
        }

        protected void GenerateReports()
        {
            _notificationService.ShowMessage("\n=== REPORTS MENU ===");
            Console.WriteLine("1. Individual Visit Summary");
            Console.WriteLine("2. Visit Count by Type");
            Console.WriteLine("3. Weekly Visit Summary");
            Console.Write("Select report type (1-3): ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Visit ID: ");
                        string idInput = Console.ReadLine();
                        if (int.TryParse(idInput, out int visitId))
                        {
                            _reportGenerator.GenerateVisitSummary(visitId, _visitManager.GetAllVisits());
                            _logger.LogActivity(_role, $"Generating Visit Summary for id {visitId}", true);
                        }
                        else
                        {
                            _notificationService.ShowError("Invalid ID format!");
                            _logger.LogActivity(_role, "Generating Visit Summary", false);
                        }
                        break;
                    case "2":
                        _reportGenerator.GenerateVisitCountByType(_visitManager.GetAllVisits());
                        _logger.LogActivity(_role, "Generating Reports by Visit Type Count", true);
                        break;
                    case "3":
                        _reportGenerator.GenerateWeeklySummary(_visitManager.GetAllVisits());
                        _logger.LogActivity(_role, "Generating Weekly summary", true);
                        break;
                    default:
                        _notificationService.ShowError("Invalid choice!");
                        _logger.LogActivity(_role, "Generating Summary", false);
                        break;
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Report generation error: {ex.Message}");
                _logger.LogActivity(_role, "Generating Summary", false);
            }
        }
    }

}
