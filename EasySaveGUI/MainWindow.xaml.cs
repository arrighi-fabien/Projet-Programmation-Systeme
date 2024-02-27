using EasySaveGUI.model;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly Language language = EasySaveGUI.model.Language.GetInstance();
        private readonly Server server = new EasySaveGUI.model.Server();
        private readonly Tool tool = Tool.GetInstance();
        private List<SaveJob> saveJobs;

        public MainWindow() {
            UniqueInstance();
            if (!Directory.Exists("logs")) {
                Directory.CreateDirectory("logs");
            }
            InitializeComponent();
            server.StartServer();
            saveJobs = tool.GetSavedSaveJob();
            this.SaveJobList.ItemsSource = saveJobs;
            Refresh();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            OpenNewWindow(new SettingsWindow());
        }

        private void SaveJobButton_Click(object sender, RoutedEventArgs e) {
            OpenNewWindow(new SaveJobWindow());
        }

        private void OpenNewWindow(Window window) {
            this.IsEnabled = false;
            window.Owner = this;
            window.Closed += (s, args) => { this.IsEnabled = true; };
            window.Show();
        }

        private void DeleteSaveJobButton_Click(object sender, RoutedEventArgs e) {
            // If no save job is selected, show an error message
            if (this.SaveJobList.SelectedItems.Count == 0) {
                ShowErrorMessageBox(language.GetString("error_no_savejob_selected"));
                return;
            }
            else {
                List<int> saveJobToDelete = [];
                // Display all index of the selected savejobs
                foreach (SaveJob saveJob in this.SaveJobList.SelectedItems) {
                    int index = this.SaveJobList.Items.IndexOf(saveJob);
                    MessageBoxResult result = MessageBox.Show(language.GetString("delete") + " " + saveJobs[index].Name + " ?", language.GetString("popup_delete_savejob"), MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    // If the user click on yes, delete the savejob
                    if (result == MessageBoxResult.Yes) {
                        saveJobToDelete.Add(index);
                    }
                }
                // Delete all selected savejobs
                int offset = 0;
                foreach (int index in saveJobToDelete) {
                    saveJobs.RemoveAt(index - offset);
                    offset++;
                    tool.WriteSavedSaveJob(saveJobs);
                }
                this.SaveJobList.Items.Refresh();
            }
        }

        private void UpdateSaveJobButton_Click(object sender, RoutedEventArgs e) {
            // If no save job is selected, show an error message
            if (this.SaveJobList.SelectedItems.Count == 0) {
                ShowErrorMessageBox(language.GetString("error_no_savejob_selected"));
                return;
            }
            else {
                // If multiple save jobs are selected, show an error message
                if (this.SaveJobList.SelectedItems.Count > 1) {
                    ShowErrorMessageBox(language.GetString("error_multiple_savejob_selected"));
                    return;
                }
                else {
                    // Open the save job window with the selected save job
                    int index = this.SaveJobList.Items.IndexOf(this.SaveJobList.SelectedItems[0]);
                    OpenNewWindow(new SaveJobWindow(saveJobs[index]));
                }
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            foreach (Window window in Application.Current.Windows) {
                if (window != this && window.IsVisible) {
                    e.Cancel = true;
                    ShowErrorMessageBox(language.GetString("error_close_windows"));
                    return;
                }
            }
        }

        public void Refresh() {
            this.SaveJobList.Items.Refresh();
            // Button text
            this.LaunchSaveJobButton.Content = language.GetString("menu_execute_save");
            this.CreateSaveJobButton.Content = language.GetString("menu_create_save");
            this.UpdateSaveJobButton.Content = language.GetString("menu_update_save");
            this.DeleteSaveJobButton.Content = language.GetString("menu_delete_save");
            this.SettingsButton.Content = language.GetString("settings");
            // Refresh headers
            if (this.SaveJobList.View is GridView gridView) {
                gridView.Columns[0].Header = language.GetString("header_name");
                gridView.Columns[1].Header = language.GetString("header_source");
                gridView.Columns[2].Header = language.GetString("header_destination");
                gridView.Columns[3].Header = language.GetString("header_type");
            }
        }

        private void LaunchSaveJobButton_Click(object sender, RoutedEventArgs e) {
            // If no save job is selected, show an error message
            if (this.SaveJobList.SelectedItems.Count == 0) {
                ShowErrorMessageBox(language.GetString("error_no_savejob_selected"));
                return;
            }
            else {
                List<SaveJob> saveJobToRun = [];
                // Launch all selected save jobs
                foreach (SaveJob saveJob in this.SaveJobList.SelectedItems) {
                    saveJobToRun.Add(saveJob);
                }
                OpenNewWindow(new SaveJobRunWindow(saveJobToRun));
            }
        }

        public void AddSaveJob(SaveJob saveJob) {
            saveJobs.Add(saveJob);
            tool.WriteSavedSaveJob(saveJobs);
            this.SaveJobList.Items.Refresh();
        }

        public void UpdateSaveJob(SaveJob oldSaveJob, SaveJob newSaveJob) {
            int index = saveJobs.IndexOf(oldSaveJob);
            saveJobs[index] = newSaveJob;
            tool.WriteSavedSaveJob(saveJobs);
            this.SaveJobList.Items.Refresh();
        }

        internal void ShowErrorMessageBox(string message) {
            MessageBox.Show(message, language.GetString("popup_error"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void UniqueInstance() {
            Process proc = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(proc.ProcessName);
            if (processes.Length > 1) {
                ShowErrorMessageBox(language.GetString("error_other_instance"));
                this.Close();
            }
        }
    }
}
