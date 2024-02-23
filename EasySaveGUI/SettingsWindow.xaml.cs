using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {

        private readonly Language language = EasySaveGUI.model.Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();

        public SettingsWindow() {
            InitializeComponent();
            // Set the language combobox to the saved language
            string savedLanguage = tool.GetConfigValue("language");
            switch (savedLanguage) {
                case "en":
                    LanguageComboBox.SelectedIndex = 0;
                    break;
                case "fr":
                    LanguageComboBox.SelectedIndex = 1;
                    break;
                case "es":
                    LanguageComboBox.SelectedIndex = 2;
                    break;
                case "de":
                    LanguageComboBox.SelectedIndex = 3;
                    break;
                case "it":
                    LanguageComboBox.SelectedIndex = 4;
                    break;
                case "ru":
                    LanguageComboBox.SelectedIndex = 5;
                    break;
                case "ar":
                    LanguageComboBox.SelectedIndex = 6;
                    break;
            }
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
            EncryptExtensionSaveButton.Content = language.GetString("save_button");
            ProfessionalAppSaveBtton.Content = language.GetString("save_button");
            // Refresh main window
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Refresh();
        }

        private void LanguageComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            // Get the new value of the combobox
            string newLanguage = (string)((System.Windows.Controls.ComboBoxItem)LanguageComboBox.SelectedItem).Content;
            // Change the language
            switch (newLanguage) {
                case "English":
                    language.SetLanguage("en");
                    break;
                case "Français":
                    language.SetLanguage("fr");
                    break;
                case "Español":
                    language.SetLanguage("es");
                    break;
                case "Deutsch":
                    language.SetLanguage("de");
                    break;
                case "Italiano":
                    language.SetLanguage("it");
                    break;
                case "普通话":
                    language.SetLanguage("ma");
                    break;
                case "Русский":
                    language.SetLanguage("ru");
                    break;
                case "العربية":
                    language.SetLanguage("ar");
                    break;
            }
            Refresh();
        }

        private void LogFormatComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            string newLogFormat = (string)((System.Windows.Controls.ComboBoxItem)LogFormatComboBox.SelectedItem).Content;
            tool.WriteConfigValue("logFormat", newLogFormat.ToLower());
        }

        private void EncryptExtensionSaveButton_Click(object sender, RoutedEventArgs e) {
            SaveTextAreas(EncryptExtensionTextBox.Text, "encryptExtensions");
        }

        private void ProfessionalAppSaveBtton_Click(object sender, RoutedEventArgs e) {
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
