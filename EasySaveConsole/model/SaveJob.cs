namespace EasySaveConsole.model {
    public abstract class SaveJob {

        private string name;
        private string sourceFolder;
        private string destinationFolder;

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

        public SaveJob(string name, string sourceFolder, string destinationFolder) {
            Name = name;
            SourceFolder = sourceFolder;
            DestinationFolder = destinationFolder;
        }

        public abstract bool IsToSave(string path);

        public void SaveData(List<JobState> jobStates) {
            if (!Directory.Exists(DestinationFolder)) {
                Directory.CreateDirectory(DestinationFolder);
            }
            List<string> files = [];
            GetFileList(SourceFolder, files);
            JobState jobState = new (Name, "", "", "ACTIVE", (uint)files.Count, 5465, 45646, 564);
            jobStates.Add(jobState);
            Tool tool = Tool.GetInstance();
            foreach (string file in files) {
                string fileName = file.Substring(SourceFolder.Length + 1);
                if (IsToSave(fileName)) {
                    if (File.Exists(file)) {
                        DateTime startTime = DateTime.Now;
                        File.Copy(file, Path.Combine([DestinationFolder, fileName]), true);
                        DateTime endTime = DateTime.Now;
                        TimeSpan duration = endTime - startTime;
                        double durationInSeconds = duration.TotalSeconds;
                        jobState.SourceFile = file;
                        jobState.DestinationFile = Path.Combine([DestinationFolder, fileName]);
                        tool.WriteJobStateJsonFile(jobStates);
                        JobLog jobLog = new(Name, file, Path.Combine([DestinationFolder, fileName]), DateTime.Now.ToString(), tool.GetFileSize(file), durationInSeconds);
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        tool.WriteJobLogJsonFile($"logs/{date}.json", jobLog);
                    }
                    else {
                        Directory.CreateDirectory(Path.Combine([DestinationFolder, fileName]));
                    }
                }
            }
            jobState.FinishJobState();
            tool.WriteJobStateJsonFile(jobStates);
        }

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
