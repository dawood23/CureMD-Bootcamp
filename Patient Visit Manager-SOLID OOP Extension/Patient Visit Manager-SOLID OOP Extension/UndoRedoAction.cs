using PatientVisitManager;
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
}
