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
            Tool tool = Tool.GetInstance();
            if (!Directory.Exists(DestinationFolder)) {
                Directory.CreateDirectory(DestinationFolder);
            }

            List<string> files = [];
            GetFileList(SourceFolder, files);

            JobState jobState = new(Name, "", "", "ACTIVE", (uint)files.Count);
            jobState.GetTotalFilesSize(SourceFolder);
            jobStates.Add(jobState);

            foreach (string file in files) {
                string fileName = file.Substring(SourceFolder.Length + 1);
                if (!IsToSave(fileName)) {
                    continue;
                }

                ulong fileSize = 0;
                if (File.Exists(file)) {
                    fileSize = tool.GetFileSize(file);
                    DateTime startTime = DateTime.Now;
                    File.Copy(file, Path.Combine([DestinationFolder, fileName]), true);
                    DateTime endTime = DateTime.Now;
                    double durationInSeconds = (endTime - startTime).TotalSeconds;
                    JobLog jobLog = new(Name, file, Path.Combine([DestinationFolder, fileName]), fileSize, durationInSeconds);
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    tool.WriteJobLogJsonFile($"logs/{date}.json", jobLog);
                }
                else {
                    Directory.CreateDirectory(Path.Combine([DestinationFolder, fileName]));
                }
                jobState.SourceFile = file;
                jobState.DestinationFile = Path.Combine([DestinationFolder, fileName]);
                jobState.FilesLeft--;
                jobState.FilesSizeLeft -= fileSize;
                tool.WriteJobStateJsonFile(jobStates);
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
