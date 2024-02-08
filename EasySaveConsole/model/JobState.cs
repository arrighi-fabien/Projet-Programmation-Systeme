namespace EasySaveConsole.model {
    public class JobState : LogSystem {

        private string state;
        private uint totalFiles;
        private ulong totalFilesSize;
        private uint filesLeft;
        private ulong filesSizeLeft;

        public string State {
            get {
                return state;
            }
            set {
                state = value;
            }
        }

        public uint TotalFiles {
            get {
                return totalFiles;
            }
            set {
                totalFiles = value;
            }
        }

        public ulong TotalFilesSize {
            get {
                return totalFilesSize;
            }
            set {
                totalFilesSize = value;
            }
        }

        public uint FilesLeft {
            get {
                return filesLeft;
            }
            set {
                filesLeft = value;
            }
        }

        public ulong FilesSizeLeft {
            get {
                return filesSizeLeft;
            }
            set {
                filesSizeLeft = value;
            }
        }

        public JobState(string name, string sourceFile, string destinationFile, string timestamp, string state, uint totalFiles, ulong totalFilesSize, uint filesLeft, ulong filesSizeLeft) : base(name, sourceFile, destinationFile, timestamp) {
            State = state;
            TotalFiles = totalFiles;
            TotalFilesSize = totalFilesSize;
            FilesLeft = filesLeft;
            FilesSizeLeft = filesSizeLeft;
        }

    }
}
