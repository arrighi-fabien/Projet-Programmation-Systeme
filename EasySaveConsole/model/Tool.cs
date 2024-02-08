using System.Text.Json;

namespace EasySaveConsole.model {
    public class Tool {

        private static Tool instance;

        public static Tool GetInstance() {
            if (instance == null) {
                instance = new Tool();
            }
            return instance;
        }

        public List<SaveJob> GetSavedSaveJob() {
            if (File.Exists("config/config.json")) {
                try {
                    string json = File.ReadAllText("config/config.json");
                    List<Dictionary<string, string>> saveJobsJson = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                    List<SaveJob> saveJobs = new List<SaveJob>();
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
            return [];
        }

        public void WriteSavedSaveJob(List<SaveJob> saveJobs) {
            List<Dictionary<string, string>> saveJobsJson = new List<Dictionary<string, string>>();
            foreach (SaveJob saveJob in saveJobs) {
                Dictionary<string, string> saveJobJson = new Dictionary<string, string> {
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
            File.WriteAllText("config/config.json", json);
        }

        public long GetFileSize(string path) {
            if (File.Exists(path)) {
                return new FileInfo(path).Length;
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

    }
}
