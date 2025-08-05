using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Singletons
{
    public class ConfigManager
    {
        private static ConfigManager _instance;
        private static readonly object _lock = new object();
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        private ConfigManager()
        {
            _settings["Currency"] = "USD";
            _settings["MaxItems"] = "50";
            _settings["StoreEmail"] = "store@example.com";
        }

        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new ConfigManager();
                    }
                }
                return _instance;
            }
        }

        public string GetSetting(string key) => _settings.ContainsKey(key) ? _settings[key] : null;
        public void SetSetting(string key, string value) => _settings[key] = value;

        public void ShowSettings()
        {
            Console.WriteLine("\n=== Configuration ===");
            foreach (var setting in _settings)
                Console.WriteLine($"{setting.Key}: {setting.Value}");
        }
    }

}
