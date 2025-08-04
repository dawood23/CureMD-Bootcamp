using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
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
}
