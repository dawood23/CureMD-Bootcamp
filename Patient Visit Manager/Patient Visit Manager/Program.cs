using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace PatientVisitManager
{
    public enum VisitType
    {
        Consultation,
        FollowUp,
        Emergency
    }

    public class PatientVisit
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitType VisitType { get; set; }
        public string Description { get; set; }
        public string DoctorName { get; set; }

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

        public override string ToString()
        {
            return $"ID: {Id}, Patient: {PatientName}, Date: {VisitDate.ToString("yyyy-MM-dd")}, Type: {VisitType}, Doctor: {DoctorName}";
        }

        public string ToCsvString()
        {
            return $"{Id},{PatientName},{VisitDate.ToString("yyyy-MM-dd")},{VisitType},{Description},{DoctorName}";
        }

        public static PatientVisit FromCsvString(string csvLine)
        {
            string[] parts = csvLine.Split(',');

            if (parts.Length != 6)
                return null;

            try
            {
                PatientVisit visit = new PatientVisit();
                visit.Id = int.Parse(parts[0]);
                visit.PatientName = parts[1];
                visit.VisitDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                visit.VisitType = (VisitType)Enum.Parse(typeof(VisitType), parts[3]);
                visit.Description = parts[4];
                visit.DoctorName = parts[5];
                return visit;
            }
            catch
            {
                return null;
            }
        }
    }

    public class UndoRedoAction
    {
        public string ActionType { get; set; }
        public PatientVisit VisitData { get; set; }
        public PatientVisit OldVisitData { get; set; }
        public string Description { get; set; }

        public UndoRedoAction(string actionType, PatientVisit visitData, string description)
        {
            ActionType = actionType;
            VisitData = visitData;
            Description = description;
        }

        public UndoRedoAction(string actionType, PatientVisit visitData, PatientVisit oldVisitData, string description)
        {
            ActionType = actionType;
            VisitData = visitData;
            OldVisitData = oldVisitData;
            Description = description;
        }
    }

    public class PatientVisitManager
    {
        private List<PatientVisit> visits;
        private string dataFilePath;
        private int nextId;
        private Stack<UndoRedoAction> undoStack;
        private Stack<UndoRedoAction> redoStack;
        private const int MAX_UNDO_ACTIONS = 10;

        public PatientVisitManager(string filePath = "patient_visits.csv")
        {
            visits = new List<PatientVisit>();
            dataFilePath = filePath;
            nextId = 1;
            undoStack = new Stack<UndoRedoAction>();
            redoStack = new Stack<UndoRedoAction>();

            LoadDataFromFile();
        }

        private void LoadDataFromFile()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    Console.WriteLine("Loading data from file...");
                    string[] lines = File.ReadAllLines(dataFilePath);

                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(lines[i].Trim()))
                        {
                            PatientVisit visit = PatientVisit.FromCsvString(lines[i]);
                            if (visit != null)
                            {
                                visits.Add(visit);
                                if (visit.Id >= nextId)
                                    nextId = visit.Id + 1;
                            }
                        }
                    }
                    Console.WriteLine($"Loaded {visits.Count} visits from file.");
                }
                else
                {
                    Console.WriteLine("No existing data file found. Creating sample data...");
                    CreateSampleData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                Console.WriteLine("Creating sample data instead...");
                CreateSampleData();
            }
        }

        private void SaveDataToFile()
        {
            try
            {
                List<string> lines = new List<string>();

                lines.Add("Id,PatientName,VisitDate,VisitType,Description,DoctorName");

                foreach (PatientVisit visit in visits)
                {
                    lines.Add(visit.ToCsvString());
                }

                File.WriteAllLines(dataFilePath, lines);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private void CreateSampleData()
        {
            string[] patientNames = {
                "John Smith", "Jane Doe", "Robert Johnson", "Emily Davis", "Michael Brown",
                "Sarah Wilson", "David Miller", "Lisa Anderson", "James Taylor", "Maria Garcia"
            };

            string[] doctorNames = {
                "Dr. Anderson", "Dr. Smith", "Dr. Johnson", "Dr. Williams",
                "Dr. Brown", "Dr. Davis", "Dr. Miller", "Dr. Wilson"
            };

            string[] descriptions = {
                "Routine checkup", "Follow-up examination", "Emergency treatment",
                "Consultation", "Blood pressure check", "Vaccination", "Prescription renewal",
                "Physical therapy", "Lab results review", "Specialist referral"
            };

            Random random = new Random();

            for (int i = 0; i < 400; i++)
            {
                PatientVisit visit = new PatientVisit();
                visit.Id = nextId++;
                visit.PatientName = patientNames[random.Next(patientNames.Length)];
                visit.VisitDate = DateTime.Now.AddDays(-random.Next(365)).Date;
                visit.VisitType = (VisitType)random.Next(3);
                visit.Description = descriptions[random.Next(descriptions.Length)];
                visit.DoctorName = doctorNames[random.Next(doctorNames.Length)];

                visits.Add(visit);
            }

            SaveDataToFile();
            Console.WriteLine("Created 400 sample patient visits for testing.");
        }

        private void AddUndoAction(UndoRedoAction action)
        {
            undoStack.Push(action);
            redoStack.Clear();

            if (undoStack.Count > MAX_UNDO_ACTIONS)
            {
                Stack<UndoRedoAction> tempStack = new Stack<UndoRedoAction>();

                for (int i = 0; i < MAX_UNDO_ACTIONS; i++)
                {
                    tempStack.Push(undoStack.Pop());
                }

                undoStack.Clear();

                while (tempStack.Count > 0)
                {
                    undoStack.Push(tempStack.Pop());
                }
            }

            Console.WriteLine($"*** Action completed: {action.Description} ***");
        }

        public void AddVisit(PatientVisit visit)
        {
            visit.Id = nextId++;

            visits.Add(visit);

            SaveDataToFile();

            UndoRedoAction action = new UndoRedoAction("ADD", visit, $"Add visit for {visit.PatientName}");
            AddUndoAction(action);
        }

        public void UpdateVisit(int id, PatientVisit updatedVisit)
        {
            PatientVisit existingVisit = null;
            int index = -1;

            for (int i = 0; i < visits.Count; i++)
            {
                if (visits[i].Id == id)
                {
                    existingVisit = visits[i];
                    index = i;
                    break;
                }
            }

            if (existingVisit != null)
            {
                PatientVisit oldVisit = new PatientVisit(
                    existingVisit.Id, existingVisit.PatientName, existingVisit.VisitDate,
                    existingVisit.VisitType, existingVisit.Description, existingVisit.DoctorName
                );

                updatedVisit.Id = id;
                visits[index] = updatedVisit;

                SaveDataToFile();

                UndoRedoAction action = new UndoRedoAction("UPDATE", updatedVisit, oldVisit, $"Update visit for {updatedVisit.PatientName}");
                AddUndoAction(action);
            }
            else
            {
                Console.WriteLine("Visit not found!");
            }
        }

        public void DeleteVisit(int id)
        {
            PatientVisit visitToDelete = null;

            for (int i = 0; i < visits.Count; i++)
            {
                if (visits[i].Id == id)
                {
                    visitToDelete = visits[i];
                    break;
                }
            }

            if (visitToDelete != null)
            {
                PatientVisit deletedVisit = new PatientVisit(
                    visitToDelete.Id, visitToDelete.PatientName, visitToDelete.VisitDate,
                    visitToDelete.VisitType, visitToDelete.Description, visitToDelete.DoctorName
                );

                visits.Remove(visitToDelete);

                SaveDataToFile();

                UndoRedoAction action = new UndoRedoAction("DELETE", deletedVisit, $"Delete visit for {deletedVisit.PatientName}");
                AddUndoAction(action);
            }
            else
            {
                Console.WriteLine("Visit not found!");
            }
        }

        public List<PatientVisit> SearchByPatientName(string name)
        {
            List<PatientVisit> results = new List<PatientVisit>();

            foreach (PatientVisit visit in visits)
            {
                if (visit.PatientName.ToLower().Contains(name.ToLower()))
                {
                    results.Add(visit);
                }
            }

            return results;
        }

        public List<PatientVisit> SearchByDoctorName(string doctorName)
        {
            List<PatientVisit> results = new List<PatientVisit>();

            foreach (PatientVisit visit in visits)
            {
                if (visit.DoctorName.ToLower().Contains(doctorName.ToLower()))
                {
                    results.Add(visit);
                }
            }

            return results;
        }

        public List<PatientVisit> SearchByDate(DateTime date)
        {
            List<PatientVisit> results = new List<PatientVisit>();

            foreach (PatientVisit visit in visits)
            {
                if (visit.VisitDate.Date == date.Date)
                {
                    results.Add(visit);
                }
            }

            return results;
        }

        public List<PatientVisit> SearchByVisitType(VisitType type)
        {
            List<PatientVisit> results = new List<PatientVisit>();

            foreach (PatientVisit visit in visits)
            {
                if (visit.VisitType == type)
                {
                    results.Add(visit);
                }
            }

            return results;
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                UndoRedoAction action = undoStack.Pop();

                if (action.ActionType == "ADD")
                {
                    visits.RemoveAll(v => v.Id == action.VisitData.Id);
                }
                else if (action.ActionType == "DELETE")
                {
                    visits.Add(action.VisitData);
                }
                else if (action.ActionType == "UPDATE")
                {
                    for (int i = 0; i < visits.Count; i++)
                    {
                        if (visits[i].Id == action.VisitData.Id)
                        {
                            visits[i] = action.OldVisitData;
                            break;
                        }
                    }
                }

                SaveDataToFile();

                redoStack.Push(action);

                Console.WriteLine($"*** Undone: {action.Description} ***");
            }
            else
            {
                Console.WriteLine("Nothing to undo.");
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                UndoRedoAction action = redoStack.Pop();

                if (action.ActionType == "ADD")
                {
                    visits.Add(action.VisitData);
                }
                else if (action.ActionType == "DELETE")
                {
                    visits.RemoveAll(v => v.Id == action.VisitData.Id);
                }
                else if (action.ActionType == "UPDATE")
                {
                    for (int i = 0; i < visits.Count; i++)
                    {
                        if (visits[i].Id == action.VisitData.Id)
                        {
                            visits[i] = action.VisitData;
                            break;
                        }
                    }
                }

                SaveDataToFile();

                undoStack.Push(action);

                Console.WriteLine($"*** Redone: {action.Description} ***");
            }
            else
            {
                Console.WriteLine("Nothing to redo.");
            }
        }

        public List<PatientVisit> GetAllVisits()
        {
            return visits;
        }

        public PatientVisit GetVisitById(int id)
        {
            foreach (PatientVisit visit in visits)
            {
                if (visit.Id == id)
                {
                    return visit;
                }
            }
            return null;
        }

        public void GenerateVisitSummary(int visitId)
        {
            PatientVisit visit = GetVisitById(visitId);

            if (visit != null)
            {
                Console.WriteLine("\n=== VISIT SUMMARY ===");
                Console.WriteLine($"Visit ID: {visit.Id}");
                Console.WriteLine($"Patient Name: {visit.PatientName}");
                Console.WriteLine($"Visit Date: {visit.VisitDate.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Visit Type: {visit.VisitType}");
                Console.WriteLine($"Description: {visit.Description}");
                Console.WriteLine($"Doctor: {visit.DoctorName}");
                Console.WriteLine("=====================");
            }
            else
            {
                Console.WriteLine("Visit not found!");
            }
        }

        public void GenerateVisitCountByType()
        {
            Console.WriteLine("\n=== VISIT COUNT BY TYPE ===");

            int consultationCount = 0;
            int followUpCount = 0;
            int emergencyCount = 0;

            foreach (PatientVisit visit in visits)
            {
                if (visit.VisitType == VisitType.Consultation)
                    consultationCount++;
                else if (visit.VisitType == VisitType.FollowUp)
                    followUpCount++;
                else if (visit.VisitType == VisitType.Emergency)
                    emergencyCount++;
            }

            Console.WriteLine($"Consultation: {consultationCount} visits");
            Console.WriteLine($"Follow-up: {followUpCount} visits");
            Console.WriteLine($"Emergency: {emergencyCount} visits");
            Console.WriteLine("===========================");
        }

        public void GenerateWeeklySummary()
        {
            DateTime today = DateTime.Now.Date;
            DateTime weekStart = today.AddDays(-(int)today.DayOfWeek);
            DateTime weekEnd = weekStart.AddDays(6);

            List<PatientVisit> weeklyVisits = new List<PatientVisit>();
            foreach (PatientVisit visit in visits)
            {
                if (visit.VisitDate >= weekStart && visit.VisitDate <= weekEnd)
                {
                    weeklyVisits.Add(visit);
                }
            }

            Console.WriteLine($"\n=== WEEKLY SUMMARY ({weekStart.ToString("yyyy-MM-dd")} to {weekEnd.ToString("yyyy-MM-dd")}) ===");
            Console.WriteLine($"Total visits this week: {weeklyVisits.Count}");

            for (DateTime day = weekStart; day <= weekEnd; day = day.AddDays(1))
            {
                int dayCount = 0;
                foreach (PatientVisit visit in weeklyVisits)
                {
                    if (visit.VisitDate.Date == day.Date)
                        dayCount++;
                }

                if (dayCount > 0)
                {
                    Console.WriteLine($"{day.ToString("yyyy-MM-dd")}: {dayCount} visits");
                }
            }
            Console.WriteLine("===========================================");
        }
    }

    public class Program
    {
        private static PatientVisitManager manager;

        public static void Main(string[] args)
        {
            Console.WriteLine("=== PATIENT VISIT MANAGER ===");
            Console.WriteLine("Welcome to my first C# project!");
            Console.WriteLine("Initializing system...\n");

            manager = new PatientVisitManager();

            ShowMainMenu();
        }

        private static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Add New Visit");
                Console.WriteLine("2. Update Visit");
                Console.WriteLine("3. Delete Visit");
                Console.WriteLine("4. Search Visits");
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
