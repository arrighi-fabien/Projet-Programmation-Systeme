using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;

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



            try {
                // Check if the destination folder exists, if not create it
                if (!Directory.Exists(DestinationFolder)) {
                    Directory.CreateDirectory(DestinationFolder);
                }

                // Get the list of files to backup
                List<string> files = new List<string>();
                GetFileList(SourceFolder, files);

                // Create a new job state
                JobState jobState = new JobState(Name, "", "", "ACTIVE", (uint)files.Count);
                jobState.GetTotalFilesSize(SourceFolder);
                jobStates.Add(jobState);

                // Get encrypted extensions
                Encrypt encrypt = new Encrypt();

                // Back up each file
                foreach (string file in files) {
                    bool isPaused = false;

                    // Continuous verification loop for business applications
                    while (true) {
                        string savedProfessionalApps = tool.GetConfigValue("professsionalApp");
                        if (!string.IsNullOrEmpty(savedProfessionalApps)) {
                            string[] apps = savedProfessionalApps.Split(';');
                            var runningApps = apps.Where(app => Process.GetProcessesByName(app).Any()).ToList();
                            if (apps.Any(app => Process.GetProcessesByName(app).Length > 0)) {
                                if (!isPaused) {
                                    isPaused = true;
                                    string runningAppNames = string.Join(", ", runningApps);

                                    // Popup to show error
                                    MessageBox.Show($"The following professional application is blocking loading files : {runningAppNames}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    

                                }
                                Thread.Sleep(5000); // Wait 5 seconds before checking again
                                continue;
                            }
                        }

                        if (isPaused) {
                            // Popup to show information
                            MessageBox.Show("The transfer starts again...", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                            isPaused = false;
                        }

                        break; // Exit the loop if no professional application is active
                    }

                    string fileName = file.Substring(SourceFolder.Length + 1);
                    string destinationFile = Path.Combine(DestinationFolder, fileName);

                    if (!IsToSave(fileName)) {
                        continue;
                    }

                    ulong fileSize = 0;
                    if (File.Exists(file)) {
                        double cipherTime = 0;
                        // Get file size
                        fileSize = tool.GetFileSize(file);
                        // Copy the file to the destination folder
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        // Check if the file should be encrypted
                        if (encrypt.IsToEncrypt(file)) {
                            // Try 3 times to encrypt the file
                            for (int i = 0; i < 3; i++) {
                                cipherTime = encrypt.EncryptFile(file, destinationFile);
                                if (cipherTime > -1) {
                                    break;
                                }
                            }
                            if (cipherTime == -1) {
                                File.Copy(file, destinationFile, true);
                            }
                        }
                        else {
                            File.Copy(file, destinationFile, true);
                        }
                        stopwatch.Stop();
                        // Create a new job log
                        JobLog jobLog = new JobLog(Name, file, destinationFile, fileSize, stopwatch.Elapsed.TotalMilliseconds, cipherTime);
                        // Write the job log to a JSON file
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        tool.WriteJobLogJsonFile(date, jobLog);
                    }
                    // If the file is a directory, create the directory in the destination folder
                    else {
                        Directory.CreateDirectory(destinationFile);
                    }
                    // Update job status
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
        /// <param name="path">The path of the directory</param>
        /// <param name="files">The list to which files will be added</param>
        public void GetFileList(string path, List<string> files) {
            string[] fichiers = Directory.GetFiles(path);
            files.AddRange(fichiers);

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders) {
                files.Add(folder);
                GetFileList(folder, files);
            }
        }
    }
}