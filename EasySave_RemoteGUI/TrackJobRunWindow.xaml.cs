﻿using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using EasySave_RemoteGUI.model;

namespace EasySave_RemoteGUI {
    /// <summary>
    /// Interaction logic for TrackJobRunWindow.xaml
    /// </summary>
    public partial class TrackJobRunWindow : Window {
        private TcpClient client;

        private bool FirstTime = true;
        private List<JobState> jobStates = new();
        private List<JobState> newJobStates = new();

        // Constructor that accepts a TcpClient
        public TrackJobRunWindow(TcpClient client) {
            InitializeComponent();
            this.client = client;

            // Start listening for messages as soon as the window is opened
            ListenForMessages();
        }

        // Method to listen for messages from the server
        private async void ListenForMessages() {
            try {
                string beforeData = string.Empty;
                string afterData = string.Empty;
                while (client != null && client.Connected) {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0) {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        // Remove all content before the first <data> tag
                        message = message.Substring(message.IndexOf("<data>") + 6);
                        // Remove all content after the first </data> tag after the first <data> tag
                        message = message.Substring(0, message.IndexOf("</data>"));
                        if (message.Length < 0) {
                            continue;
                        }
                        Dispatcher.Invoke(() => {
                            newJobStates = JsonSerializer.Deserialize<List<JobState>>(message);
                            UpdateUI();
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
        private void UpdateUI() {
            if (FirstTime) {
                jobStates = newJobStates;
                JobStateListBox.ItemsSource = jobStates;
                FirstTime = false;
            }
            else {
                for (int i = 0; i < jobStates.Count; i++) {
                    jobStates[i].Name = newJobStates[i].Name;
                    jobStates[i].State = newJobStates[i].State;
                    jobStates[i].Progression = newJobStates[i].Progression;
                    JobStateListBox.Items.Refresh();
                }
            }
        }
    }
}
