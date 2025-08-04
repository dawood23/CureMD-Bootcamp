using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
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
}
