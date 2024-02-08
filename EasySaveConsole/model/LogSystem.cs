namespace EasySaveConsole.model {
    public abstract class LogSystem {

        private string name;
        private string sourceFile;
        private string destinationFile;

        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        public string SourceFile {
            get {
                return sourceFile;
            }
            set {
                sourceFile = value;
            }
        }

        public string DestinationFile {
            get {
                return destinationFile;
            }
            set {

                destinationFile = value;
            }
        }

        public LogSystem(string name, string sourceFile, string destinationFile) {
            Name = name;
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
        }

    }
}
