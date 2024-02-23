using System.Diagnostics;
using System.Text;

namespace CryptoSoft {
    internal class Program {
        /// <summary>
        /// Main method to encrypt / decrypt a file using the XOR algorithm.
        /// </summary>
        /// <param name="args">The first argument is the source file, the second argument is the destination file and the third argument is the key</param>
        /// <returns>The time it took to encrypt the file in milliseconds or -1 if an error occurred</returns>
        static int Main(string[] args) {

            string sourceFile;
            string destinationFile;
            string key;

            try {
                sourceFile = args[0];
                destinationFile = args[1];
                // if a third argument is provided, it is the key
                if (args.Length > 2) {
                    key = args[2];
                }
                key = "2784070415730864970";
            }
            catch (Exception) {
                return -1;
            }

            if (!File.Exists(sourceFile)) {
                return -1;
            }

            try {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                // Convert text to bytes
                byte[] data = File.ReadAllBytes(sourceFile);
                byte[] byteKey = Encoding.UTF8.GetBytes(key);

                byte[] encryptedData = EncryptData(data, byteKey);

                // Write the encrypted data to a file
                File.WriteAllBytes(destinationFile, encryptedData);

                stopwatch.Stop();
                int elapsedTime = (int)stopwatch.Elapsed.TotalMilliseconds;
                Console.WriteLine("File encrypted in " + elapsedTime + " milliseconds.");

                return elapsedTime;

            }
            catch (Exception) {
                Console.WriteLine("Error: Encrypted data could not be written to the file.");
                return -1;
            }
        }

        /// <summary>
        /// Encrypts the data using the key using the XOR algorithm using multiple threads.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static byte[] EncryptData(byte[] data, byte[] key) {
            // Adapt number of threads to the file size
            int threads = Environment.ProcessorCount;
            int partitionSize = data.Length / threads;
            byte[] encryptedData = new byte[data.Length];

            Parallel.For(0, threads, i => {
                int start = i * partitionSize;
                int end = (i == threads - 1) ? data.Length : start + partitionSize;

                for (int j = start; j < end; j++) {
                    encryptedData[j] = (byte)(data[j] ^ key[j % key.Length]);
                }
            });

            return encryptedData;
        }
    }
}
