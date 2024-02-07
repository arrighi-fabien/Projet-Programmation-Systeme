using System.Globalization;
using System.Resources;

namespace EasySaveConsole.model {
    public class Language {
        private CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;
        public static Language instance;

        public Language(string lang="en") {
            resourceManager = new ResourceManager($"EasySaveConsole.config.locales.Resource", typeof(Program).Assembly);
            SetLanguage(lang);
        }

        public static Language GetInstance() {
            if (instance == null) {
                instance = new Language();
            }
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
            cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        }

    }
}
