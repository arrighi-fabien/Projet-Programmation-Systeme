using System.Windows;
using System.Windows.Controls;
using EasySaveGUI.model;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for SaveJobWindow.xaml
    /// </summary>
    public partial class SaveJobWindow : Window {

        private SaveJob? saveJob;
        private Language language = EasySaveGUI.model.Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();

        public SaveJobWindow() {
            InitializeComponent();
            // Change label to the language
            this.SaveJobNameLabel.Content = language.GetString("label_savejob_name");
            this.SaveJobSourceLabel.Content = language.GetString("label_source_folder");
            this.SaveJobDestinationLabel.Content = language.GetString("label_destination_folder");
            this.SaveJobTypeLabel.Content = language.GetString("label_save_type");
            this.SaveButton.Content = language.GetString("save_button");

            // Update ComboBox items to the selected language
            if (TypeComboBox.Items.Count >= 2) { // Assure that there are at least two items
                (TypeComboBox.Items[0] as ComboBoxItem).Content = language.GetString("full_save");
                (TypeComboBox.Items[1] as ComboBoxItem).Content = language.GetString("differential_save");
            }
        }

        public SaveJobWindow(SaveJob saveJob) : this() { // Call the base constructor
            this.saveJob = saveJob;
            DataContext = this.saveJob;
            // Set the selected index based on the type of saveJob
            this.TypeComboBox.SelectedIndex = saveJob is FullSave ? 0 : 1;
        }


        private void Save_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            if (this.SaveJobName.Text == "" || this.SourceFolder.Text == "" || this.DestinationFolder.Text == "") {
                mainWindow.ShowErrorMessageBox(language.GetString("error_fill_all_fields"));
                return;
            }
            if (!tool.PathDirectoryIsValid(this.SourceFolder.Text) || !tool.PathDirectoryIsValid(this.DestinationFolder.Text)) {
                mainWindow.ShowErrorMessageBox(language.GetString("error_enter_valid_path"));
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
