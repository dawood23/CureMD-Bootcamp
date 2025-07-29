using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

// Enums
public enum VisitType
{
    Consultation,
    FollowUp,
    Emergency
}

// Models
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
        PatientName = string.Empty;
        Description = string.Empty;
        DoctorName = string.Empty;
    }

    public PatientVisit(int id, string patientName, DateTime visitDate, VisitType visitType, string description, string doctorName = "")
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
        return $"ID: {Id}, Patient: {PatientName}, Date: {VisitDate:yyyy-MM-dd}, Type: {VisitType}, Doctor: {DoctorName}";
    }

    public string ToCsvString()
    {
        return $"{Id},{PatientName},{VisitDate:yyyy-MM-dd},{VisitType},{Description},{DoctorName}";
    }

    public static PatientVisit FromCsvString(string csvLine)
    {
        var parts = csvLine.Split(',');
        if (parts.Length != 6) return null;

        return new PatientVisit
        {
            Id = int.Parse(parts[0]),
            PatientName = parts[1],
            VisitDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
            VisitType = Enum.Parse<VisitType>(parts[3]),
            Description = parts[4],
            DoctorName = parts[5]
        };
    }
}

// Command Pattern for Undo/Redo
public interface ICommand
{
    void Execute();
    void Undo();
    string Description { get; }
}

public class AddVisitCommand : ICommand
{
    private readonly PatientVisitManager _manager;
    private readonly PatientVisit _visit;
    public string Description { get; }

    public AddVisitCommand(PatientVisitManager manager, PatientVisit visit)
    {
        _manager = manager;
        _visit = visit;
        Description = $"Add visit for {visit.PatientName}";
    }

    public void Execute()
    {
        _manager.AddVisitInternal(_visit);
    }

    public void Undo()
    {
        _manager.DeleteVisitInternal(_visit.Id);
    }
}

public class UpdateVisitCommand : ICommand
{
    private readonly PatientVisitManager _manager;
    private readonly PatientVisit _oldVisit;
    private readonly PatientVisit _newVisit;
    public string Description { get; }

    public UpdateVisitCommand(PatientVisitManager manager, PatientVisit oldVisit, PatientVisit newVisit)
    {
        _manager = manager;
        _oldVisit = new PatientVisit(oldVisit.Id, oldVisit.PatientName, oldVisit.VisitDate, oldVisit.VisitType, oldVisit.Description, oldVisit.DoctorName);
        _newVisit = newVisit;
        Description = $"Update visit for {newVisit.PatientName}";
    }

    public void Execute()
    {
        _manager.UpdateVisitInternal(_newVisit);
    }

    public void Undo()
    {
        _manager.UpdateVisitInternal(_oldVisit);
    }
}

public class DeleteVisitCommand : ICommand
{
    private readonly PatientVisitManager _manager;
    private readonly PatientVisit _visit;
    public string Description { get; }

    public DeleteVisitCommand(PatientVisitManager manager, PatientVisit visit)
    {
        _manager = manager;
        _visit = new PatientVisit(visit.Id, visit.PatientName, visit.VisitDate, visit.VisitType, visit.Description, visit.DoctorName);
        Description = $"Delete visit for {visit.PatientName}";
    }

    public void Execute()
    {
        _manager.DeleteVisitInternal(_visit.Id);
    }

    public void Undo()
    {
        _manager.AddVisitInternal(_visit);
    }
}

// Main Manager Class
public class PatientVisitManager
{
    private List<PatientVisit> _visits;
    private readonly string _dataFilePath;
    private int _nextId;
    private Stack<ICommand> _undoStack;
    private Stack<ICommand> _redoStack;
    private const int MaxUndoActions = 10;

    public PatientVisitManager(string dataFilePath = "patient_visits.csv")
    {
        _visits = new List<PatientVisit>();
        _dataFilePath = dataFilePath;
        _undoStack = new Stack<ICommand>();
        _redoStack = new Stack<ICommand>();
        _nextId = 1;
        LoadData();
    }

    // File Operations
    private void LoadData()
    {
        try
        {
            if (File.Exists(_dataFilePath))
            {
                var lines = File.ReadAllLines(_dataFilePath);
                foreach (var line in lines.Skip(1)) // Skip header
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var visit = PatientVisit.FromCsvString(line);
                        if (visit != null)
                        {
                            _visits.Add(visit);
                            if (visit.Id >= _nextId)
                                _nextId = visit.Id + 1;
                        }
                    }
                }
                Console.WriteLine($"Loaded {_visits.Count} visits from file.");
            }
            else
            {
                GenerateMockData();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            GenerateMockData();
        }
    }

    private void SaveData()
    {
        try
        {
            var lines = new List<string> { "Id,PatientName,VisitDate,VisitType,Description,DoctorName" };
            lines.AddRange(_visits.Select(v => v.ToCsvString()));
            File.WriteAllLines(_dataFilePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    // Mock Data Generation
    private void GenerateMockData()
    {
        var random = new Random();
        var patientNames = new[] { "John Smith", "Jane Doe", "Robert Johnson", "Emily Davis", "Michael Brown", "Sarah Wilson", "David Miller", "Lisa Anderson", "James Taylor", "Maria Garcia" };
        var doctorNames = new[] { "Dr. Anderson", "Dr. Smith", "Dr. Johnson", "Dr. Williams", "Dr. Brown", "Dr. Davis", "Dr. Miller", "Dr. Wilson" };
        var descriptions = new[] { "Routine checkup", "Follow-up examination", "Emergency treatment", "Consultation", "Blood pressure check", "Vaccination", "Prescription renewal", "Physical therapy", "Lab results review", "Specialist referral" };

        for (int i = 0; i < 400; i++)
        {
            var visit = new PatientVisit
            {
                Id = _nextId++,
                PatientName = patientNames[random.Next(patientNames.Length)],
                VisitDate = DateTime.Now.AddDays(-random.Next(365)).Date,
                VisitType = (VisitType)random.Next(3),
                Description = descriptions[random.Next(descriptions.Length)],
                DoctorName = doctorNames[random.Next(doctorNames.Length)]
            };
            _visits.Add(visit);
        }

        SaveData();
        Console.WriteLine("Generated 400 mock patient visits for testing.");
    }

    // Command Management
    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear(); // Clear redo stack when new command is executed

        // Maintain max undo history
        if (_undoStack.Count > MaxUndoActions)
        {
            var tempStack = new Stack<ICommand>();
            for (int i = 0; i < MaxUndoActions; i++)
            {
                tempStack.Push(_undoStack.Pop());
            }
            _undoStack.Clear();
            while (tempStack.Count > 0)
            {
                _undoStack.Push(tempStack.Pop());
            }
        }

        SaveData();
        ShowNotification($"Action completed: {command.Description}");
    }

    // Core CRUD Operations
    public void AddVisit(PatientVisit visit)
    {
        visit.Id = _nextId++;
        var command = new AddVisitCommand(this, visit);
        ExecuteCommand(command);
    }

    internal void AddVisitInternal(PatientVisit visit)
    {
        _visits.Add(visit);
    }

    public void UpdateVisit(int id, PatientVisit updatedVisit)
    {
        var existingVisit = _visits.FirstOrDefault(v => v.Id == id);
        if (existingVisit != null)
        {
            updatedVisit.Id = id;
            var command = new UpdateVisitCommand(this, existingVisit, updatedVisit);
            ExecuteCommand(command);
        }
        else
        {
            Console.WriteLine("Visit not found!");
        }
    }

    internal void UpdateVisitInternal(PatientVisit visit)
    {
        var index = _visits.FindIndex(v => v.Id == visit.Id);
        if (index != -1)
        {
            _visits[index] = visit;
        }
    }

    public void DeleteVisit(int id)
    {
        var visit = _visits.FirstOrDefault(v => v.Id == id);
        if (visit != null)
        {
            var command = new DeleteVisitCommand(this, visit);
            ExecuteCommand(command);
        }
        else
        {
            Console.WriteLine("Visit not found!");
        }
    }

    internal void DeleteVisitInternal(int id)
    {
        _visits.RemoveAll(v => v.Id == id);
    }

    // Search Operations
    public List<PatientVisit> SearchByPatientName(string name)
    {
        return _visits.Where(v => v.PatientName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<PatientVisit> SearchByDoctorName(string doctorName)
    {
        return _visits.Where(v => v.DoctorName.Contains(doctorName, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<PatientVisit> SearchByDate(DateTime date)
    {
        return _visits.Where(v => v.VisitDate.Date == date.Date).ToList();
    }

    public List<PatientVisit> SearchByType(VisitType type)
    {
        return _visits.Where(v => v.VisitType == type).ToList();
    }

    // Undo/Redo Operations
    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            SaveData();
            ShowNotification($"Undone: {command.Description}");
        }
        else
        {
            Console.WriteLine("Nothing to undo.");
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            SaveData();
            ShowNotification($"Redone: {command.Description}");
        }
        else
        {
            Console.WriteLine("Nothing to redo.");
        }
    }

    // Reporting Features
    public void GenerateVisitSummary(int visitId)
    {
        var visit = _visits.FirstOrDefault(v => v.Id == visitId);
        if (visit != null)
        {
            Console.WriteLine("\n=== VISIT SUMMARY ===");
            Console.WriteLine($"Visit ID: {visit.Id}");
            Console.WriteLine($"Patient Name: {visit.PatientName}");
            Console.WriteLine($"Visit Date: {visit.VisitDate:yyyy-MM-dd}");
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
        var typeGroups = _visits.GroupBy(v => v.VisitType);
        foreach (var group in typeGroups)
        {
            Console.WriteLine($"{group.Key}: {group.Count()} visits");
        }
        Console.WriteLine("===========================");
    }

    public void GenerateWeeklySummary()
    {
        var today = DateTime.Now.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var weekEnd = weekStart.AddDays(6);

        var weeklyVisits = _visits.Where(v => v.VisitDate >= weekStart && v.VisitDate <= weekEnd).ToList();

        Console.WriteLine($"\n=== WEEKLY SUMMARY ({weekStart:yyyy-MM-dd} to {weekEnd:yyyy-MM-dd}) ===");
        Console.WriteLine($"Total visits this week: {weeklyVisits.Count}");

        var dailyGroups = weeklyVisits.GroupBy(v => v.VisitDate.Date).OrderBy(g => g.Key);
        foreach (var day in dailyGroups)
        {
            Console.WriteLine($"{day.Key:yyyy-MM-dd}: {day.Count()} visits");
        }
        Console.WriteLine("===========================================");
    }

    // Utility Methods
    private void ShowNotification(string message)
    {
        Console.WriteLine($"\n*** {message} ***");
    }

    public List<PatientVisit> GetAllVisits()
    {
        return _visits.ToList();
    }

    public PatientVisit GetVisitById(int id)
    {
        return _visits.FirstOrDefault(v => v.Id == id);
    }
}

// Main Program
public class Program
{
    private static PatientVisitManager _manager;

    public static void Main(string[] args)
    {
        Console.WriteLine("=== PATIENT VISIT MANAGER ===");
        Console.WriteLine("Initializing system...\n");

        _manager = new PatientVisitManager();

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

            var choice = Console.ReadLine();

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
                    _manager.Undo();
                    break;
                case "8":
                    _manager.Redo();
                    break;
                case "9":
                    Console.WriteLine("Thank you for using Patient Visit Manager!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void AddNewVisit()
    {
        Console.WriteLine("\n=== ADD NEW VISIT ===");

        try
        {
            Console.Write("Patient Name: ");
            var patientName = Console.ReadLine();

            Console.Write("Visit Date (yyyy-mm-dd) [Press Enter for today]: ");
            var dateInput = Console.ReadLine();
            var visitDate = string.IsNullOrWhiteSpace(dateInput) ? DateTime.Now.Date : DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Console.WriteLine("Visit Type:");
            Console.WriteLine("1. Consultation");
            Console.WriteLine("2. Follow-up");
            Console.WriteLine("3. Emergency");
            Console.Write("Select (1-3): ");
            var typeChoice = Console.ReadLine();
            var visitType = typeChoice switch
            {
                "1" => VisitType.Consultation,
                "2" => VisitType.FollowUp,
                "3" => VisitType.Emergency,
                _ => VisitType.Consultation
            };

            Console.Write("Description/Notes: ");
            var description = Console.ReadLine();

            Console.Write("Doctor Name (optional): ");
            var doctorName = Console.ReadLine();

            var visit = new PatientVisit(0, patientName, visitDate, visitType, description, doctorName ?? "");
            _manager.AddVisit(visit);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding visit: {ex.Message}");
        }
    }

    private static void UpdateVisit()
    {
        Console.WriteLine("\n=== UPDATE VISIT ===");
        Console.Write("Enter Visit ID to update: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var existingVisit = _manager.GetVisitById(id);
            if (existingVisit != null)
            {
                Console.WriteLine($"Current visit: {existingVisit}");
                Console.WriteLine("Enter new information (press Enter to keep current value):");

                Console.Write($"Patient Name [{existingVisit.PatientName}]: ");
                var patientName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(patientName)) patientName = existingVisit.PatientName;

                Console.Write($"Visit Date [{existingVisit.VisitDate:yyyy-MM-dd}]: ");
                var dateInput = Console.ReadLine();
                var visitDate = string.IsNullOrWhiteSpace(dateInput) ? existingVisit.VisitDate : DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Console.WriteLine($"Visit Type [{existingVisit.VisitType}]:");
                Console.WriteLine("1. Consultation");
                Console.WriteLine("2. Follow-up");
                Console.WriteLine("3. Emergency");
                Console.Write("Select (1-3) or press Enter to keep current: ");
                var typeChoice = Console.ReadLine();
                var visitType = typeChoice switch
                {
                    "1" => VisitType.Consultation,
                    "2" => VisitType.FollowUp,
                    "3" => VisitType.Emergency,
                    _ => existingVisit.VisitType
                };

                Console.Write($"Description [{existingVisit.Description}]: ");
                var description = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description)) description = existingVisit.Description;

                Console.Write($"Doctor Name [{existingVisit.DoctorName}]: ");
                var doctorName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(doctorName)) doctorName = existingVisit.DoctorName;

                var updatedVisit = new PatientVisit(id, patientName, visitDate, visitType, description, doctorName);
                _manager.UpdateVisit(id, updatedVisit);
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

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var visit = _manager.GetVisitById(id);
            if (visit != null)
            {
                Console.WriteLine($"Visit to delete: {visit}");
                Console.Write("Are you sure? (y/N): ");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y")
                {
                    _manager.DeleteVisit(id);
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

        var choice = Console.ReadLine();
        List<PatientVisit> results = new List<PatientVisit>();

        switch (choice)
        {
            case "1":
                Console.Write("Enter patient name (partial match): ");
                var patientName = Console.ReadLine();
                results = _manager.SearchByPatientName(patientName ?? "");
                break;
            case "2":
                Console.Write("Enter doctor name (partial match): ");
                var doctorName = Console.ReadLine();
                results = _manager.SearchByDoctorName(doctorName ?? "");
                break;
            case "3":
                Console.Write("Enter date (yyyy-mm-dd): ");
                var dateInput = Console.ReadLine();
                if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                {
                    results = _manager.SearchByDate(searchDate);
                }
                else
                {
                    Console.WriteLine("Invalid date format!");
                    return;
                }
                break;
            case "4":
                Console.WriteLine("Select visit type:");
                Console.WriteLine("1. Consultation");
                Console.WriteLine("2. Follow-up");
                Console.WriteLine("3. Emergency");
                Console.Write("Select (1-3): ");
                var typeChoice = Console.ReadLine();
                var visitType = typeChoice switch
                {
                    "1" => VisitType.Consultation,
                    "2" => VisitType.FollowUp,
                    "3" => VisitType.Emergency,
                    _ => VisitType.Consultation
                };
                results = _manager.SearchByType(visitType);
                break;
            default:
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
            foreach (var visit in results.Take(20)) // Limit to 20 results for readability
            {
                Console.WriteLine(visit.ToString());
            }
            if (results.Count > 20)
            {
                Console.WriteLine($"... and {results.Count - 20} more results.");
            }
        }
    }

    private static void ViewAllVisits()
    {
        var visits = _manager.GetAllVisits();
        Console.WriteLine($"\n=== ALL VISITS ({visits.Count} total) ===");

        if (visits.Count == 0)
        {
            Console.WriteLine("No visits found.");
            return;
        }

        Console.WriteLine("Showing first 20 visits:");
        Console.WriteLine(new string('-', 80));
        foreach (var visit in visits.Take(20))
        {
            Console.WriteLine(visit.ToString());
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

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter Visit ID: ");
                if (int.TryParse(Console.ReadLine(), out int visitId))
                {
                    _manager.GenerateVisitSummary(visitId);
                }
                else
                {
                    Console.WriteLine("Invalid ID format!");
                }
                break;
            case "2":
                _manager.GenerateVisitCountByType();
                break;
            case "3":
                _manager.GenerateWeeklySummary();
                break;
            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
}