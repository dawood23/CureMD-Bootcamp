using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public interface IReportGenerator
    {
        void GenerateVisitSummary(int visitId, List<IVisit> visits);
        void GenerateVisitCountByType(List<IVisit> visits);
        void GenerateWeeklySummary(List<IVisit> visits);
    }
}
