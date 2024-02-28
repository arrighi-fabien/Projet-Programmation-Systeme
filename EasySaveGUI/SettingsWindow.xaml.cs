using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {

        private readonly Language language = EasySaveGUI.model.Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();
        private Server _server = new EasySaveGUI.model.Server();
        private Dictionary<string, string> languageMappings = new() {
            { "English", "en" },
            { "Français", "fr" },
            { "Español", "es" },
            { "Deutsch", "de" },
            { "Italiano", "it" },
            { "Русский", "ru" },
            { "العربية", "ar" }
        };


        public SettingsWindow() {
            InitializeComponent();
            _server = new Server();

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
            string savedFileSize = tool.GetConfigValue("fileSize");
            FileSizeTextBox.Text = savedFileSize;
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
            PriorityExtensionLabel.Text = language.GetString("priority_extension");
            ServerStatusLabel.Text = language.GetString("server_status");
            ServerPortLabel.Text = language.GetString("server_port");
            FileSizeLabel.Text = language.GetString("file_size");

            // Set the server labels to the language
            ServerToggleButton.Content = language.GetString("start_server"); 
            ServerToggleButton.Content = language.GetString("stop_server");

            // Set the save button to the language
            ServerPortSaveButton.Content = language.GetString("apply_button");
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
            // Convert value to int or set to 10 if not a number
            int fileSize = int.TryParse(FileSizeTextBox.Text, out int result) ? result : 10;
            tool.WriteConfigValue("fileSize", fileSize.ToString());
        }

        private void SaveTextAreas(string content, string key) {
            // Remove the empty lines
            string result = string.Join(";", content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries));
            // Save the apps in the config file
            tool.WriteConfigValue(key, result);
        }

        private void ServerPortTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {

        }

        private void ServerStatusCheckBox_Checked(object sender, RoutedEventArgs e) {

        }
        private void ServerPortSaveButton_Click(object sender, RoutedEventArgs e) {
            if (_server.IsServerRunning) {
                MessageBox.Show(language.GetString("cannot_change_port_while_running"));
                return;
            }
            if (int.TryParse(ServerPortTextBox.Text, out int newPort)) {
                _server.Port = newPort;
                MessageBox.Show(language.GetString("port_updated_successfully"));
            }
            else {
                MessageBox.Show(language.GetString("invalid_port_number"));
            }
        }

        // Start or stop the server
        private void ServerToggleButton_Click(object sender, RoutedEventArgs e) {
            if (_server != null) {
                if (!_server.IsServerRunning) {
                    _server.StartServer();
                    ServerToggleButton.Content = language.GetString("start_server"); 
                    MessageBox.Show(language.GetString("server_started_on_port") + _server.Port);

                }
                else {
                    _server.StopServer();
                    ServerToggleButton.Content = language.GetString("stop_server");
                    MessageBox.Show(language.GetString("server_stopped"));

                }
            }
        }


    }
}
