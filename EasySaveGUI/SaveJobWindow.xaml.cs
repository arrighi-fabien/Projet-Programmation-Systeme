using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SaveJobWindow.xaml
    /// </summary>
    public partial class SaveJobWindow : Window {

        private SaveJob? saveJob;
        private Language Language = Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();

        public SaveJobWindow() {
            InitializeComponent();
        }

        public SaveJobWindow(SaveJob saveJob) {
            InitializeComponent();
            this.saveJob = saveJob;
            this.SaveJobName.Text = saveJob.Name;
            this.SourceFolder.Text = saveJob.SourceFolder;
            this.DestinationFolder.Text = saveJob.DestinationFolder;
            this.TypeComboBox.SelectedIndex = saveJob.GetType().Name == "FullSave" ? 0 : 1;
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            if (this.SaveJobName.Text == "" || this.SourceFolder.Text == "" || this.DestinationFolder.Text == "") {
                mainWindow.ShowErrorMessageBox("Please fill all the fields");
                return;
            }
            if (!tool.PathDirectoryIsValid(this.SourceFolder.Text) || !tool.PathDirectoryIsValid(this.DestinationFolder.Text)) {
                mainWindow.ShowErrorMessageBox("Please enter a valid path");
                return;
            }

            SaveJob saveJob;
            if (this.TypeComboBox.SelectedIndex == 0) {
                saveJob = new FullSave(this.SaveJobName.Text, this.SourceFolder.Text, this.DestinationFolder.Text);
            }
            else {
                saveJob = new DifferentialSave(this.SaveJobName.Text, this.SourceFolder.Text, this.DestinationFolder.Text);
            }

            if (this.saveJob != null) {
                mainWindow.UpdateSaveJob(this.saveJob, saveJob);
            }
            else {
                mainWindow.AddSaveJob(saveJob);
            }

            this.Close();
        }

    }
}
