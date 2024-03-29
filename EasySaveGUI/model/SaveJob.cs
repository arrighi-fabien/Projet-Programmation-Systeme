﻿using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Net;
using EasySaveGUI.model;

namespace EasySaveGUI.model {
    // Abstract class to manage the save job
    public abstract class SaveJob {

        private readonly Language language = EasySaveGUI.model.Language.GetInstance();
        private static object lockObject = new();

        // Attributes for the name, sourceFolder and destinationFolder
        private string? name;
        private string? sourceFolder;
        private string? destinationFolder;
        public static CountdownEvent countdownPriorityFile = new(0);
        private static CountdownEvent countdownFileSize = new(0);

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

        // Network load threshold( in %)
        private const double NetworkLoadThreshold = 50.0;

        /// <summary>
        /// Save the data from the source folder to the destination folder
        /// </summary>
        /// <param name="jobStates">List of job states</param>
        public int SaveData(List<JobState> jobStates, ManualResetEvent manualResetEvent, CancellationToken cancellationToken) {
            Tool tool = Tool.GetInstance();

            // Get the network load threshold
            string networkLoadThresholdStr = tool.GetConfigValue("networkLoadThreshold");
            double networkLoadThreshold = !string.IsNullOrEmpty(networkLoadThresholdStr) ? double.Parse(networkLoadThresholdStr) : 50.0;

            // Check if the network load is superior to the threshold
            double networkLoad = NetworkUtils.GetNetworkLoad();
            bool reduceParallelTasks = networkLoad > networkLoadThreshold;

            try {
                // Check if the destination folder exists, if not create it
                if (!Directory.Exists(DestinationFolder)) {
                    Directory.CreateDirectory(DestinationFolder);
                }

                // Get the list of files to backup
                List<string> files = [];
                GetFileList(SourceFolder, files);
                files = OrderByPriority(files, tool.GetConfigValue("priorityExtensions"));

                // Create a new job state
                JobState jobState = new(Name, "", "", "ACTIVE", (uint)files.Count);
                jobState.GetTotalFilesSize(SourceFolder);
                jobStates.Add(jobState);

                // Get encrypted extensions
                Encrypt encrypt = new();

                // Back up each file
                foreach (string file in files) {
                    // Check if the job has been cancelled
                    if (cancellationToken.IsCancellationRequested) {
                        jobState.State = "CANCELLED";
                        // Change the countdown event to 0 to unlock the threads
                        if (countdownFileSize.CurrentCount > 0) {
                            countdownFileSize.Signal();
                        }
                        if (countdownPriorityFile.CurrentCount > 0) {
                            countdownPriorityFile.Signal();
                        }
                        return 0;
                    }

                    bool isPaused = false;

                    // Continuous verification loop for business applications
                    while (true) {
                        string savedProfessionalApps = tool.GetConfigValue("professsionalApp");
                        if (!string.IsNullOrEmpty(savedProfessionalApps)) {
                            string[] apps = savedProfessionalApps.Split(';');
                            bool runningApps = apps.Any(app => Process.GetProcessesByName(app).Any());
                            if (runningApps && !isPaused) {
                                isPaused = true;
                                string runningAppNames = string.Join(", ", runningApps);
                                // Popup to show error
                                MessageBox.Show(language.GetString("following_professional"));
                                // Wait 2 second before checking again
                                Thread.Sleep(2000);
                                continue;
                            }
                        }

                        if (isPaused) {
                            // Popup to show information
                            MessageBox.Show(language.GetString("transfer_starts"));
                            isPaused = false;
                        }
                        // Exit the loop if no professional application is active
                        break;
                    }

                    // Check if the job is paused
                    manualResetEvent.WaitOne();


                    if (reduceParallelTasks) {
                        Thread.Sleep(100);
                    }
                    // Update and check network load regularly
                    networkLoad = NetworkUtils.GetNetworkLoad();
                    reduceParallelTasks = networkLoad > networkLoadThreshold;

                    string fileName = file.Substring(SourceFolder.Length + 1);
                    string destinationFile = Path.Combine(DestinationFolder, fileName);

                    // If the files isn't a priority file wait for the other threads to finish their priority files
                    if (!IsPrioritary(file, tool.GetConfigValue("priorityExtensions")) && countdownPriorityFile.CurrentCount > 0) {
                        countdownPriorityFile.Signal();
                        jobState.State = "WAITING";
                        countdownPriorityFile.Wait();
                        jobState.State = "ACTIVE";
                    }

                    // Check if the file should be saved
                    if (!IsToSave(fileName)) {
                        continue;
                    }

                    ulong fileSize = 0;
                    if (File.Exists(file)) {
                        double cipherTime = 0;
                        // Get file size
                        fileSize = tool.GetFileSize(file);

                        // If fileSize is superior to the value in the config file, and lock the next thread until the transfer is done
                        if (fileSize >= ulong.Parse(tool.GetConfigValue("fileSize")) * 1024) {
                            lock (lockObject) {
                                if (countdownFileSize.CurrentCount > 0) {
                                    countdownFileSize.Wait();
                                    countdownFileSize = new(1);
                                }
                                else {
                                    countdownFileSize = new(1);
                                }
                            }
                        }

                        // Copy the file to the destination folder
                        Stopwatch stopwatch = new();
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

                        // Tell the next thread that the transfer of big file is done
                        if (countdownFileSize.CurrentCount > 0) {
                            countdownFileSize.Signal();
                        }

                        // Create a new job log
                        JobLog jobLog = new(Name, file, destinationFile, fileSize, stopwatch.Elapsed.TotalNanoseconds / 1_000_000, cipherTime);
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

        /// <summary>
        /// Order the files by priority
        /// </summary>
        /// <param name="files">The list of files</param>
        /// <param name="priorityExtension">The priority extensions</param>
        /// <returns>The list of files ordered by priority</returns>
        private List<string> OrderByPriority(List<string> files, string priorityExtension) {
            // Put priorityExtension in a list
            List<string> priorityExtensionList = priorityExtension.Split(";").Select(ext => "." + ext).ToList();
            // Sorted folders first and then priorityExtension files
            var sortedFiles = files.OrderBy(path => {
                // Folders are always first
                if (Directory.Exists(path))
                    return 0;
                else {
                    string extension = Path.GetExtension(path).ToLower();
                    int index = priorityExtensionList.IndexOf(extension);
                    return index != -1 ? index + 1 : priorityExtensionList.Count + 1;
                }
            });
            return sortedFiles.ToList();
        }

        /// <summary>
        /// Check if the file is a priority file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="priorityExtension">The priority extensions</param>
        /// <returns>True if the file is a priority file, false otherwise</returns>
        private bool IsPrioritary(string fileName, string priorityExtension) {
            List<string> priorityExtensionList = priorityExtension.Split(";").Select(ext => "." + ext).ToList();
            string extension = Path.GetExtension(fileName).ToLower();
            // Check if the file is a directory or if the extension is in the priorityExtension list
            return priorityExtensionList.Contains(extension) || Directory.Exists(fileName);
        }
    }
}