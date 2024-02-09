namespace EasySaveConsole.model {
    // Abstract class to manage the save job
    public abstract class SaveJob {

        // Variables for name, source folder, and destination folder
        private string name;

        // Property for name
        private string sourceFolder;

        // Property for source folder
        private string destinationFolder;

        // Property for destination folder
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        // Property for source folder
        public string SourceFolder {
            get {
                return sourceFolder;
            }
            set {
                sourceFolder = value;
            }
        }

        // Property for destination folder
        public string DestinationFolder {
            get {
                return destinationFolder;
            }
            set {
                destinationFolder = value;
            }
        }

        // Constructor for SaveJob
        public SaveJob(string name, string sourceFolder, string destinationFolder) {
            Name = name;
            SourceFolder = sourceFolder;
            DestinationFolder = destinationFolder;
        }

        // Method to determine if a file should be saved
        public abstract bool IsToSave(string path);

        // Method to save data
        public void SaveData(List<JobState> jobStates) {
            Tool tool = Tool.GetInstance();

            // Check if the destination folder exists, if not, create it
            if (!Directory.Exists(DestinationFolder)) {
                Directory.CreateDirectory(DestinationFolder);
            }

            // Get the list of files to save
            List<string> files = [];
            GetFileList(SourceFolder, files);

            // Create a new job state
            JobState jobState = new(Name, "", "", "ACTIVE", (uint)files.Count);
            jobState.GetTotalFilesSize(SourceFolder);
            jobStates.Add(jobState);

            // Save each file
            foreach (string file in files) {
                string fileName = file.Substring(SourceFolder.Length + 1);
                if (!IsToSave(fileName)) {
                    continue;
                }

                // Get the file size
                ulong fileSize = 0;
                if (File.Exists(file)) {

                    // Get the file size
                    fileSize = tool.GetFileSize(file);
                    // Copy the file to the destination folder
                    DateTime startTime = DateTime.Now;
                    File.Copy(file, Path.Combine([DestinationFolder, fileName]), true);
                    // Log the job
                    DateTime endTime = DateTime.Now;
                    // Calculate the duration of the transfer
                    double durationInSeconds = (endTime - startTime).TotalSeconds;
                    // Create a new job log
                    JobLog jobLog = new(Name, file, Path.Combine([DestinationFolder, fileName]), fileSize, durationInSeconds);
                    // Write the job log to a JSON file
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    // Check if the log file exists
                    tool.WriteJobLogJsonFile($"logs/{date}.json", jobLog);
                }
                // If the file is a directory, create the directory in the destination folder
                else {
                    Directory.CreateDirectory(Path.Combine([DestinationFolder, fileName]));
                }
                // Update the job state
                jobState.SourceFile = file;
                jobState.DestinationFile = Path.Combine([DestinationFolder, fileName]);
                jobState.FilesLeft--;
                jobState.FilesSizeLeft -= fileSize;
                tool.WriteJobStateJsonFile(jobStates);
            }
            // Finish the job
            jobState.FinishJobState();
            tool.WriteJobStateJsonFile(jobStates);
        }

        // Method to get the list of files in a folder
        public void GetFileList(string path, List<string> files) {
            string[] fichiers = Directory.GetFiles(path);
            files.AddRange(fichiers);

            // Get the list of subdirectories
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders) {
                files.Add(folder);
                GetFileList(folder, files);
            }
        }

    }
}