using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public interface IVisitRepository
    {
        void SaveVisits(List<IVisit> visits);
        List<IVisit> LoadVisits();
        bool FileExists();
    }

}
