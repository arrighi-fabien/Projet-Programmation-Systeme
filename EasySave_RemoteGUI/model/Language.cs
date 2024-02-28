using System;
using System.Globalization;
using System.Resources;

namespace EasySave_RemoteGUI.model {

    // Class to manage the application language
    public class Language {

        // Attributes for culture info, resource manager, and the instance of Language
        private CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;
        public static Language? instance;

        // Static event for language change notification
        public static event Action? LanguageChanged;

        public Language() {
            // Initialize resource manager to manage localization resources
            resourceManager = new ResourceManager("EasySave_RemoteGUI.config.locales.Resource", typeof(App).Assembly);
            // Get the saved language or default to English
            string lang = Tool.GetInstance().GetConfigValue("language");
            if (lang == "") {
                lang = "en";
            }
            SetLanguage(lang);
        }

        // Method to get the singleton instance of Language
        public static Language GetInstance() {
            instance ??= new Language();
            return instance;
        }

        // Method to get a localized string from the resource manager
        public string GetString(string key) {
            try {
                return resourceManager.GetString(key, cultureInfo) ?? "[Missing translation]";
            }
            catch (MissingManifestResourceException) {
                return "[Missing translation]";
            }
        }

        // Method to set the application language and notify subscribers
        public void SetLanguage(string languageCode) {
            // Write the saved language
            Tool.GetInstance().WriteConfigValue("language", languageCode);
            // Set the culture info
            cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        }
    }
}