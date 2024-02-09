namespace EasySaveConsole.model {

    // Represents the current state of a job, inheriting from LogSystem
    public class JobState : LogSystem {

        // Variables for state, totalFiles, totalFilesSize, filesLeft, filesSizeLeft, and progression
        private string state;
        private uint totalFiles;
        private ulong totalFilesSize;
        private uint filesLeft;
        private ulong filesSizeLeft;
        private int progression;

        // Properties for state, totalFiles, totalFilesSize, filesLeft, filesSizeLeft, and progression
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

        /// <summary>
        /// Constructor for JobState
        /// </summary>
        /// <param name="name">Name of savejob</param>
        /// <param name="sourceFile">Source files of the savejob</param>
        /// <param name="destinationFile">Destination files of the savejob</param>
        /// <param name="state">State of the savejob</param>
        /// <param name="totalFiles">Total files in the source folder</param>
        public JobState(string name, string sourceFile, string destinationFile, string state, uint totalFiles) : base(name, sourceFile, destinationFile) {
            State = state;
            TotalFiles = totalFiles;
            FilesLeft = totalFiles;
        }

        /// <summary>
        /// Mark the savejob as finished
        /// </summary>
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
        
        /// <summary>
        /// Get the total size of files in the specified directory
        /// </summary>
        /// <param name="path">Path of the directory</param>
        public void GetTotalFilesSize(string path) {
            try {
                // Check if the path exists
                if (!Directory.Exists(path)) {
                    Console.WriteLine("Le chemin spécifié n'existe pas.");
                    return;
                }

                // Get the list of files in the directory
                string[] files = Directory.GetFiles(path);
                foreach (string file in files) {
                    // Get the file info
                    FileInfo fileInfo = new(file);
                    // Increment the total size of files
                    TotalFilesSize += (uint)fileInfo.Length;
                    // Increment the size of files left
                    FilesSizeLeft += (uint)fileInfo.Length;
                }

                // Get the list of subdirectories in the directory
                string[] subdirectories = Directory.GetDirectories(path);
                
                foreach (string subdirectory in subdirectories) {
                    // Recursively call the method to get the total size of files in the subdirectory
                    GetTotalFilesSize(subdirectory);
                }

            }
            // Catch any exceptions
            catch {

            }
        }

    }
}