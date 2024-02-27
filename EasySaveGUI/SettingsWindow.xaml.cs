using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {

        private readonly Language language = EasySaveGUI.model.Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();
        private Dictionary<string, string> languageMappings = new() {
            { "English", "en" },
            { "Français", "fr" },
            { "Español", "es" },
            { "Deutsch", "de" },
            { "Italiano", "it" },
            { "普通话", "ma" },
            { "Русский", "ru" },
            { "العربية", "ar" }
        };

        public SettingsWindow() {
            InitializeComponent();
            // Set the language combobox to the saved language
            string savedLanguage = tool.GetConfigValue("language");

            List<string> languageNames = new(languageMappings.Keys);
            LanguageComboBox.ItemsSource = languageNames;
            LanguageComboBox.SelectedIndex = languageNames.IndexOf(languageMappings.FirstOrDefault(x => x.Value == savedLanguage).Key);

            // Set the log format combobox to the saved log format
            string savedLogFormat = tool.GetConfigValue("logFormat");
            switch (savedLogFormat) {
                case "json":
                    LogFormatComboBox.SelectedIndex = 0;
                    break;
                case "xml":
                    LogFormatComboBox.SelectedIndex = 1;
                    break;
            }
            // Get the saved extensions and apps
            string savedEncryptExtensions = tool.GetConfigValue("encryptExtensions");
            EncryptExtensionTextBox.Text = savedEncryptExtensions.Replace(";", "\r\n");
            string savedProfessionalApp = tool.GetConfigValue("professsionalApp");
            ProfessionalAppTextBox.Text = savedProfessionalApp.Replace(";", "\r\n");
            string savedPriorityExtensions = tool.GetConfigValue("priorityExtensions");
            PriorityExtensionTextBox.Text = savedPriorityExtensions.Replace(";", "\r\n");

            Refresh();
        }

        private void Refresh() {
            // Set the Title of the window to the language
            Title = $"EasySave {language.GetString("settings")}";
            // Set the labels to the language
            LanguageLabel.Text = language.GetString("label_language");
            LogFormatLabel.Text = language.GetString("label_log_format");
            EncryptExtensionLabel.Text = language.GetString("label_encrypt_extensions");
            ProfessionalAppLabel.Text = language.GetString("label_professional_app");
            // Set the buttons to the language
            SaveButton.Content = language.GetString("save_button");
            // Refresh main window
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Refresh();
        }

        private void LanguageComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            // Get the new value of the combobox
            string newLanguage = LanguageComboBox.SelectedItem.ToString();
            // Change the language
            try {
                language.SetLanguage(languageMappings[newLanguage]);
            }
            catch (Exception) {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ShowErrorMessageBox(language.GetString("error_savejob"));
            }
            Refresh();
        }

        private void LogFormatComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            string newLogFormat = (string)((System.Windows.Controls.ComboBoxItem)LogFormatComboBox.SelectedItem).Content;
            tool.WriteConfigValue("logFormat", newLogFormat.ToLower());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            SaveTextAreas(PriorityExtensionTextBox.Text, "priorityExtensions");
            SaveTextAreas(EncryptExtensionTextBox.Text, "encryptExtensions");
            SaveTextAreas(ProfessionalAppTextBox.Text, "professsionalApp");
        }

        private void SaveTextAreas(string content, string key) {
            // Remove the empty lines
            string result = string.Join(";", content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries));
            // Save the apps in the config file
            tool.WriteConfigValue(key, result);
        }

    }
}
