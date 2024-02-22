using System.Diagnostics;
using System.IO;

namespace EasySaveGUI.model {
    // Abstract class to manage the save job
    public abstract class SaveJob {

        // Attributes for the name, sourceFolder and destinationFolder
        private string? name;
        private string? sourceFolder;
        private string? destinationFolder;

        // Properties for the name, sourceFolder and destinationFolder
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        public string SourceFolder {
            get {
                return sourceFolder;
            }
            set {
                sourceFolder = value;
            }
        }

        public string DestinationFolder {
            get {
                return destinationFolder;
            }
            set {
                destinationFolder = value;
            }
        }

        public string Type {
            get {
                return GetType().Name;
            }
        }

        /// <summary>
        /// Constructor for SaveJob
        /// </summary>
        /// <param name="name">Name of savejob</param>
        /// <param name="sourceFolder">Source folder of the savejob</param>
        /// <param name="destinationFolder">Destination folder of the savejob</param>
        public SaveJob(string name, string sourceFolder, string destinationFolder) {
            Name = name;
            SourceFolder = sourceFolder;
            DestinationFolder = destinationFolder;
        }

        /// <summary>
        /// Return if the file should be saved
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>True if the file should be saved, false otherwise</returns>
        public abstract bool IsToSave(string path);

        /// <summary>
        /// Save the data from the source folder to the destination folder
        /// </summary>
        /// <param name="jobStates">List of job states</param>
        public int SaveData(List<JobState> jobStates) {
            Tool tool = Tool.GetInstance();

            // Check if professionnal apps are running
            string savedProfessionalApps = tool.GetConfigValue("professsionalApp");
            string[] apps = savedProfessionalApps.Split(";");
            foreach (string app in apps) {
                if (Process.GetProcessesByName(app).Length > 0) {
                    return 1;
                }
            }
            try {
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

                // Get encrypted extensions
                Encrypt encrypt = new();

                // Save each file
                foreach (string file in files) {
                    string fileName = file.Substring(SourceFolder.Length + 1);
                    string destinationFile = Path.Combine([DestinationFolder, fileName]);

                    if (!IsToSave(fileName)) {
                        continue;
                    }

                    ulong fileSize = 0;
                    if (File.Exists(file)) {
                        // Get the file size
                        fileSize = tool.GetFileSize(file);
                        // Copy the file to the destination folder
                        DateTime startTime = DateTime.Now;

                        // Check if the file should be encrypted
                        if (encrypt.IsToEncrypt(file)) {
                            // Encrypt the file
                            string key = "";
                            encrypt.EncryptFile(file, destinationFile, key);
                        }
                        else {
                            File.Copy(file, destinationFile, true);
                        }
                        // Log the job
                        DateTime endTime = DateTime.Now;
                        // Calculate the duration of the transfer
                        double durationInSeconds = (endTime - startTime).TotalSeconds;
                        // Create a new job log
                        JobLog jobLog = new(Name, file, destinationFile, fileSize, durationInSeconds);
                        // Write the job log to a JSON file
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        // Check if the log file exists
                        tool.WriteJobLogJsonFile(date, jobLog);
                    }
                    // If the file is a directory, create the directory in the destination folder
                    else {
                        Directory.CreateDirectory(destinationFile);
                    }
                    // Update the job state
                    jobState.SourceFile = file;
                    jobState.DestinationFile = destinationFile;
                    jobState.FilesLeft--;
                    jobState.FilesSizeLeft -= fileSize;
                    jobState.Progression = (int)((files.Count - jobState.FilesLeft) * 100 / files.Count);
                    tool.WriteJobStateFile(jobStates);
                }
                // Finish the job
                jobState.FinishJobState();
                tool.WriteJobStateFile(jobStates);
                return 0;
            }
            catch (Exception) {
                return 2;
            }
        }

        /// <summary>
        /// Get the list of files and folders in the specified directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="files"></param>
        public void GetFileList(string path, List<string> files) {
            // Get the list of files
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