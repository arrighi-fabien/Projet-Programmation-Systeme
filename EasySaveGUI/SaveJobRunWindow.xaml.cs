﻿using System.Windows;
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
            foreach (SaveJob saveJob in saveJobs) {
                runningSaveJobTextBlock.Text = saveJob.Name;
                int result = saveJob.SaveData(jobStates);
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                switch (result) {
                    case 0:
                        break;
                    case 1:
                        mainWindow.ShowErrorMessageBox(language.GetString("error_professionalapp"));
                        break;
                    case 2:
                        mainWindow.ShowErrorMessageBox(language.GetString("error_savejob"));
                        break;
                }
            }

            MessageBox.Show(language.GetString("savejob_finished"), "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}