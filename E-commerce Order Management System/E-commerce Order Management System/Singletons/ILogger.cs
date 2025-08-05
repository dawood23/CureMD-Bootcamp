using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Singletons
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private List<string> _logs = new List<string>();

        private Logger() { }

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new Logger();
                    }
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public void ShowLogs()
        {
            Console.WriteLine("\n=== System Logs ===");
            foreach (var log in _logs)
                Console.WriteLine(log);
        }
    }
}
