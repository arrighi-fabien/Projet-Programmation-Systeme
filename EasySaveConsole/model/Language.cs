using System.Globalization;
using System.Resources;

namespace EasySaveConsole.model {
    public class Language {
        private CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;

        public Language(string lang="en") {
            resourceManager = new ResourceManager($"EasySaveConsole.config.locales.Resource", typeof(Program).Assembly);
            SetLanguage(lang);
        }

        public string GetString(string key) {
            try {
                return resourceManager.GetString(key, cultureInfo);
            }
            catch (MissingManifestResourceException) {
                return "Missing translation";
            }
        }

        public void SetLanguage(string languageCode) {
            cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        }

    }
}
