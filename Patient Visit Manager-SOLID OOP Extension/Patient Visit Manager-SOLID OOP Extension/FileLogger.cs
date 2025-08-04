using PatientVisitManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientVisitManager
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath = "activity_log.txt")
        {
            _logFilePath = logFilePath;
        }

        public void Log(string action, Roles role, bool success)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = success ? "Success" : "Failure";
            string logEntry = $"{timestamp} | Role: {role} | Action: {action} | Status: {status}";

            WriteToFile(logEntry);
        }

        public void LogActivity(Roles role, string action, bool success)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = success ? "Success" : "Failure";
            string logEntry = $"{timestamp} | {role} | {action} | Status: {status}";

            WriteToFile(logEntry);
        }

        public void Logout(Roles role)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"{timestamp} | Role: {role} | Action: {role} Logged Out";

            WriteToFile(logEntry);
        }

        private void WriteToFile(string logEntry)
        {
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
    }
}
