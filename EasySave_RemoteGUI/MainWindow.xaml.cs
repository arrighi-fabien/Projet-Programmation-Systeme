using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySave_RemoteGUI.model;

namespace EasySave_RemoteGUI {
    public partial class MainWindow : Window {
        private TcpClient? client;
        private bool isConnected = false;
        private readonly Language language = EasySave_RemoteGUI.model.Language.GetInstance();

        public MainWindow() {
            InitializeComponent();
            Refresh();
        }

        public static class GlobalSettings {
            // Default server IP and port
            public static string server_ip { get; set; } = "127.0.0.1"; 
            public static int server_port { get; set; } = 5500; 
        }

        // Handles the click event of the connect/disconnect button
        private void ConnectDisconnectButton_Click(object sender, RoutedEventArgs e) {
            if (!isConnected) {
                ConnectToServer();
            }
            else {
                DisconnectFromServer();
            }
        }

        // Initiates connection to the server
        private void ConnectToServer() {
            try {
                // Reload global settings in case they were changed
                GlobalSettings.server_ip = Tool.GetInstance().GetConfigValue("server_ip");
                GlobalSettings.server_port = int.Parse(Tool.GetInstance().GetConfigValue("server_port"));

                client = new TcpClient(GlobalSettings.server_ip, GlobalSettings.server_port);
                isConnected = true;
                UpdateButtonForDisconnect();
                MessageBox.Show(language.GetString("connected_successfully"), language.GetString("connection_status"), MessageBoxButton.OK, MessageBoxImage.Information);
                TrackJobRunWindow trackJobRunWindow = new TrackJobRunWindow(client);
                trackJobRunWindow.Show();
            }
            catch (Exception ex) {
                MessageBox.Show($"{language.GetString("failed_connect")}: {ex.Message}", language.GetString("connection_error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Closes the connection to the server
        private void DisconnectFromServer() {
            if (client != null) {
                client.Close();
                client = null;
                isConnected = false;
                UpdateButtonForConnect();
                MessageBox.Show(language.GetString("disconnected_successfully"), language.GetString("connection_status"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Updates the button to show "Disconnect" and prepares for disconnection
        private void UpdateButtonForDisconnect() {
            ConnectDisconnectButton.Content = language.GetString("disconnect_button");
        }

        // Updates the button to show "Connect" and prepares for connection
        private void UpdateButtonForConnect() {
            ConnectDisconnectButton.Content = language.GetString("connect_button");
        }

        private void Refresh() {
            if (isConnected) {
                UpdateButtonForDisconnect();
            }
            else {
                UpdateButtonForConnect();
            }
            SettingsButton.Content = language.GetString("settings");
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            SettingsWindow settingsWindow = new SettingsWindow();
            // Open settings window as a dialog
            settingsWindow.ShowDialog(); 
            Refresh();
        }
    }
}
