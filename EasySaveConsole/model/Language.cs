using System.Globalization;
using System.Resources;
using System.Text.Json;

namespace EasySaveConsole.model {

    // Class to manage the language of the application
    public class Language {
        
        // Variables for culture info and resource manager
        private CultureInfo cultureInfo;

        // Resource manager for language
        private readonly ResourceManager resourceManager;
        
        // Constructor for Language
        public static Language instance;

        // Constructor for Language
        public Language() {

            // Set resource manager
            resourceManager = new($"EasySaveConsole.config.locales.Resource", typeof(Program).Assembly);
            
            // Get saved language
            string lang = GetSavedLanguage();
            
            // If no language is saved, set to English
            if (lang == "") {
                lang = "en";
            }

            // Set culture info
            SetLanguage(lang);
        }

        // Method to get the instance of Language
        public static Language GetInstance() {

            // If instance is null, create a new instance
            instance ??= new Language();

            return instance;
        }

        // Method to get a string from the resource manager
        public string GetString(string key) {
            try {
                // Return the string from the resource manager
                return resourceManager.GetString(key, cultureInfo);
            }
            // If the string is missing, return a placeholder
            catch (MissingManifestResourceException) {
                return "[Missing translation]";
            }
        }
        
        // Method to set the language
        public void SetLanguage(string languageCode) {
            
            // Write the saved language
            WriteSavedLanguage(languageCode);

            // Set the culture info
            cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        }

        // Method to get the saved language
        public string GetSavedLanguage() {
            
            // Try to read the saved language from the config file
            try {
                string json = File.ReadAllText("config/language.json");
                string language = JsonSerializer.Deserialize<Dictionary<string, string>>(json)["language"];
                return language;
            }
            // If the file is missing, return an empty string
            catch {
                return "";
            }
        }

        // Method to write the saved language to the config file
        public void WriteSavedLanguage(string language) {

            // Write the language to the config file
            string json = JsonSerializer.Serialize(new Dictionary<string, string> { { "language", language } });
            File.WriteAllText("config/language.json", json);
        }

    }
}