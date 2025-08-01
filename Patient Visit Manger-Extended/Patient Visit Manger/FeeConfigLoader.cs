using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PatientVisitManager
{
    public static class FeeConfigLoader
    {
        private static Dictionary<string, int> feeRules;

        public static void LoadFees(string filePath = "Data.json")
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fee config file not found. Creating default config...");

                var defaultFees = new Dictionary<string, int>
                {
                    { "Consultation", 500 },
                    { "Follow-up", 300 },
                    { "Emergency", 1000 }
                };

                string defaultJson = JsonSerializer.Serialize(defaultFees, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, defaultJson);

                feeRules = defaultFees;
                return;
            }

            string json = File.ReadAllText(filePath);
            feeRules = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
        }


        public static int GetFeeForVisitType(VisitType type)
        {
            if (feeRules == null)
                LoadFees();

            string key = type.ToString().Replace("FollowUp", "Follow-up");
            return feeRules.ContainsKey(key) ? feeRules[key] : 0;
        }

    }
}
