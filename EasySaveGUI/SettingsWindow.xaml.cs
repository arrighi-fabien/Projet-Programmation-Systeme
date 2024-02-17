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
            Refresh();
        }

        private void Refresh() {
            // Set the Title of the window to the language
            Title = $"EasySave {language.GetString("settings")}";
            // Set the labels to the language
            LanguageLabel.Text = language.GetString("label_language");
            LogFormatLabel.Text = language.GetString("label_log_format");
            EncryptExtensionLabel.Text = language.GetString("label_encrypt_extensions");
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
            }
            Refresh();
        }

        private void LogFormatComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            string newLogFormat = (string)((System.Windows.Controls.ComboBoxItem)LogFormatComboBox.SelectedItem).Content;
            tool.WriteConfigValue("logFormat", newLogFormat.ToLower());
        }
    }
}
