using System;
using System.IO;

namespace PatientVisitManager
{
    class ActivityLogging
    {
        public Roles Roles { get; set; }
        public string Filename { get; set; }

        public ActivityLogging(string filename)
        {
            this.Filename = filename;
        }

        public void Log(string action, Roles role, bool success)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = success ? "Success" : "Failure";
            string logEntry = $"{timestamp} | Role: {role} | Action: {action} | Status: {status}";

            try
            {
                File.AppendAllText(Filename, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
        public void Logout(Roles role)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"{timestamp} | Role: {role} | Action: {role} Logged Out";
            try
            {
                File.AppendAllText(Filename, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        public void LogActivity(Roles role,string action,bool success)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = success ? "Success" : "Failure";
            string currentLogEntry = $"{timestamp} | {role} | {action} | status: {status}";

            try
            {

                File.AppendAllText(Filename, currentLogEntry + Environment.NewLine);

            }catch(Exception ex)
            {
                Console.WriteLine("Failed to Edit the Log File for entry: "+currentLogEntry);

            }
        }
    }
}
