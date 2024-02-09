using System.Globalization;
using System.Resources;
using System.Text.Json;

namespace EasySaveConsole.model {

    // Class to manage the language of the application
    public class Language {
        
        // Attributes for the culture info, the resource manager and the instance of Language
        private CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;
        public static Language instance;

        /// <summary>
        /// Constructor for Language
        /// </summary>
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

        /// <summary>
        /// Method to get the instance of Language
        /// </summary>
        /// <returns>The instance of Language.</returns>
        public static Language GetInstance() {
            // If instance is null, create a new instance
            instance ??= new Language();
            return instance;
        }

        /// <summary>
        /// Method to get a string from the resource manager
        /// </summary>
        /// <param name="key">The key of the string to get.</param>
        /// <returns>The string from the resource manager.</returns>
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

        /// <summary>
        /// Method to set the language of the application
        /// </summary>
        /// <param name="languageCode">The language code to set.</param>
        public void SetLanguage(string languageCode) {
            // Write the saved language
            WriteSavedLanguage(languageCode);
            // Set the culture info
            cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        }

        /// <summary>
        /// Get the saved language from the language.json config file
        /// </summary>
        /// <returns>The saved language code</returns>
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

        /// <summary>
        /// Method to write the saved language to the config file
        /// </summary>
        /// <param name="language">The language code to save.</param>
        public void WriteSavedLanguage(string language) {
            // Write the language to the config file
            string json = JsonSerializer.Serialize(new Dictionary<string, string> { { "language", language } });
            File.WriteAllText("config/language.json", json);
        }

    }
}