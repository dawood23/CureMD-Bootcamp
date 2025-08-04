using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public interface ILogger
    {
        void Log(string action, Roles role, bool success);
        void LogActivity(Roles role, string action, bool success);
        void Logout(Roles role);
    }

}
