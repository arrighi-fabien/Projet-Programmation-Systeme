namespace EasySaveConsole.model {
    public class JobState : LogSystem {

        private string state;
        private uint totalFiles;
        private ulong totalFilesSize;
        private uint filesLeft;
        private ulong filesSizeLeft;
        private int progression;

        public string State {
            get {
                return state;
            }
            set {
                state = value;
            }
        }

        public uint TotalFiles {
            get {
                return totalFiles;
            }
            set {
                totalFiles = value;
            }
        }

        public ulong TotalFilesSize {
            get {
                return totalFilesSize;
            }
            set {
                totalFilesSize = value;
            }
        }

        public uint FilesLeft {
            get {
                return filesLeft;
            }
            set {
                filesLeft = value;
            }
        }

        public ulong FilesSizeLeft {
            get {
                return filesSizeLeft;
            }
            set {
                filesSizeLeft = value;
            }
        }

        public int Progression {
            get {
                return progression;
            }
            set {
                progression = value;
            }
        }

        public JobState(string name, string sourceFile, string destinationFile, string state, uint totalFiles) : base(name, sourceFile, destinationFile) {
            State = state;
            TotalFiles = totalFiles;
            FilesLeft = totalFiles;
        }

        public void FinishJobState() {
            State = "END";
            TotalFiles = 0;
            TotalFilesSize = 0;
            FilesLeft = 0;
            FilesSizeLeft = 0;
            Progression = 0;
            SourceFile = "";
            DestinationFile = "";
        }

        public void GetTotalFilesSize(string path) {
            try {
                // Check if the path exists
                if (!Directory.Exists(path)) {
                    Console.WriteLine("Le chemin spécifié n'existe pas.");
                    return;
                }

                // Retrieve the size of files in the specified directory
                string[] files = Directory.GetFiles(path);
                foreach (string file in files) {
                    FileInfo fileInfo = new(file);
                    TotalFilesSize += (uint)fileInfo.Length;
                    FilesSizeLeft += (uint)fileInfo.Length;
                }

                // Retrieve the size of files in subdirectories
                string[] subdirectories = Directory.GetDirectories(path);
                foreach (string subdirectory in subdirectories) {
                    GetTotalFilesSize(subdirectory);
                }

            }
            catch {

            }
        }

    }
}
