using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace EasySave_RemoteGUI.model {

    // Class to manage the application language
    public class Tool {

        private static Tool? instance;
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions {
            WriteIndented = true
        };

        // Method to get the singleton instance of Tool
        public static Tool GetInstance() {
            instance ??= new Tool();
            return instance;
        }

        // Method to get a value from the config file
        public string GetConfigValue(string key) {
            try {
                string json = File.ReadAllText("config/config.json");
                var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (config != null && config.ContainsKey(key)) {
                    return config[key];
                }
                return "";
            }
            catch {
                return "";
            }
        }

        // Method to write a value to the config file
        public void WriteConfigValue(string key, string value) {
            Dictionary<string, string> config;
            try {
                string json = File.ReadAllText("config/config.json");
                config = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            catch {
                config = new Dictionary<string, string>();
            }

            config[key] = value;
            string updatedJson = JsonSerializer.Serialize(config, serializerOptions);
            File.WriteAllText("config/config.json", updatedJson);
        }
    }
}
