using System.Diagnostics;
using System.IO;

namespace EasySaveGUI.model {
    /// <summary>
    /// Class to encrypt files
    /// </summary>
    class Encrypt {

        private List<string> encryptExtensions;
        private static Encrypt instance;

        /// <summary>
        /// Constructor for the Encrypt class
        /// </summary>
        public Encrypt() {
            // Put each extension in a list
            string encryptExtensions = Tool.GetInstance().GetConfigValue("encryptExtensions");
            this.encryptExtensions = new List<string>(encryptExtensions.Split(';'));
        }

        /// <summary>
        /// Get the instance of the Encrypt class
        /// </summary>
        /// <returns>Instance of the Encrypt class</returns>
        public Encrypt GetInstance() {
            if (instance == null) {
                instance = new Encrypt();
            }
            return instance;
        }

        /// <summary>
        /// Encrypt a file with the CryptoSoft program
        /// </summary>
        /// <param name="sourceFile">Source file to encrypt</param>
        /// <param name="destinationFile">Destination file to encrypt</param>
        /// <param name="key">Key to encrypt the file</param>
        public void EncryptFile(string sourceFile, string destinationFile, string? key) {
            Process process = new();
            process.StartInfo.FileName = "cryptosoft/CryptoSoft.exe";
            // Add the arguments to the process (key is added if it's not null)
            process.StartInfo.Arguments = key == "" ? $"{sourceFile} {destinationFile}" : $"{sourceFile} {destinationFile} {key}";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Return if the file should be encrypted
        /// </summary>
        /// <param name="file">Path of the file</param>
        /// <returns>True if the file should be encrypted, false otherwise</returns>
        public bool IsToEncrypt(string file) {
            // Check if the file extension is in the list
            string extension = Path.GetExtension(file);
            if (extension == "") {
                return false;
            }
            // Remove the dot from the extension
            extension = extension.Substring(1);
            return encryptExtensions.Contains(extension);
        }
    }
}
