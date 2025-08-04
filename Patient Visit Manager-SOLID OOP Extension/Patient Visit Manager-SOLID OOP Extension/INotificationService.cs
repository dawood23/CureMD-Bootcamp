using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public interface INotificationService
    {
        void ShowMessage(string message);
        void ShowError(string message);
        void ShowWarning(string message);
        void ShowSuccess(string message);
        bool ConfirmAction(string message);
    }
}
