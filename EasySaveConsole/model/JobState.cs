namespace EasySaveConsole.model {

    // Represents the current state of a job, inheriting from LogSystem
    public class JobState : LogSystem {

        // Variables for state, total files, total files size, files left, files size left, and progression
        private string state;
        private uint totalFiles;
        private ulong totalFilesSize;
        private uint filesLeft;
        private ulong filesSizeLeft;
        private int progression;

        // Property for job state (e.g., Running, Paused)
        public string State {
            get {
                return state;
            }
            set {
                state = value;
            }
        }

        // Property for the total number of files to be processed
        public uint TotalFiles {
            get {
                return totalFiles;
            }
            set {
                totalFiles = value;
            }
        }

        // Property for the total size of files to be processed
        public ulong TotalFilesSize {
            get {
                return totalFilesSize;
            }
            set {
                totalFilesSize = value;
            }
        }

        // Property for the number of files left to be processed
        public uint FilesLeft {
            get {
                return filesLeft;
            }
            set {
                filesLeft = value;
            }
        }

        // Property for the total size of files left to be processed
        public ulong FilesSizeLeft {
            get {
                return filesSizeLeft;
            }
            set {
                filesSizeLeft = value;
            }
        }

        // Property for tracking the progression percentage of the job
        public int Progression {
            get {
                return progression;
            }
            set {
                progression = value;
            }
        }

        // Constructor for JobState, initializing the state and total files
        public JobState(string name, string sourceFile, string destinationFile, string state, uint totalFiles) : base(name, sourceFile, destinationFile) {
            
            // Set the initial state of the job
            State = state;

            // Set the total number of files to be processed
            TotalFiles = totalFiles;

            // Initially, the number of files left is equal to the total number of files
            FilesLeft = totalFiles;
        }

        // Method to mark the job state as finished and reset all properties
        public void FinishJobState() {

            // Mark the state as ended
            State = "END";

            // Reset the total number of files
            TotalFiles = 0;

            // Reset the total size of files
            TotalFilesSize = 0;

            // Reset the number of files left
            FilesLeft = 0;

            // Reset the size of files left
            FilesSizeLeft = 0;

            // Reset the progression to 0%
            Progression = 0;

            // Clear the source file path
            SourceFile = "";

            // Clear the destination file path
            DestinationFile = "";
        }
        
        // Method to calculate the progression percentage of the job
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

                    // Get the file info
                    FileInfo fileInfo = new(file);

                    // Increment the total size of files
                    TotalFilesSize += (uint)fileInfo.Length;
                    
                    // Increment the size of files left
                    FilesSizeLeft += (uint)fileInfo.Length;
                }

                // Retrieve the size of files in subdirectories
                string[] subdirectories = Directory.GetDirectories(path);
                
                // Recursively call the method for each subdirectory
                foreach (string subdirectory in subdirectories) {
                    
                    // Get the total size of files in the subdirectory
                    GetTotalFilesSize(subdirectory);
                }

            }
            // Catch any exceptions
            catch {

            }
        }

    }
}