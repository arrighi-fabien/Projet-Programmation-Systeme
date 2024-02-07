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

        public void SaveData() {
            if (!Directory.Exists(DestinationFolder)) {
                Directory.CreateDirectory(DestinationFolder);
            }
            List<string> files = new List<string>();
            GetFileList(SourceFolder, files);
            foreach (string file in files) {
                string fileName = file.Substring(SourceFolder.Length + 1);
                if (IsToSave(fileName)) {
                    if (File.Exists(file)) {
                        File.Copy(file, Path.Combine([DestinationFolder, fileName]), true);
                    }
                    else {
                        Directory.CreateDirectory(Path.Combine([DestinationFolder, fileName]));
                    }
                }
            }
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
