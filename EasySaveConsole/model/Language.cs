using System.Globalization;
using System.Resources;
using System.Text.Json;

namespace EasySaveConsole.model {
    public class Language {
        private CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;
        public static Language instance;

        public Language() {
            resourceManager = new($"EasySaveConsole.config.locales.Resource", typeof(Program).Assembly);
            string lang = GetSavedLanguage();
            if (lang == "") {
                lang = "en";
            }
            SetLanguage(lang);
        }

        public static Language GetInstance() {
            instance ??= new Language();
            return instance;
        }

        public string GetString(string key) {
            try {
                return resourceManager.GetString(key, cultureInfo);
            }
            catch (MissingManifestResourceException) {
                return "[Missing translation]";
            }
        }

        public void SetLanguage(string languageCode) {
            WriteSavedLanguage(languageCode);
            cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        }

        public string GetSavedLanguage() {
            try {
                string json = File.ReadAllText("config/language.json");
                string language = JsonSerializer.Deserialize<Dictionary<string, string>>(json)["language"];
                return language;
            }
            catch {
                return "";
            }
        }

        public void WriteSavedLanguage(string language) {
            string json = JsonSerializer.Serialize(new Dictionary<string, string> { { "language", language } });
            File.WriteAllText("config/language.json", json);
        }

    }
}
