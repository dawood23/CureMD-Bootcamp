using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
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
        public void AddRedoAction(UndoRedoAction action)
        {
            redoStack.Push(action);
            if (redoStack.Count > MAX_UNDO_ACTIONS)
            {
                Stack<UndoRedoAction> tempstack = new Stack<UndoRedoAction>();

                for (int i = 0; i < MAX_UNDO_ACTIONS; i++)
                {
                    tempstack.Push(undoStack.Pop());
                }
                redoStack.Clear();
                while (tempstack.Count > 0)
                {
                    redoStack.Push(tempstack.Pop());
                }

            }
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

        public List<PatientVisit> SortbyDate()
        {
            return visits.OrderBy(x => x.VisitDate).ToList();
        }

        public List<PatientVisit> SortByName()
        {
            return visits.OrderBy(x => x.PatientName).ToList();
        }

        public List<PatientVisit> SortByDoctorName()
        {
            return visits.OrderBy(x => x.DoctorName).ToList();
        }

    }
}
