using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public enum VisitType
    {
        Consultation,
        FollowUp,
        Emergency
    }

    public enum Roles
    {
        Admin,
        Receptionist
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var application = new Application();
                application.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }

    public class Application
    {
        public INotificationService _notificationService;
        public ILogger _logger;
        public IVisitRepository _repository;
        public IVisitManager _visitManager;
        public IReportGenerator _reportGenerator;
        public AuthenticationService _authService;

        public Application()
        {
            _notificationService = new ConsoleNotificationService();
            _logger = new FileLogger();
            _repository = new CsvVisitRepository("patient_visits.csv", _notificationService);
            _visitManager = new VisitManager(_repository, _notificationService);
            _reportGenerator = new ReportGenerator(_notificationService);
            _authService = new AuthenticationService(_logger, _notificationService);
        }

        public void Run()
        {
            _notificationService.ShowMessage("=== PATIENT VISIT MANAGER ===");

            while (true)
            {
                ShowLoginMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleAdminLogin();
                        break;
                    case "2":
                        HandleReceptionistLogin();
                        break;
                    case "3":
                        _notificationService.ShowMessage("Thank you for using Patient Visit Manager!");
                        return;
                    default:
                        _notificationService.ShowError("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ShowLoginMenu()
        {
            _notificationService.ShowMessage("\nEnter the role you want to sign in with:");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Receptionist");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option (1-3): ");
        }

        private void HandleAdminLogin()
        {
            Console.Write("Enter Admin Email (admin@gmail.com): ");
            string email = Console.ReadLine();
            Console.Write("Enter Admin Password (12345): ");
            string password = Console.ReadLine();

            if (_authService.AuthenticateAdmin(email, password))
            {
                _notificationService.ShowSuccess("=== WELCOME ADMIN ===");
                var adminService = new AdminService(_visitManager, _reportGenerator, _notificationService, _logger);
                RunRoleInterface(adminService, "9");
            }
        }

        private void HandleReceptionistLogin()
        {
            Console.Write("Enter Receptionist Email (reception@gmail.com): ");
            string email = Console.ReadLine();
            Console.Write("Enter Receptionist Password (1234): ");
            string password = Console.ReadLine();

            if (_authService.AuthenticateReceptionist(email, password))
            {
                _notificationService.ShowSuccess("=== WELCOME RECEPTIONIST ===");
                var receptionistService = new ReceptionistService(_visitManager, _reportGenerator, _notificationService, _logger);
                RunRoleInterface(receptionistService, "7");
            }
        }

        private void RunRoleInterface(IRoleService roleService, string exitChoice)
        {
            while (true)
            {
                roleService.ShowMenu();
                string choice = Console.ReadLine();

                if (choice == exitChoice)
                {
                    roleService.HandleMenuChoice(choice);
                    break;
                }

                roleService.HandleMenuChoice(choice);
            }
        }
    }
}
