using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveGUI.model {

    public class Server {
        private TcpListener serverListener;
        private bool isServerRunning;
        private const int port = 5500; // The port number for the server

        public void StartServer() {
            serverListener = new TcpListener(IPAddress.Any, port);
            serverListener.Start();
            isServerRunning = true;
            Console.WriteLine($"server_start {port}"); // Message when server starts
            ListenForClients();
        }

        public void StopServer() {
            isServerRunning = false;
            serverListener.Stop();
            Console.WriteLine("server_stopped"); // Message when server stops
        }

        private async void ListenForClients() {
            while (isServerRunning) {
                TcpClient client = await serverListener.AcceptTcpClientAsync();
                if (client != null && isServerRunning) {
                    Task.Run(() => HandleClient(client));
                }
            }
        }

        private void HandleClient(TcpClient client) {
            try {
                using (var networkStream = client.GetStream()) {
                    var buffer = new byte[client.ReceiveBufferSize];
                    while (true) {
                        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) {
                            break; // The client closed the connection
                        }
                        // Respond to the client
                        string response = "message_received"; // Simplified response
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        networkStream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
            finally {
                client.Close();
            }
        }
    }
}