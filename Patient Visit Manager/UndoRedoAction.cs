using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
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
}
