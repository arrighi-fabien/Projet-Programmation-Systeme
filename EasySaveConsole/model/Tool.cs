using System.Text.Json;

namespace EasySaveConsole.model {
    public class Tool {

        private static Tool instance;
        private readonly JsonSerializerOptions serializerOptions = new() {
            WriteIndented = true
        };

        public static Tool GetInstance() {
            instance ??= new Tool();
            return instance;
        }

        public List<SaveJob> GetSavedSaveJob() {
            try {
                string json = File.ReadAllText("config/savejobs.json");
                List<Dictionary<string, string>> saveJobsJson = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                List<SaveJob> saveJobs = [];
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

        public void WriteSavedSaveJob(List<SaveJob> saveJobs) {
            List<Dictionary<string, string>> saveJobsJson = [];
            foreach (SaveJob saveJob in saveJobs) {
                Dictionary<string, string> saveJobJson = new() {
                    { "Name", saveJob.Name },
                    { "SourceFolder", saveJob.SourceFolder },
                    { "DestinationFolder", saveJob.DestinationFolder }
                };
                if (saveJob is FullSave) {
                    saveJobJson.Add("type", "full");
                }
                else if (saveJob is DifferentialSave) {
                    saveJobJson.Add("type", "differential");
                }
                saveJobsJson.Add(saveJobJson);
            }
            string json = JsonSerializer.Serialize(saveJobsJson);
            File.WriteAllText("config/savejobs.json", json);
        }

        public ulong GetFileSize(string path) {
            if (File.Exists(path)) {
                return (ulong)new FileInfo(path).Length;
            }
            return 0;
        }

        public bool PathDirectoryIsValid(string path) {
            if (string.IsNullOrEmpty(path))
                return false;

            if (!Path.IsPathRooted(path))
                return false;

            char[] invalidChars = Path.GetInvalidPathChars();
            if (path.IndexOfAny(invalidChars) != -1)
                return false;

            if (path.Length >= 260)
                return false;

            if (!Path.IsPathFullyQualified(path))
                return false;

            return true;
        }

        public void WriteJobLogJsonFile(string path, JobLog obj) {
            string json = "";
            if (File.Exists(path)) {
                json = File.ReadAllText(path);
                List<JobLog> jobLogs = JsonSerializer.Deserialize<List<JobLog>>(json);
                jobLogs.Add(obj);
                json = JsonSerializer.Serialize(jobLogs, serializerOptions);
            }
            else {
                List<JobLog> jobLogs = [obj];
                json = JsonSerializer.Serialize(jobLogs, serializerOptions);
            }
            File.WriteAllText(path, json);
        }

        public void WriteJobStateJsonFile(List<JobState> jobStates) {
            string path = "logs/state.json";
            string json = JsonSerializer.Serialize(jobStates, serializerOptions);
            File.WriteAllText(path, json);
        }

    }
}
