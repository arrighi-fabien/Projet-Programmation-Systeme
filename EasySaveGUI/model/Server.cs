using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Threading;
using System.Text.Json;

namespace EasySaveGUI.model {

    // Class to manage the server
    public class Server {
        private ConcurrentBag<TcpClient> clients = new ConcurrentBag<TcpClient>();
        private TcpListener serverListener;
        private CancellationTokenSource cts;
        private static Server instance;
        private List<CancellationTokenSource> cancellationTokenList = [];
        private ManualResetEvent manualResetEvent = new(true);
        private List<JobState> jobs = new List<JobState>();

        // Private constructor to prevent instantiation
        private Server() {
        }

        public bool IsServerRunning {
            get {
                return isServerRunning;
            }
        }
        public class CommandObject {
            public string Command {
                get; set;
            }
            public string JobName {
                get; set;
            }
        }
        private bool isServerRunning;
        // The port the server listens on
        private int _port = 5500; 
        public int Port {
            get {
                return _port;
            }
            set {
                // Check if the server is running before changing the port
                if (!isServerRunning) { 
                    _port = value;
                }
                else {
                    throw new InvalidOperationException("Cannot change port while server is running.");
                }
            }
        }

        // Start the server
        public void StartServer() {
            try {
                // Initialize the CancellationTokenSource for this server session
                cts = new CancellationTokenSource();

                serverListener = new TcpListener(IPAddress.Any, Port);
                serverListener.Start();
                isServerRunning = true;
                Console.WriteLine($"Server started on port {Port}");
                // Pass the cancellation token to ListenForClients
                ListenForClients(cts.Token);
            }
            catch (Exception ex) {
                // Log or display the exception
                Console.WriteLine($"Error starting server: {ex.Message}");
            }
        }

        // Stop the server
        public void StopServer() {
            try {
                // Check if the server is already stopped to avoid redundancy
                if (!isServerRunning)
                    return;

                // Begin additional try block for cancellation specific code
                try {
                    // Signal cancellation to waiting tasks
                    cts.Cancel();
                }
                catch (Exception ex) {
                    // Handle exceptions specifically related to task cancellation
                    Console.WriteLine($"Error cancelling tasks: {ex.Message}");
                }

                // Ensure the server no longer accepts new connections
                serverListener?.Stop();
                // Clear the list of clients
                serverListener = null; 

                // Update the server status
                isServerRunning = false;
                Console.WriteLine("Server stopped");
            }
            catch (Exception ex) {
                // Handle general exceptions related to stopping the server
                Console.WriteLine($"Error stopping server: {ex.Message}");
            }
            finally {
                // Ensure these actions happen regardless of any exceptions
                if (cts != null) {
                    // Reset the CancellationTokenSource for the next start
                    cts.Dispose();
                    cts = null;
                }
            }
        }

        // Listen for incoming client connections
        private async void ListenForClients(CancellationToken cancellationToken) {
            while (isServerRunning && !cancellationToken.IsCancellationRequested) {
                try {
                    TcpClient client = await serverListener.AcceptTcpClientAsync().ConfigureAwait(false);
                    if (client != null && isServerRunning) {
                        // Pass the cancellation token to the handling method if necessary
                        Task.Run(() => HandleClient(client, cancellationToken), cancellationToken);
                    }
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.OperationAborted || ex.SocketErrorCode == SocketError.Interrupted) {
                    // Log or handle the expected exception when the serverListener is stopped
                    Console.WriteLine("Server stopping, accept operation cancelled.");
                    break; 
                }
                catch (Exception ex) {
                    // Log unexpected exceptions
                    Console.WriteLine($"Unexpected exception in ListenForClients: {ex.Message}");
                    break; 
                }
            }
        }

        // Handle the client connection
        private async Task HandleClient(TcpClient client, CancellationToken cancellationToken) {
            clients.Add(client);
            try {
                using (var networkStream = client.GetStream()) {
                    var buffer = new byte[client.ReceiveBufferSize];
                    while (!cancellationToken.IsCancellationRequested) {
                        int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        if (bytesRead == 0)
                            break; 

                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                        MessageBox.Show($"Received command: {receivedData}");

                        // Deserialize the received data into a CommandObject
                        CommandObject commandObject = JsonSerializer.Deserialize<CommandObject>(receivedData);
                        MessageBox.Show($"CommandObject deserialized: Command = {commandObject.Command}, JobName = {commandObject.JobName}");
                        if (commandObject == null) {
                            MessageBox.Show("Invalid command format.");
                            continue; 
                        }

                        MessageBox.Show($"Command received: {commandObject.Command}, JobName: {commandObject.JobName}");

                        // Execute command based on deserialized CommandObject
                        Dispatcher.CurrentDispatcher.Invoke(() => {
                            switch (commandObject.Command.ToUpper()) {
                                case "PAUSE":
                                    PauseJobByName(commandObject.JobName);
                                    break;
                                case "RESUME":
                                    ResumeJobByName(commandObject.JobName);
                                    break;
                                case "STOP":
                                    StopJobByName(commandObject.JobName);
                                    break;
                                default:
                                    MessageBox.Show($"Unknown command: {commandObject.Command}");
                                    break;
                            }
                        });
                    }
                }
            }
            catch (OperationCanceledException) {
                // This exception is thrown when cancellationToken.ThrowIfCancellationRequested() is called
                Console.WriteLine("Operation was cancelled. This is expected during server shutdown.");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.OperationAborted || ex.SocketErrorCode == SocketError.Interrupted) {
                // This is expected when the server is shutting down and cancels pending operations
                Console.WriteLine("Socket operation was aborted, likely due to server stop.");
            }
            catch (Exception ex) {
                // Log unexpected exceptions
                Console.WriteLine($"An unexpected exception occurred: {ex.Message}");
            }
            finally {
                // Always ensure the client is closed properly
                client?.Close();
            }
        }

        // Broadcast progress to all connected clients
        public void BroadcastProgress(string data) {
            // Add begining markup and final markup to the data
            data = "<data>" + data + "</data>";
            foreach (var client in clients) {
                if (client.Connected) {
                    var buffer = Encoding.UTF8.GetBytes(data);
                    client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }

        public static Server Instance {
            get {
                if (instance == null) {
                    instance = new Server();
                }
                return instance;
            }
        }
        // Pause the job with the given name
        private void PauseJobByName(string jobName) {
            MessageBox.Show($"Trying to find job: {jobName}");
            var job = FindJobByName(jobName);
            if (job != null) {
                job.State = "PAUSED"; 
                job.ManualResetEvent.Reset(); 
                MessageBox.Show($"Paused job: {jobName}");
            }
            else {
                MessageBox.Show($"Job not found: {jobName}");
            }
        }

        private void ResumeJobByName(string jobName) {
            MessageBox.Show($"Trying to find job: {jobName}");
            var job = FindJobByName(jobName);
            if (job != null) {
                Dispatcher.CurrentDispatcher.Invoke(() => {
                    job.ManualResetEvent?.Set();
                    MessageBox.Show($"Resumed job: {jobName}");

                });
            }
            else {
                MessageBox.Show($"Job not found: {jobName}");
            }
        }

        // Stop the job with the given name
        private void StopJobByName(string jobName) {
            MessageBox.Show($"Trying to find job: {jobName}");
            var job = FindJobByName(jobName);
            if (job != null) {
                Dispatcher.CurrentDispatcher.Invoke(() => {
                    job.ManualResetEvent?.Set();
                    //job.CancellationTokenSource?.Cancel(); 
                    MessageBox.Show($"Stopped job: {jobName}");
                });
            }
            else {
                MessageBox.Show($"Job not found: {jobName}");
            }
        }

        private JobState FindJobByName(string jobName) {
            object job = null;
            MessageBox.Show($"Job search result for '{jobName}': found = {(job != null ? "Yes" : "No")}");
            return jobs.FirstOrDefault(job => job.Name.Equals(jobName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
