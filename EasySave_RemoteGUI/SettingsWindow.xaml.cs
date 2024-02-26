using System.Windows;
using System.Windows.Controls;
using EasySave_RemoteGUI.model;

namespace EasySave_RemoteGUI {

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    // Class to manage the settings window
    public partial class SettingsWindow : Window {
        private readonly Language language = EasySave_RemoteGUI.model.Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();

        // Constructor
        public SettingsWindow() {
            InitializeComponent();
            LoadSettings();
            InitializeLanguageComboBox();
            Refresh();
        }

        // Method to load the settings

        private void LoadSettings() {
            // Load the server IP and port from the config file
            string currentServerIp = tool.GetConfigValue("server_ip");
            string currentServerPort = tool.GetConfigValue("server_port");
            // Set the server IP and port in the textboxes
            ServerIpTextBox.Text = !string.IsNullOrEmpty(currentServerIp) ? currentServerIp : "127.0.0.1"; // Utilisez votre propre adresse IP par défaut si nécessaire
            ServerPortTextBox.Text = !string.IsNullOrEmpty(currentServerPort) ? currentServerPort : "5500"; // Utilisez votre propre port par défaut si nécessaire
        }

        // Method to initialize the language combobox

        private void InitializeLanguageComboBox() {
            string savedLanguage = tool.GetConfigValue("language");
            var languageIndex = new Dictionary<string, int> {
                { "en", 0 }, { "fr", 1 }, { "es", 2 }, { "de", 3 }, { "it", 4 }, { "ru", 5 }, { "ar", 6 }
            };

            LanguageComboBox.SelectedIndex = languageIndex.TryGetValue(savedLanguage, out int index) ? index : 0; // Default to English if not found
        }

        // Method to refresh the language
        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            tool.WriteConfigValue("server_ip", ServerIpTextBox.Text);
            tool.WriteConfigValue("server_port", ServerPortTextBox.Text);

            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedLanguage) {
                var languageCode = selectedLanguage.Content.ToString().ToLower();
                var languageMap = new Dictionary<string, string> {
                    { "english", "en" },
                    { "français", "fr" },
                    { "español", "es" },
                    { "deutsch", "de" },
                    { "italiano", "it" },
                    { "русский", "ru" },
                    { "العربية", "ar" }
                };
                if (languageMap.TryGetValue(languageCode, out string code)) {
                    tool.WriteConfigValue("language", code);
                    language.SetLanguage(code);
                }
            }

            MessageBox.Show(language.GetString("settings_saved"));
            this.Close();
        }

        // Method to refresh the language

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedLanguage) {
                var languageCode = selectedLanguage.Content.ToString().ToLower();
                var languageMap = new Dictionary<string, string> {
                    { "english", "en" },
                    { "français", "fr" },
                    { "español", "es" },
                    { "deutsch", "de" },
                    { "italiano", "it" },
                    { "русский", "ru" },
                    { "العربية", "ar" }
                };
                if (languageMap.TryGetValue(languageCode, out string code)) {
                    language.SetLanguage(code);
                    Refresh();
                }
            }
        }

        // Method to refresh the language
        private void Refresh() {
            ServerIpLabel.Text = language.GetString("server_ip");
            ServerPortLabel.Text = language.GetString("server_port");
            LanguageLabel.Text = language.GetString("language");
            SaveLabel.Content = language.GetString("save_button"); 
        }
    }
}
