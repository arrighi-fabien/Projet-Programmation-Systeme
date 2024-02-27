using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SaveJobRunWindow.xaml
    /// </summary>
    public partial class SaveJobRunWindow : Window {

        private List<JobState> jobStates = []; 
        private Language language = EasySaveGUI.model.Language.GetInstance();

        public SaveJobRunWindow(List<SaveJob> saveJobs) {
            InitializeComponent();

            // Set the text for "Executing :" dynamically based on selected language
            textBlock.Text = $"{language.GetString("executing")}";

            // Wait the window to be loaded before running the save jobs
            this.Loaded += (s, args) => {
                // Wait 1 second before running the save jobs
                Task.Delay(1000).ContinueWith(t => {
                    Dispatcher.Invoke(() => {
                        RunSaveJobs(saveJobs);
                    });
                });
            };
        }

        private void RunSaveJobs(List<SaveJob> saveJobs) {
            CountdownEvent countdownEvent = new(saveJobs.Count);
            SaveJob.countdownPriorityFile = new(saveJobs.Count);

            foreach (SaveJob saveJob in saveJobs) {
                // Create thread for each save job
                Thread thread = new(() => {
                    // Run the save job
                    int result = saveJob.SaveData(jobStates);
                    countdownEvent.Signal();
                    Dispatcher.Invoke(() => {
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                        switch (result) {
                            case 0:
                                // Success, no action needed here
                                break;
                            case 1:
                                mainWindow.ShowErrorMessageBox(language.GetString("error_professionalapp"));
                                break;
                            case 2:
                                mainWindow.ShowErrorMessageBox(language.GetString("error_savejob"));
                                break;
                        }
                    });
                });
                thread.Start();
            }

            countdownEvent.Wait();

            MessageBox.Show(language.GetString("savejob_finished"), "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}
