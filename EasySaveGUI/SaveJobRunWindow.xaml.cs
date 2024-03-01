using System.Windows;
using System.Windows.Controls;
using EasySaveGUI.model;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private Server server;
        private List<SaveJob> saveJobs;

        public SaveJobRunWindow(List<SaveJob> saveJobs, Server server) {
            InitializeComponent();
            Refresh();

            this.saveJobs = saveJobs;
            this.server = server;

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
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            CountdownEvent countdownEvent = new(saveJobs.Count);
            SaveJob.countdownPriorityFile = new(saveJobs.Count);
            var lastSentJson = ""; // Keep track of the last sent JSON to avoid redundant sends

            List<string> folders = [];
            foreach (SaveJob saveJob in saveJobs) {

                if (!folders.Contains(saveJob.SourceFolder)) {
                    folders.Add(saveJob.SourceFolder);
                }
                else {
                    mainWindow.ShowErrorMessageBox(language.GetString("error_samefolder"));
                    this.Close();
                    return;
                }
            }

                // Create a thread for each save job
                Thread thread = new(() => {
                    CancellationTokenSource cancellationToken = new();
                    cancellationTokenList.Add(cancellationToken);
                    // Run the save job
                    int result = saveJob.SaveData(jobStates, manualResetEvent, cancellationToken.Token);
                    Dispatcher.Invoke(() => {
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
                // Refresh SaveJobRun every 500ms
                while (countdownEvent.CurrentCount > 0) {
                    Dispatcher.Invoke(() => {
                        SaveJobRun.Items.Refresh();

                        string currentJson = System.Text.Json.JsonSerializer.Serialize(jobStates);
                        // Check if there has been a change in the progression
                        if (server != null && currentJson != lastSentJson) {
                            server.BroadcastProgress(currentJson);
                            // Update the last sent JSON
                            lastSentJson = currentJson;
                        }
                    });
                    // Wait 500ms before refreshing the list view
                    Thread.Sleep(1000); 
                }
                // Final update after all jobs are completed
                Dispatcher.Invoke(() => {
                    SaveJobRun.Items.Refresh();
                    // Only send final state if there are changes
                    string finalJson = System.Text.Json.JsonSerializer.Serialize(jobStates);
                    if (server != null && finalJson != lastSentJson) {
                        server.BroadcastProgress(finalJson);
                    }
                });
            });
            threadRefreshListView.Start();

            Thread threadEnd = new(() => {
                // Wait for all save jobs to finish
                countdownEvent.Wait();
                Dispatcher.Invoke(() => {

                    MessageBox.Show(language.GetString("savejob_finished"), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Close the window
                });
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

        public void Refresh() {
            // Refresh btn
            Play_Button.Content = language.GetString("btn_play");
            Pause_Button.Content = language.GetString("btn_pause");
            Stop_Button.Content = language.GetString("btn_stop");
            // Refresh headers
            if (SaveJobRun.View is GridView gridView) {
                gridView.Columns[0].Header = language.GetString("header_name");
                gridView.Columns[1].Header = language.GetString("progress_bar");
                gridView.Columns[2].Header = language.GetString("header_progression");
                gridView.Columns[3].Header = language.GetString("header_status");
            }
        }
        private void UpdateAndSendProgress() {
            var jobStates = new List<JobState>(); 
            string json = System.Text.Json.JsonSerializer.Serialize(jobStates);
            server.BroadcastProgress(json);
        }
    }
}