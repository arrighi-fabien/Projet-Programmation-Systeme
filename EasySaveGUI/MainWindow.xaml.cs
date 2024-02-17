using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly Language language = EasySaveGUI.model.Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();
        private List<SaveJob> saveJobs;

        public MainWindow() {
            InitializeComponent();
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
                MessageBox.Show(language.GetString("no_savejob_selected"), language.GetString("error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else {
                List<int> saveJobToDelete = [];
                // Display all index of the selected savejobs
                foreach (SaveJob saveJob in this.SaveJobList.SelectedItems) {
                    int index = this.SaveJobList.Items.IndexOf(saveJob);
                    MessageBoxResult result = MessageBox.Show(language.GetString("delete_savejob") + " " + saveJobs[index].Name + " ?", language.GetString("delete"), MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
                MessageBox.Show(language.GetString("no_savejob_selected"), language.GetString("error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else {
                // If multiple save jobs are selected, show an error message
                if (this.SaveJobList.SelectedItems.Count > 1) {
                    MessageBox.Show(language.GetString("multiple_savejob_selected"), language.GetString("error"), MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show(language.GetString("error_close_windows"), language.GetString("error"), MessageBoxButton.OK, MessageBoxImage.Error);
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
        }

        private void LaunchSaveJobButton_Click(object sender, RoutedEventArgs e) {
            // If no save job is selected, show an error message
            if (this.SaveJobList.SelectedItems.Count == 0) {
                MessageBox.Show(language.GetString("no_savejob_selected"), language.GetString("error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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

    }
}