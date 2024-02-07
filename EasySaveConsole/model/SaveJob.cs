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
            this.name = name;
            this.sourceFolder = sourceFolder;
            this.destinationFolder = destinationFolder;
        }

        public abstract bool IsToSave(string path);

    }
}
