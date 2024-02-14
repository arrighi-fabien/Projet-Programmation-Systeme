namespace EasySaveConsole.model {

    // Abstract class to manage the logs of the application
    public abstract class LogSystem {

        // Attributes for name, sourceFile, and destinationFile
        private string? name;
        private string? sourceFile;
        private string? destinationFile;

        // Properties for name, sourceFile, and destinationFile
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

        /// <summary>
        /// Constructor for LogSystem
        /// </summary>
        public LogSystem() {

        }

        /// <summary>
        /// Constructor for LogSystem
        /// </summary>
        /// <param name="name">Name of savejob</param>
        /// <param name="sourceFile">Source files of the savejob</param>
        /// <param name="destinationFile">Destination files of the savejob</param>
        public LogSystem(string name, string sourceFile, string destinationFile) {
            Name = name;
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
        }

    }
}