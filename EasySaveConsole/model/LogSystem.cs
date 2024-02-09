namespace EasySaveConsole.model {
    
    // Abstract class to manage the logs of the application
    public abstract class LogSystem {

        // Variables for name, source file, and destination file
        private string name;

        // Constructor for LogSystem
        private string sourceFile;

        // Constructor for LogSystem
        private string destinationFile;

        // Property for name
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        // Property for source file
        public string SourceFile {
            get {
                return sourceFile;
            }
            set {
                sourceFile = value;
            }
        }

        // Property for destination file
        public string DestinationFile {
            get {
                return destinationFile;
            }
            set {

                destinationFile = value;
            }
        }

        // Constructor for LogSystem
        public LogSystem(string name, string sourceFile, string destinationFile) {
            Name = name;
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
        }

    }
}