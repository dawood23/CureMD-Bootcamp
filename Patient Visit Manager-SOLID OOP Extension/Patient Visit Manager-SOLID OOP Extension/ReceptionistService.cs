using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class ReceptionistService : BaseRoleService
    {
        public ReceptionistService(IVisitManager visitManager, IReportGenerator reportGenerator,
            INotificationService notificationService, ILogger logger)
            : base(visitManager, reportGenerator, notificationService, logger, Roles.Receptionist)
        {
        }

        public override void ShowMenu()
        {
            _notificationService.ShowMessage("\n=== RECEPTIONIST MAIN MENU ===");
            Console.WriteLine("1. Add New Visit");
            Console.WriteLine("2. Search Visits (Filtering and Sorting)");
            Console.WriteLine("3. View All Visits");
            Console.WriteLine("4. Generate Reports");
            Console.WriteLine("5. Undo Last Action");
            Console.WriteLine("6. Redo Last Action");
            Console.WriteLine("7. Exit/Logout");
            Console.Write("Select an option (1-7): ");
        }

        public override void HandleMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    AddNewVisit();
                    break;
                case "2":
                    SearchVisits();
                    break;
                case "3":
                    ViewAllVisits();
                    break;
                case "4":
                    GenerateReports();
                    break;
                case "5":
                    _visitManager.Undo();
                    break;
                case "6":
                    _visitManager.Redo();
                    break;
                case "7":
                    _notificationService.ShowMessage("Thank you for using Patient Visit Manager!");
                    _notificationService.ShowMessage("Goodbye Receptionist!");
                    _logger.Logout(Roles.Receptionist);
                    break;
                default:
                    _notificationService.ShowError("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
