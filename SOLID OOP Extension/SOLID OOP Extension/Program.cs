using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

    public interface IVisit
    {
        int Id { get; set; }
        string PatientName { get; set; }
        DateTime VisitDate { get; set; }
        string Description { get; set; }
        string DoctorName { get; set; }
        int VisitDuration { get; set; }
        int Fee { get; set; }
        VisitType VisitType { get; }

        void CalculateFee();
        bool ValidateVisit();
        string ToCsvString();
    }

    public interface IVisitRepository
    {
        void SaveVisits(List<IVisit> visits);
        List<IVisit> LoadVisits();
        bool FileExists();
    }

    public interface ILogger
    {
        void Log(string action, Roles role, bool success);
        void LogActivity(Roles role, string action, bool success);
        void Logout(Roles role);
    }

    public interface INotificationService
    {
        void ShowMessage(string message);
        void ShowError(string message);
        void ShowWarning(string message);
        void ShowSuccess(string message);
        bool ConfirmAction(string message);
    }

    public interface IReportGenerator
    {
        void GenerateVisitSummary(int visitId, List<IVisit> visits);
        void GenerateVisitCountByType(List<IVisit> visits);
        void GenerateWeeklySummary(List<IVisit> visits);
    }

    public interface IVisitManager
    {
        void AddVisit(IVisit visit);
        void UpdateVisit(int id, IVisit updatedVisit);
        void DeleteVisit(int id);
        IVisit GetVisitById(int id);
        List<IVisit> GetAllVisits();
        List<IVisit> SearchByPatientName(string name);
        List<IVisit> SearchByDoctorName(string doctorName);
        List<IVisit> SearchByDate(DateTime date);
        List<IVisit> SearchByVisitType(VisitType type);
        List<IVisit> SortByDate();
        List<IVisit> SortByName();
        List<IVisit> SortByDoctorName();
        void Undo();
        void Redo();
        bool HasConflictingVisit(string patientName, DateTime visitDate);
    }

    public interface IRoleService
    {
        void ShowMenu();
        void HandleMenuChoice(string choice);
    }

    public abstract class BaseVisit : IVisit
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Description { get; set; }
        public string DoctorName { get; set; }
        public int VisitDuration { get; set; }
        public int Fee { get; set; }
        public abstract VisitType VisitType { get; }

        protected BaseVisit()
        {
            PatientName = "";
            Description = "";
            DoctorName = "";
        }

        protected BaseVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration = 0)
        {
            Id = id;
            PatientName = patientName;
            VisitDate = visitDate;
            Description = description;
            DoctorName = doctorName;
            VisitDuration = duration;
        }

        public abstract void CalculateFee();

        public virtual bool ValidateVisit()
        {
            if (string.IsNullOrWhiteSpace(PatientName))
                return false;
            if (VisitDuration <= 0)
                return false;
            if (VisitDate == default)
                return false;
            return true;
        }

        public virtual string ToCsvString()
        {
            return $"{Id},{PatientName},{VisitDate:yyyy-MM-dd HH:mm},{VisitType},{Description},{DoctorName},{VisitDuration},{Fee}";
        }

        public override string ToString()
        {
            return $"ID: {Id}, Patient: {PatientName}, Date: {VisitDate:yyyy-MM-dd HH:mm}, Type: {VisitType}, Doctor: {DoctorName}, Duration: {VisitDuration}min, Fee: ${Fee}";
        }

        public static IVisit FromCsvString(string csvLine)
        {
            string[] parts = csvLine.Split(',');
            if (parts.Length != 8)
                return null;

            try
            {
                var visitTypeEnum = (VisitType)Enum.Parse(typeof(VisitType), parts[3]);
                var visitDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                var duration = int.Parse(parts[6]);
                var fee = int.Parse(parts[7]);

                IVisit visit = visitTypeEnum switch
                {
                    VisitType.Consultation => new ConsultationVisit(),
                    VisitType.FollowUp => new FollowUpVisit(),
                    VisitType.Emergency => new EmergencyVisit(),
                    _ => throw new ArgumentException("Unknown visit type")
                };

                visit.Id = int.Parse(parts[0]);
                visit.PatientName = parts[1];
                visit.VisitDate = visitDate;
                visit.Description = parts[4];
                visit.DoctorName = parts[5];
                visit.VisitDuration = duration;
                visit.Fee = fee;

                return visit;
            }
            catch
            {
                return null;
            }
        }
    }

    public class ConsultationVisit : BaseVisit
    {
        public override VisitType VisitType => VisitType.Consultation;
        private const int BASE_FEE_PER_MINUTE = 500;

        public ConsultationVisit() : base() { }

        public ConsultationVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration)
            : base(id, patientName, visitDate, description, doctorName, duration)
        {
            CalculateFee();
        }

        public override void CalculateFee()
        {
            Fee = VisitDuration * BASE_FEE_PER_MINUTE;
        }

        public override bool ValidateVisit()
        {
            if (!base.ValidateVisit())
                return false;

            if (VisitDuration < 15)
                return false;

            return true;
        }
    }

    public class FollowUpVisit : BaseVisit
    {
        public override VisitType VisitType => VisitType.FollowUp;
        private const int BASE_FEE_PER_MINUTE = 300;

        public FollowUpVisit() : base() { }

        public FollowUpVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration)
            : base(id, patientName, visitDate, description, doctorName, duration)
        {
            CalculateFee();
        }

        public override void CalculateFee()
        {
            Fee = VisitDuration * BASE_FEE_PER_MINUTE;
        }

        public override bool ValidateVisit()
        {
            if (!base.ValidateVisit())
                return false;

            if (VisitDuration < 10)
                return false;

            return true;
        }
    }

    public class EmergencyVisit : BaseVisit
    {
        public override VisitType VisitType => VisitType.Emergency;
        private const int BASE_FEE_PER_MINUTE = 1000;

        public EmergencyVisit() : base() { }

        public EmergencyVisit(int id, string patientName, DateTime visitDate, string description, string doctorName, int duration)
            : base(id, patientName, visitDate, description, doctorName, duration)
        {
            CalculateFee();
        }

        public override void CalculateFee()
        {
            Fee = VisitDuration * BASE_FEE_PER_MINUTE;
            if (Fee < 2000)
                Fee = 2000;
        }

        public override bool ValidateVisit()
        {
            if (!base.ValidateVisit())
                return false;

            if (string.IsNullOrWhiteSpace(DoctorName))
                return false;

            return true;
        }
    }

    public class UndoRedoAction
    {
        public string ActionType { get; set; }
        public IVisit VisitData { get; set; }
        public IVisit OldVisitData { get; set; }
        public string Description { get; set; }

        public UndoRedoAction(string actionType, IVisit visitData, string description)
        {
            ActionType = actionType;
            VisitData = visitData;
            Description = description;
        }

        public UndoRedoAction(string actionType, IVisit visitData, IVisit oldVisitData, string description)
        {
            ActionType = actionType;
            VisitData = visitData;
            OldVisitData = oldVisitData;
            Description = description;
        }
    }

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

    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath = "activity_log.txt")
        {
            _logFilePath = logFilePath;
        }

        public void Log(string action, Roles role, bool success)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = success ? "Success" : "Failure";
            string logEntry = $"{timestamp} | Role: {role} | Action: {action} | Status: {status}";

            WriteToFile(logEntry);
        }

        public void LogActivity(Roles role, string action, bool success)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = success ? "Success" : "Failure";
            string logEntry = $"{timestamp} | {role} | {action} | Status: {status}";

            WriteToFile(logEntry);
        }

        public void Logout(Roles role)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"{timestamp} | Role: {role} | Action: {role} Logged Out";

            WriteToFile(logEntry);
        }

        private void WriteToFile(string logEntry)
        {
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
    }

    public class ConsoleNotificationService : INotificationService
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}");
            Console.ResetColor();
        }

        public void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: {message}");
            Console.ResetColor();
        }

        public void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS: {message}");
            Console.ResetColor();
        }

        public bool ConfirmAction(string message)
        {
            Console.Write($"{message} (Y/N): ");
            string response = Console.ReadLine()?.Trim().ToUpper();
            return response == "Y" || response == "YES";
        }
    }

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

    public class VisitManager : IVisitManager
    {
        private List<IVisit> _visits;
        private readonly IVisitRepository _repository;
        private readonly INotificationService _notificationService;
        private int _nextId;
        private readonly Stack<UndoRedoAction> _undoStack;
        private readonly Stack<UndoRedoAction> _redoStack;
        private const int MAX_UNDO_ACTIONS = 10;

        public VisitManager(IVisitRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _visits = new List<IVisit>();
            _nextId = 1;
            _undoStack = new Stack<UndoRedoAction>();
            _redoStack = new Stack<UndoRedoAction>();

            LoadVisits();
        }

        private void LoadVisits()
        {
            _visits = _repository.LoadVisits();

            if (_visits.Any())
            {
                _nextId = _visits.Max(v => v.Id) + 1;
                _notificationService.ShowSuccess($"Loaded {_visits.Count} visits from file.");
            }
            else if (!_repository.FileExists())
            {
                _notificationService.ShowMessage("No existing data file found. Creating sample data...");
                CreateSampleData();
            }
        }

        private void CreateSampleData()
        {
            string[] patientNames = { "John Smith", "Jane Doe", "Robert Johnson", "Emily Davis", "Michael Brown" };
            string[] doctorNames = { "Dr. Anderson", "Dr. Smith", "Dr. Johnson", "Dr. Williams" };
            string[] descriptions = { "Routine checkup", "Follow-up examination", "Emergency treatment", "Consultation" };

            Random random = new Random();

            for (int i = 0; i < 400; i++)
            {
                var visitType = (VisitType)random.Next(3);
                var visitDate = DateTime.Now.AddDays(-random.Next(30))
                    .Date.AddHours(random.Next(8, 18)).AddMinutes(random.Next(0, 60));
                var duration = random.Next(15, 120);

                IVisit visit = visitType switch
                {
                    VisitType.Consultation => new ConsultationVisit(_nextId++,
                        patientNames[random.Next(patientNames.Length)], visitDate,
                        descriptions[random.Next(descriptions.Length)],
                        doctorNames[random.Next(doctorNames.Length)], duration),
                    VisitType.FollowUp => new FollowUpVisit(_nextId++,
                        patientNames[random.Next(patientNames.Length)], visitDate,
                        descriptions[random.Next(descriptions.Length)],
                        doctorNames[random.Next(doctorNames.Length)], duration),
                    VisitType.Emergency => new EmergencyVisit(_nextId++,
                        patientNames[random.Next(patientNames.Length)], visitDate,
                        descriptions[random.Next(descriptions.Length)],
                        doctorNames[random.Next(doctorNames.Length)], duration),
                    _ => throw new ArgumentException("Unknown visit type")
                };

                _visits.Add(visit);
            }

            SaveVisits();
            _notificationService.ShowSuccess($"Created {_visits.Count} sample patient visits for testing.");
        }

        private void SaveVisits()
        {
            _repository.SaveVisits(_visits);
        }

        private void AddUndoAction(UndoRedoAction action)
        {
            _undoStack.Push(action);

            if (_undoStack.Count > MAX_UNDO_ACTIONS)
            {
                var tempStack = new Stack<UndoRedoAction>();
                for (int i = 0; i < MAX_UNDO_ACTIONS; i++)
                {
                    tempStack.Push(_undoStack.Pop());
                }
                _undoStack.Clear();
                while (tempStack.Count > 0)
                {
                    _undoStack.Push(tempStack.Pop());
                }
            }

            _redoStack.Clear();
            _notificationService.ShowMessage($"*** Action completed: {action.Description} ***");
        }

        public void AddVisit(IVisit visit)
        {
            if (!visit.ValidateVisit())
            {
                _notificationService.ShowError("Visit validation failed!");
                return;
            }

            visit.Id = _nextId++;
            visit.CalculateFee();

            _visits.Add(visit);
            SaveVisits();

            var action = new UndoRedoAction("ADD", visit, $"Add visit for {visit.PatientName}");
            AddUndoAction(action);
        }

        public void UpdateVisit(int id, IVisit updatedVisit)
        {
            var existingVisit = GetVisitById(id);
            if (existingVisit == null)
            {
                _notificationService.ShowError("Visit not found!");
                return;
            }

            if (!updatedVisit.ValidateVisit())
            {
                _notificationService.ShowError("Updated visit validation failed!");
                return;
            }

            var oldVisitCopy = CreateVisitCopy(existingVisit);

            updatedVisit.Id = id;
            updatedVisit.CalculateFee();

            var index = _visits.FindIndex(v => v.Id == id);
            _visits[index] = updatedVisit;

            SaveVisits();

            var action = new UndoRedoAction("UPDATE", updatedVisit, oldVisitCopy, $"Update visit for {updatedVisit.PatientName}");
            AddUndoAction(action);
        }

        public void DeleteVisit(int id)
        {
            var visitToDelete = GetVisitById(id);
            if (visitToDelete == null)
            {
                _notificationService.ShowError("Visit not found!");
                return;
            }

            var deletedVisitCopy = CreateVisitCopy(visitToDelete);
            _visits.Remove(visitToDelete);
            SaveVisits();

            var action = new UndoRedoAction("DELETE", deletedVisitCopy, $"Delete visit for {deletedVisitCopy.PatientName}");
            AddUndoAction(action);
        }

        private IVisit CreateVisitCopy(IVisit original)
        {
            return original.VisitType switch
            {
                VisitType.Consultation => new ConsultationVisit(original.Id, original.PatientName,
                    original.VisitDate, original.Description, original.DoctorName, original.VisitDuration),
                VisitType.FollowUp => new FollowUpVisit(original.Id, original.PatientName,
                    original.VisitDate, original.Description, original.DoctorName, original.VisitDuration),
                VisitType.Emergency => new EmergencyVisit(original.Id, original.PatientName,
                    original.VisitDate, original.Description, original.DoctorName, original.VisitDuration),
                _ => throw new ArgumentException("Unknown visit type")
            };
        }

        public IVisit GetVisitById(int id)
        {
            return _visits.FirstOrDefault(v => v.Id == id);
        }

        public List<IVisit> GetAllVisits()
        {
            return new List<IVisit>(_visits);
        }

        public List<IVisit> SearchByPatientName(string name)
        {
            return _visits.Where(v => v.PatientName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<IVisit> SearchByDoctorName(string doctorName)
        {
            return _visits.Where(v => v.DoctorName.Contains(doctorName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<IVisit> SearchByDate(DateTime date)
        {
            return _visits.Where(v => v.VisitDate.Date == date.Date).ToList();
        }

        public List<IVisit> SearchByVisitType(VisitType type)
        {
            return _visits.Where(v => v.VisitType == type).ToList();
        }

        public List<IVisit> SortByDate()
        {
            return _visits.OrderBy(v => v.VisitDate).ToList();
        }

        public List<IVisit> SortByName()
        {
            return _visits.OrderBy(v => v.PatientName).ToList();
        }

        public List<IVisit> SortByDoctorName()
        {
            return _visits.OrderBy(v => v.DoctorName).ToList();
        }

        public bool HasConflictingVisit(string patientName, DateTime visitDate)
        {
            return _visits.Any(v =>
                v.PatientName.Equals(patientName, StringComparison.OrdinalIgnoreCase) &&
                Math.Abs((v.VisitDate - visitDate).TotalMinutes) < 30);
        }

        public void Undo()
        {
            if (_undoStack.Count == 0)
            {
                _notificationService.ShowMessage("Nothing to undo.");
                return;
            }

            var action = _undoStack.Pop();

            switch (action.ActionType)
            {
                case "ADD":
                    _visits.RemoveAll(v => v.Id == action.VisitData.Id);
                    break;
                case "DELETE":
                    _visits.Add(action.VisitData);
                    break;
                case "UPDATE":
                    var index = _visits.FindIndex(v => v.Id == action.VisitData.Id);
                    if (index >= 0)
                        _visits[index] = action.OldVisitData;
                    break;
            }

            SaveVisits();
            _redoStack.Push(action);
            _notificationService.ShowMessage($"*** Undone: {action.Description} ***");
        }

        public void Redo()
        {
            if (_redoStack.Count == 0)
            {
                _notificationService.ShowMessage("Nothing to redo.");
                return;
            }

            var action = _redoStack.Pop();

            switch (action.ActionType)
            {
                case "ADD":
                    _visits.Add(action.VisitData);
                    break;
                case "DELETE":
                    _visits.RemoveAll(v => v.Id == action.VisitData.Id);
                    break;
                case "UPDATE":
                    var index = _visits.FindIndex(v => v.Id == action.VisitData.Id);
                    if (index >= 0)
                        _visits[index] = action.VisitData;
                    break;
            }

            SaveVisits();
            _undoStack.Push(action);
            _notificationService.ShowMessage($"*** Redone: {action.Description} ***");
        }
    }

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

    public class AuthenticationService
    {
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;

        private const string AdminEmail = "admin@gmail.com";
        private const string AdminPassword = "12345";
        private const string ReceptionistEmail = "reception@gmail.com";
        private const string ReceptionistPassword = "1234";

        public AuthenticationService(ILogger logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public bool AuthenticateAdmin(string email, string password)
        {
            bool isValid = email == AdminEmail && password == AdminPassword;
            _logger.Log(isValid ? "Admin Login" : "Admin Login Attempt", Roles.Admin, isValid);

            if (!isValid)
            {
                _notificationService.ShowError("Invalid Admin Credentials");
            }

            return isValid;
        }

        public bool AuthenticateReceptionist(string email, string password)
        {
            bool isValid = email == ReceptionistEmail && password == ReceptionistPassword;
            _logger.Log(isValid ? "Reception Login" : "Reception Login Attempt", Roles.Receptionist, isValid);

            if (!isValid)
            {
                _notificationService.ShowError("Invalid Receptionist Credentials");
            }

            return isValid;
        }
    }

    public class Application
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;
        private readonly IVisitRepository _repository;
        private readonly IVisitManager _visitManager;
        private readonly IReportGenerator _reportGenerator;
        private readonly AuthenticationService _authService;

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
}