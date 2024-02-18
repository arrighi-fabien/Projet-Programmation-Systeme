using System.Diagnostics;
using System.IO;

namespace EasySaveGUI.model {
    class Encrypt {

        private List<string> encryptExtensions;
        private static Encrypt instance;

        public Encrypt(string encryptExtensions) {
            // Put each extension in a list
            this.encryptExtensions = new List<string>(encryptExtensions.Split(';'));
        }

        public Encrypt GetInstance(string? encryptExtensions) {
            if (instance == null) {
                instance = new Encrypt(encryptExtensions);
            }
            return instance;
        }

        public void EncryptFile(string sourceFile, string destinationFile, string? key) {
            Process process = new();
            process.StartInfo.FileName = "cryptosoft/CryptoSoft.exe";
            // Add the arguments to the process (key is added if it's not null)
            process.StartInfo.Arguments = key == "" ? $"{sourceFile} {destinationFile}" : $"{sourceFile} {destinationFile} {key}";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

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
