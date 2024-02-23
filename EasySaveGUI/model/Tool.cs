using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace EasySaveGUI.model {

    // Class to manage the tools of the application
    public class Tool {

        // Attributes for the instance of Tool and the serializer options
        private static Tool? instance;
        private readonly JsonSerializerOptions serializerOptions = new() {
            WriteIndented = true
        };

        /// <summary>
        /// Method to get the instance of Tool
        /// </summary>
        /// <returns>The instance of Tool.</returns>
        public static Tool GetInstance() {
            // If instance is null, create a new instance
            instance ??= new Tool();
            return instance;
        }

        /// <summary>
        /// Get the saved save jobs from the savejobs.json config file
        /// </summary>
        /// <returns>The list of saved save jobs.</returns>
        public List<SaveJob> GetSavedSaveJob() {
            try {
                // Get the saved saveJobs from the savejobs.json  config file
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
                return saveJobs;
            }
            catch (Exception) {
                return [];
            }
        }

        /// <summary>
        /// Save the list of savejobs to the savejobs.json config file
        /// </summary>
        /// <param name="saveJobs">The list of savejobs to save.</param>
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
            // Write the list of save jobs to the savejobs.json config file
            string json = JsonSerializer.Serialize(saveJobsJson);
            File.WriteAllText("config/savejobs.json", json);
        }

        /// <summary>
        /// Get the file size of a file
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The file size.</returns>
        public ulong GetFileSize(string path) {
            if (File.Exists(path)) {
                return (ulong)new FileInfo(path).Length;
            }
            return 0;
        }

        /// <summary>
        /// Return if the path is a valid directory
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path is a valid directory, false otherwise.</returns>
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

        /// <summary>
        /// Method to write logs to a JSON file
        /// </summary>
        /// <param name="path">The path of the log file.</param>
        /// <param name="obj">The object to write to the log file.</param>
        public void WriteJobLogJsonFile(string path, JobLog obj) {
            string logContent = "";
            string format = GetConfigValue("logFormat");
            path = "logs/" + path;

            // Check if the log file exists
            List<JobLog> jobLogs;
            try {
                switch (format) {
                    case "json":
                        path += ".json";
                        if (File.Exists(path)) {
                            logContent = File.ReadAllText(path);
                            jobLogs = JsonSerializer.Deserialize<List<JobLog>>(logContent);
                            jobLogs.Add(obj);
                            logContent = JsonSerializer.Serialize(jobLogs, serializerOptions);
                        }
                        else {
                            logContent = JsonSerializer.Serialize(new List<JobLog> { obj }, serializerOptions);
                        }
                        break;
                    case "xml":
                        path += ".xml";
                        XmlSerializer xmlSerializer = new(typeof(List<JobLog>));
                        StringWriter stringWriter = new();
                        if (File.Exists(path)) {
                            logContent = File.ReadAllText(path);
                            jobLogs = (List<JobLog>)xmlSerializer.Deserialize(new StringReader(logContent));
                            jobLogs.Add(obj);
                            xmlSerializer.Serialize(stringWriter, jobLogs);
                            logContent = stringWriter.ToString();
                        }
                        else {
                            xmlSerializer.Serialize(stringWriter, new List<JobLog> { obj });
                            logContent = stringWriter.ToString();
                        }
                        break;
                }
            }
            catch (Exception e) {
                Console.Write(e.ToString());
            }

            File.WriteAllText(path, logContent);
        }

        /// <summary>
        /// Method to write the savejobs states to a file
        /// </summary>
        /// <param name="jobStates">The list of job states to write.</param>
        public void WriteJobStateFile(List<JobState> jobStates) {
            try {
                string logContent = "";
                string path = "logs/state";
                string format = GetConfigValue("logFormat");
                switch (format) {
                    case "json":
                        path += ".json";
                        logContent = JsonSerializer.Serialize(jobStates, serializerOptions);
                        break;
                    case "xml":
                        path += ".xml";
                        XmlSerializer xmlSerializer = new(typeof(List<JobState>));
                        StringWriter stringWriter = new();
                        xmlSerializer.Serialize(stringWriter, jobStates);
                        logContent = stringWriter.ToString();
                        break;
                }
                File.WriteAllText(path, logContent);
            }
            catch (Exception e) {
                Console.Write(e.ToString());
            }
        }

        /// <summary>
        /// Get saved value from the config file
        /// </summary>
        /// <returns>The value from the config file</returns>
        public string GetConfigValue(string key) {
            // Try to read the saved language from the config file
            try {
                string json = File.ReadAllText("config/config.json");
                string language = JsonSerializer.Deserialize<Dictionary<string, string>>(json)[key];
                return language;
            }
            // If the file is missing, return an empty string
            catch {
                return "";
            }
        }

        /// <summary>
        /// Write a value to the config file
        /// </summary>
        /// <param name="key">Key of the value to write</param>
        /// <param name="value">Value to write</param>
        public void WriteConfigValue(string key, string value) {
            // Change the key value in the config file without changing the other values
            try {
                string json = File.ReadAllText("config/config.json");
                Dictionary<string, string> config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                config[key] = value;
                json = JsonSerializer.Serialize(config, serializerOptions);
                File.WriteAllText("config/config.json", json);
            }
            catch {
                // If the file is missing, create a new file with the key value
                Dictionary<string, string> config = new() { { key, value } };
                string json = JsonSerializer.Serialize(config, serializerOptions);
                File.WriteAllText("config/config.json", json);
            }
        }

    }
}