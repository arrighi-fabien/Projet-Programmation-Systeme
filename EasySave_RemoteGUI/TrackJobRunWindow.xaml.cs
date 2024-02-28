using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Text.Json; 
using System.IO;

namespace EasySave_RemoteGUI {
    /// <summary>
    /// Interaction logic for TrackJobRunWindow.xaml
    /// </summary>
    public partial class TrackJobRunWindow : Window {
        private TcpClient client;

        // Constructor that accepts a TcpClient
        public TrackJobRunWindow(TcpClient client) {
            InitializeComponent();
            this.client = client;
            ListenForMessages(); // Start listening for messages as soon as the window is opened
        }

        // Method to listen for messages from the server
        private async void ListenForMessages() {
            try {
                while (client != null && client.Connected) {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0) {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Dispatcher.Invoke(() => {
                        });
                    }
                }
            }
            catch (IOException ex) {
                Dispatcher.Invoke(() => {
                    Console.WriteLine("Connection has been closed.");
                });
            }
            catch (ObjectDisposedException ex) {
                Dispatcher.Invoke(() => {
                    Console.WriteLine("Connection has been closed.");
                });
            }
            catch (Exception ex) {
                Dispatcher.Invoke(() => {
                    MessageBox.Show($"Error receiving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        // Method to update the UI with job state data
        private void UpdateUI(List<dynamic> jobStates) {
            // Clear existing items
            JobStateListBox.Items.Clear();

            // Add new items
            foreach (var jobState in jobStates) {
                JobStateListBox.Items.Add($"Name: {jobState.Name}, Progress: {jobState.Progress}%");
            }
        }
    }
}
