using System.Windows;

namespace EasySaveGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
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

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            foreach (Window window in Application.Current.Windows) {
                if (window != this && window.IsVisible) {
                    e.Cancel = true;
                    MessageBox.Show("Please close all windows before exiting the application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void DeleteSaveJobButton_Click(object sender, RoutedEventArgs e) {
            // If no save job is selected, show an error message
            if (this.SaveJobList.Items.Count == 0) {
                MessageBox.Show("No save job selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}