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
        // Attributes
        private ConcurrentBag<TcpClient> clients = [];
        private TcpListener serverListener;
        private CancellationTokenSource cts;
        private static Server instance;
        private List<CancellationTokenSource> cancellationTokenList = [];
        private ManualResetEvent manualResetEvent = new(true);
        private List<JobState> jobs = [];
        private bool isServerRunning;
        private int _port = 5500;

        // Properties
        public bool IsServerRunning {
            get {
                return isServerRunning;
            }
        }

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

        /// <summary>
        /// Constructor for the Server class
        /// </summary>
        private Server() {

        }

        public static Server Instance {
            get {
                instance ??= new Server();
                return instance;
            }
        }

        public class CommandObject {
            public string Command {
                get; set;
            }

        }

        /// <summary>
        /// Start the server
        /// </summary>
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

        /// <summary>
        /// Stop the server
        /// </summary>
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

        /// <summary>
        /// Listen for incoming client connections
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to stop the listener</param>
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

        /// <summary>
        /// Handle an individual client connection
        /// </summary>
        /// <param name="client">Connected TcpClient</param>
        /// <param name="cancellationToken">Cancellation token to stop handling the client</param>
        /// <returns></returns>
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
                        // Deserialize the received data into a CommandObject
                        CommandObject commandObject = JsonSerializer.Deserialize<CommandObject>(receivedData);
                        if (commandObject == null) {
                            MessageBox.Show("Invalid command format.");
                            continue; 
                        }

                        MessageBox.Show($"Command received: {commandObject.Command}");

                        // Execute command based on deserialized CommandObject
                        Dispatcher.CurrentDispatcher.Invoke(() => {
                            switch (commandObject.Command.ToUpper()) {
                                case "PAUSE":
                                    PauseJob();
                                    break;
                                case "RESUME":
                                    ResumeJob();
                                    break;
                                case "STOP":
                                    StopJob();
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

        /// <summary>
        /// Broadcast progress to all connected clients
        /// </summary>
        /// <param name="data">Data to send to clients</param>
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

        /// <summary>
        /// Pause SaveJobs
        /// </summary>
        private void PauseJob() {
            MessageBox.Show($"Trying to pause jobs");
        }

        /// <summary>
        /// Resume SaveJobs
        /// </summary>
        private void ResumeJob() {
            MessageBox.Show($"Trying to resume jobs");
        }

        /// <summary>
        /// Stop SaveJobs
        /// </summary>
        private void StopJob() {
            MessageBox.Show($"Trying to stop jobs");
        }

    }
}
