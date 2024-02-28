using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SaveJobRunWindow.xaml
    /// </summary>
    public partial class SaveJobRunWindow : Window {

        private List<JobState> jobStates = [];
        private Language language = EasySaveGUI.model.Language.GetInstance();
        private List<Thread> saveJobThreadList = [];
        private List<CancellationTokenSource> cancellationTokenList = [];
        private ManualResetEvent manualResetEvent = new(true);

        public SaveJobRunWindow(List<SaveJob> saveJobs) {
            InitializeComponent();
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
                    CancellationTokenSource cancellationToken = new();
                    cancellationTokenList.Add(cancellationToken);
                    // Run the save job
                    int result = saveJob.SaveData(jobStates, manualResetEvent, cancellationToken.Token);
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
                    countdownEvent.Signal();
                });
                thread.Start();
                saveJobThreadList.Add(thread);
            }
            Thread.Sleep(50);
            SaveJobRun.ItemsSource = jobStates;
            Thread threadRefreshListView = new(() => {
                // Refresh SaveJobRun every 100ms
                while (countdownEvent.CurrentCount > 0) {
                    Dispatcher.Invoke(() => {
                        SaveJobRun.Items.Refresh();
                    });
                    Thread.Sleep(10);
                }
                Dispatcher.Invoke(() => {
                    SaveJobRun.Items.Refresh();
                });
            });
            threadRefreshListView.Start();

            Thread threadEnd = new(() => {
                // Wait for all save jobs to finish
                countdownEvent.Wait();
                MessageBox.Show(language.GetString("savejob_finished"), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            });
            threadEnd.Start();
        }

        private void PauseSaveJob_Click(object sender, RoutedEventArgs e) {
            manualResetEvent.Reset();
        }

        private void ResumeSaveJob_Click(object sender, RoutedEventArgs e) {
            manualResetEvent.Set();
        }

        private void StopSaveJob_Click(object sender, RoutedEventArgs e) {
            manualResetEvent.Set();
            foreach (CancellationTokenSource cancellationToken in cancellationTokenList) {
                cancellationToken.Cancel();
            }
        }

    }
}
