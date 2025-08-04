using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
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
}
