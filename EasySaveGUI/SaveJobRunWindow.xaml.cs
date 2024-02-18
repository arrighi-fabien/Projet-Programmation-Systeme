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

            // Wait the window to be loaded before running the save jobs
            this.Loaded += (s, args) => {
                RunSaveJobs(saveJobs);
            };
        }

        private void RunSaveJobs(List<SaveJob> saveJobs) {
            foreach (SaveJob saveJob in saveJobs) {
                runningSaveJobTextBlock.Text = saveJob.Name;
                saveJob.SaveData(jobStates);
            }

            saveJobProgressBar.Value = 100;

            MessageBox.Show(language.GetString("savejob_finished"), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
