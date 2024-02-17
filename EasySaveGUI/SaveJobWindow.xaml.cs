using System.Windows;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SaveJobWindow.xaml
    /// </summary>
    public partial class SaveJobWindow : Window {

        SaveJob? saveJob;

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
            if (this.SaveJobName.Text == "" || this.SourceFolder.Text == "" || this.DestinationFolder.Text == "") {
                MessageBox.Show("Please fill all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.UpdateSaveJob(this.saveJob, saveJob);
            }
            else {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.AddSaveJob(saveJob);
            }
            this.Close();
        }
    }
}
