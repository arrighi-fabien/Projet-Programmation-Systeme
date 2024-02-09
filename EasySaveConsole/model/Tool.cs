﻿using System.Text.Json;

namespace EasySaveConsole.model {

    // Class to manage the tools of the application
    public class Tool {

        // Instance of Tool
        private static Tool instance;
        
        // Serializer options
        private readonly JsonSerializerOptions serializerOptions = new() {
            WriteIndented = true
        };

        // Constructor for Tool
        public static Tool GetInstance() {
            instance ??= new Tool();
            return instance;
        }

        // Method to get the saved language
        public List<SaveJob> GetSavedSaveJob() {
            try {
                // Read the saved save jobs from the savejobs.json file
                string json = File.ReadAllText("config/savejobs.json");
                List<Dictionary<string, string>> saveJobsJson = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                List<SaveJob> saveJobs = [];

                // Create a new save job for each saved save job    
                foreach (Dictionary<string, string> saveJob in saveJobsJson) {
                    if (saveJob["type"] == "full") {
                        SaveJob savedSaveJob = new FullSave(saveJob["Name"], saveJob["SourceFolder"], saveJob["DestinationFolder"]);
                        saveJobs.Add(savedSaveJob);
                    }
                    else if (saveJob["type"] == "differential") {
                        SaveJob savedSaveJob = new DifferentialSave(saveJob["Name"], saveJob["SourceFolder"], saveJob["DestinationFolder"]);
                        saveJobs.Add(savedSaveJob);
                    }
                }
                // Return the list of save jobs
                return saveJobs;
            }
            catch (Exception) {
                return [];
            }
        }
        
        // Method to write the saved save jobs to the savejobs.json file
        public void WriteSavedSaveJob(List<SaveJob> saveJobs) {
            List<Dictionary<string, string>> saveJobsJson = [];

            // Create a dictionary for each save job and add it to the list
            foreach (SaveJob saveJob in saveJobs) {

                // Create a dictionary for each save job
                Dictionary<string, string> saveJobJson = new() {
                    { "Name", saveJob.Name },
                    { "SourceFolder", saveJob.SourceFolder },
                    { "DestinationFolder", saveJob.DestinationFolder }
                };
                // Add the type of save job to the dictionary
                if (saveJob is FullSave) {
                    saveJobJson.Add("type", "full");
                }
                // Add the dictionary to the list
                else if (saveJob is DifferentialSave) {
                    saveJobJson.Add("type", "differential");
                }
                saveJobsJson.Add(saveJobJson);
            }
            // Write the list of save jobs to the savejobs.json file
            string json = JsonSerializer.Serialize(saveJobsJson);
            File.WriteAllText("config/savejobs.json", json);
        }

        // Method to get the size of a file
        public ulong GetFileSize(string path) {
            if (File.Exists(path)) {
                return (ulong)new FileInfo(path).Length;
            }
            return 0;
        }

        // Method to get the size of a directory
        public bool PathDirectoryIsValid(string path) {
            
            // Check if the path is empty
            if (string.IsNullOrEmpty(path))
                return false;
            
            // Check if the path is valid
            if (!Path.IsPathRooted(path))
                return false;

            // Check if the path contains invalid characters
            char[] invalidChars = Path.GetInvalidPathChars();
            if (path.IndexOfAny(invalidChars) != -1)
                return false;

            // Check if the path is too long
            if (path.Length >= 260)
                return false;

            // Check if the path is fully qualified
            if (!Path.IsPathFullyQualified(path))
                return false;

            return true;
        }

        // Method to get the size of a directory
        public void WriteJobLogJsonFile(string path, JobLog obj) {
            string json = "";

            // Check if the log file exists
            if (File.Exists(path)) {
                json = File.ReadAllText(path);
                List<JobLog> jobLogs = JsonSerializer.Deserialize<List<JobLog>>(json);
                jobLogs.Add(obj);
                json = JsonSerializer.Serialize(jobLogs, serializerOptions);
            }
            
            // If the log file doesn't exist, create a new log file
            else {
                List<JobLog> jobLogs = [obj];
                json = JsonSerializer.Serialize(jobLogs, serializerOptions);
            }
            File.WriteAllText(path, json);
        }

        // Method to get the size of a directory
        public void WriteJobStateJsonFile(List<JobState> jobStates) {
            string path = "logs/state.json";
            string json = JsonSerializer.Serialize(jobStates, serializerOptions);
            File.WriteAllText(path, json);
        }

    }
}