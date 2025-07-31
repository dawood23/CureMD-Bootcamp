using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text.Json;

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
        public static string AdminEmail = "admin@gmail.com";
        public static string AdminPassword = "12345";
        public static string ReceptionEmail = "reception@gmail.com";
        public static string ReceptionPassword = "1234";

        public Roles role;
        
        private static PatientVisitManager manager;

        public static void Main(string[] args)
        {
            Console.WriteLine("=== PATIENT VISIT MANAGER ===");
            Program program = new Program();
            manager = new PatientVisitManager();

           
            while (true)
            {
                Console.WriteLine("Enter the role you want to sign in with:(1 for Admin, 2 for Receptionist, 3 to exit): ");
                string choice = Console.ReadLine();

                Console.WriteLine();
                if (choice == "1")
                {
                    program.role= Roles.Admin;
                    Console.Write("Enter Admin Email(admin@gmail.com): ");
                    string em = Console.ReadLine();
                    Console.Write("Enter Admin Password(12345): ");
                    string pas=Console.ReadLine();

                    if (em != AdminEmail || pas != AdminPassword)
                    {
                        Console.WriteLine("Invalid Credentials");
                        continue;
                    }
                    else
                    {
                        initAdminInterface();
                    }
                    break;
                }
                else if (choice == "2")
                {
                    program.role = Roles.Receptionist;
                    Console.Write("Enter Receptionist Email(reception@gmail.com): ");
                    string em = Console.ReadLine();
                    Console.Write("Enter Receptionist Password(1234): ");
                    string pas = Console.ReadLine();


                    if (em != ReceptionEmail || pas != ReceptionPassword)
                    {
                        Console.WriteLine("Invalid Credentials");
                        continue;
                    }
                    else
                    {
                        initReceptionistInterface();
                    }

                    break;
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }

            }
        }
        public static void initAdminInterface()
        {
            Console.WriteLine("=====WELCOME ADMIN=====");
            ShowMainMenu();

        }

        private static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n===ADMIN MAIN MENU ===");
                Console.WriteLine("1. Add New Visit");
                Console.WriteLine("2. Update Visit");
                Console.WriteLine("3. Delete Visit");
                Console.WriteLine("4. Search Visits(Filerting and Sorting)");
                Console.WriteLine("5. View All Visits");
                Console.WriteLine("6. Generate Reports");
                Console.WriteLine("7. Undo Last Action");
                Console.WriteLine("8. Redo Last Action");
                Console.WriteLine("9. Exit");
                Console.Write("Select an option (1-9): ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    AddNewVisit();
                }
                else if (choice == "2")
                {
                    UpdateVisit();
                }
                else if (choice == "3")
                {
                    DeleteVisit();
                }
                else if (choice == "4")
                {
                    SearchVisits();
                }
                else if (choice == "5")
                {
                    ViewAllVisits();
                }
                else if (choice == "6")
                {
                    GenerateReports();
                }
                else if (choice == "7")
                {
                    manager.Undo();
                }
                else if (choice == "8")
                {
                    manager.Redo();
                }
                else if (choice == "9")
                {
                    Console.WriteLine("Thank you for using Patient Visit Manager!");
                    Console.WriteLine("Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }

        public static void initReceptionistInterface()
        {
            Console.WriteLine("=====WELCOME RECEPTIONIST=====");
            ShowReceptionistMenu();
        }
        public static void ShowReceptionistMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Add New Visit");
                Console.WriteLine("2. Search Visits(Filerting and Sorting)");
                Console.WriteLine("3. View All Visits");
                Console.WriteLine("4. Generate Reports");
                Console.WriteLine("5. Undo Last Action");
                Console.WriteLine("6. Redo Last Action");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option (1-7): ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    AddNewVisit();
                }
                else if (choice == "2")
                {
                    SearchVisits();
                }
                else if (choice == "3")
                {
                    ViewAllVisits();
                }
                else if (choice == "4")
                {
                    GenerateReports();
                }
                else if (choice == "5")
                {
                    manager.Undo();
                }
                else if (choice == "6")
                {
                    manager.Redo();
                }
                else if (choice == "7")
                {
                    Console.WriteLine("Thank you for using Patient Visit Manager!");
                    Console.WriteLine("Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }

        private static void AddNewVisit()
        {
            Console.WriteLine("\n=== ADD NEW VISIT ===");

            try
            {
                Console.Write("Patient Name: ");
                string patientName = Console.ReadLine();

                Console.Write("Visit Date (yyyy-mm-dd) [Press Enter for today]: ");
                string dateInput = Console.ReadLine();
                DateTime visitDate;

                if (string.IsNullOrEmpty(dateInput.Trim()))
                {
                    visitDate = DateTime.Now.Date;
                }
                else
                {
                    visitDate = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                Console.WriteLine("Visit Type:");
                Console.WriteLine("1. Consultation");
                Console.WriteLine("2. Follow-up");
                Console.WriteLine("3. Emergency");
                Console.Write("Select (1-3): ");
                string typeChoice = Console.ReadLine();

                VisitType visitType = VisitType.Consultation;
                if (typeChoice == "1")
                    visitType = VisitType.Consultation;
                else if (typeChoice == "2")
                    visitType = VisitType.FollowUp;
                else if (typeChoice == "3")
                    visitType = VisitType.Emergency;

                Console.Write("Description/Notes: ");
                string description = Console.ReadLine();

                Console.Write("Doctor Name (optional): ");
                string doctorName = Console.ReadLine();
                if (doctorName == null) doctorName = "";

                PatientVisit visit = new PatientVisit(0, patientName, visitDate, visitType, description, doctorName);

                manager.AddVisit(visit);

                Console.WriteLine("Visit added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding visit: {ex.Message}");
                Console.WriteLine("Please try again with valid input.");
            }
        }

        private static void UpdateVisit()
        {
            Console.WriteLine("\n=== UPDATE VISIT ===");
            Console.Write("Enter Visit ID to update: ");

            string idInput = Console.ReadLine();
            int id;

            if (int.TryParse(idInput, out id))
            {
                PatientVisit existingVisit = manager.GetVisitById(id);
                if (existingVisit != null)
                {
                    Console.WriteLine($"Current visit: {existingVisit}");
                    Console.WriteLine("Enter new information (press Enter to keep current value):");

                    Console.Write($"Patient Name [{existingVisit.PatientName}]: ");
                    string patientName = Console.ReadLine();
                    if (string.IsNullOrEmpty(patientName.Trim()))
                        patientName = existingVisit.PatientName;

                    Console.Write($"Visit Date [{existingVisit.VisitDate.ToString("yyyy-MM-dd")}]: ");
                    string dateInput = Console.ReadLine();
                    DateTime visitDate = existingVisit.VisitDate;
                    if (!string.IsNullOrEmpty(dateInput.Trim()))
                    {
                        try
                        {
                            visitDate = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid date format, keeping original date.");
                        }
                    }

                    Console.WriteLine($"Visit Type [{existingVisit.VisitType}]:");
                    Console.WriteLine("1. Consultation");
                    Console.WriteLine("2. Follow-up");
                    Console.WriteLine("3. Emergency");
                    Console.Write("Select (1-3) or press Enter to keep current: ");
                    string typeChoice = Console.ReadLine();

                    VisitType visitType = existingVisit.VisitType;
                    if (typeChoice == "1")
                        visitType = VisitType.Consultation;
                    else if (typeChoice == "2")
                        visitType = VisitType.FollowUp;
                    else if (typeChoice == "3")
                        visitType = VisitType.Emergency;

                    Console.Write($"Description [{existingVisit.Description}]: ");
                    string description = Console.ReadLine();
                    if (string.IsNullOrEmpty(description.Trim()))
                        description = existingVisit.Description;

                    Console.Write($"Doctor Name [{existingVisit.DoctorName}]: ");
                    string doctorName = Console.ReadLine();
                    if (string.IsNullOrEmpty(doctorName.Trim()))
                        doctorName = existingVisit.DoctorName;

                    PatientVisit updatedVisit = new PatientVisit(id, patientName, visitDate, visitType, description, doctorName);

                    manager.UpdateVisit(id, updatedVisit);

                    Console.WriteLine("Visit updated successfully!");
                }
                else
                {
                    Console.WriteLine("Visit not found!");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format!");
            }
        }

        private static void DeleteVisit()
        {
            Console.WriteLine("\n=== DELETE VISIT ===");
            Console.Write("Enter Visit ID to delete: ");

            string idInput = Console.ReadLine();
            int id;

            if (int.TryParse(idInput, out id))
            {
                PatientVisit visit = manager.GetVisitById(id);
                if (visit != null)
                {
                    Console.WriteLine($"Visit to delete: {visit}");
                    Console.Write("Are you sure? (y/N): ");
                    string confirmation = Console.ReadLine();

                    if (confirmation != null && confirmation.ToLower() == "y")
                    {
                        manager.DeleteVisit(id);
                        Console.WriteLine("Visit deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Delete cancelled.");
                    }
                }
                else
                {
                    Console.WriteLine("Visit not found!");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format!");
            }
        }

        private static void SearchVisits()
        {
            Console.WriteLine("\n=== SEARCH VISITS ===");
            Console.WriteLine("1. Search by Patient Name");
            Console.WriteLine("2. Search by Doctor Name");
            Console.WriteLine("3. Search by Date");
            Console.WriteLine("4. Search by Visit Type");
            Console.WriteLine("5. Sort by Date");
            Console.WriteLine("6. Sort by Name");
            Console.WriteLine("7. Sort by Doctor Name");
            Console.Write("Select search type (1-4): ");

            string choice = Console.ReadLine();
            List<PatientVisit> results = new List<PatientVisit>();

            if (choice == "1")
            {
                Console.Write("Enter patient name (partial match): ");
                string patientName = Console.ReadLine();
                if (patientName != null)
                {
                    results = manager.SearchByPatientName(patientName);
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter doctor name (partial match): ");
                string doctorName = Console.ReadLine();
                if (doctorName != null)
                {
                    results = manager.SearchByDoctorName(doctorName);
                }
            }
            else if (choice == "3")
            {
                Console.Write("Enter date (yyyy-mm-dd): ");
                string dateInput = Console.ReadLine();
                try
                {
                    DateTime searchDate = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    results = manager.SearchByDate(searchDate);
                }
                catch
                {
                    Console.WriteLine("Invalid date format!");
                    return;
                }
            }
            else if (choice == "4")
            {
                Console.WriteLine("Select visit type:");
                Console.WriteLine("1. Consultation");
                Console.WriteLine("2. Follow-up");
                Console.WriteLine("3. Emergency");
                Console.Write("Select (1-3): ");
                string typeChoice = Console.ReadLine();

                VisitType visitType = VisitType.Consultation;
                if (typeChoice == "1")
                    visitType = VisitType.Consultation;
                else if (typeChoice == "2")
                    visitType = VisitType.FollowUp;
                else if (typeChoice == "3")
                    visitType = VisitType.Emergency;

                results = manager.SearchByVisitType(visitType);
            }
            else if (choice == "5")
            {
                results = manager.SortbyDate();
            }
            else if (choice == "6")
            {
                results = manager.SortByName();
            }
            else if (choice == "7")
            {
                results = manager.SortByDoctorName();
            }
            else
            {
                Console.WriteLine("Invalid choice!");
                return;
            }

            DisplaySearchResults(results);
        }

        private static void DisplaySearchResults(List<PatientVisit> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("No visits found matching your criteria.");
            }
            else
            {
                Console.WriteLine($"\nFound {results.Count} visit(s):");
                Console.WriteLine(new string('-', 80));

                int countToShow = Math.Min(results.Count, 20);
                for (int i = 0; i < countToShow; i++)
                {
                    Console.WriteLine(results[i].ToString());
                }

                if (results.Count > 20)
                {
                    Console.WriteLine($"... and {results.Count - 20} more results.");
                }
            }
        }

        private static void ViewAllVisits()
        {
            List<PatientVisit> visits = manager.GetAllVisits();
            Console.WriteLine($"\n=== ALL VISITS ({visits.Count} total) ===");

            if (visits.Count == 0)
            {
                Console.WriteLine("No visits found.");
                return;
            }

            Console.WriteLine("Showing first 20 visits:");
            Console.WriteLine(new string('-', 80));

            int countToShow = Math.Min(visits.Count, 20);
            for (int i = 0; i < countToShow; i++)
            {
                Console.WriteLine(visits[i].ToString());
            }

            if (visits.Count > 20)
            {
                Console.WriteLine($"... and {visits.Count - 20} more visits.");
            }
        }

        private static void GenerateReports()
        {
            Console.WriteLine("\n=== REPORTS MENU ===");
            Console.WriteLine("1. Individual Visit Summary");
            Console.WriteLine("2. Visit Count by Type");
            Console.WriteLine("3. Weekly Visit Summary");
            Console.Write("Select report type (1-3): ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter Visit ID: ");
                string idInput = Console.ReadLine();
                int visitId;

                if (int.TryParse(idInput, out visitId))
                {
                    manager.GenerateVisitSummary(visitId);
                }
                else
                {
                    Console.WriteLine("Invalid ID format!");
                }
            }
            else if (choice == "2")
            {
                manager.GenerateVisitCountByType();
            }
            else if (choice == "3")
            {
                manager.GenerateWeeklySummary();
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }
    }
}



/*
static void Main()
{
    string jsonPath = "data.json";
    string jsonContent = File.ReadAllText(jsonPath);

    Dictionary<string, int> data = JsonSerializer.Deserialize<Dictionary<string, int>>(jsonContent);

    foreach (var item in data)
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }
}*/