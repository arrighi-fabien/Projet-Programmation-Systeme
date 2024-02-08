using System.Xml.Linq;

namespace EasySaveConsole.model {
    public class JobState : LogSystem {

        private string state;
        private uint totalFiles;
        private ulong totalFilesSize;
        private uint filesLeft;
        private ulong filesSizeLeft;
        private int progression;

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

        public int Progression {
            get {
                return progression;
            }
            set {
                progression = value;
            }
        }

        public JobState(string name, string sourceFile, string destinationFile, string state, uint totalFiles, ulong totalFilesSize, uint filesLeft, ulong filesSizeLeft) : base(name, sourceFile, destinationFile) {
            State = state;
            TotalFiles = totalFiles;
            TotalFilesSize = totalFilesSize;
            FilesLeft = filesLeft;
            FilesSizeLeft = filesSizeLeft;
        }

        public void FinishJobState() {
            State = "END";
            TotalFiles = 0;
            TotalFilesSize = 0;
            FilesLeft = 0;
            FilesSizeLeft = 0;
            Progression = 0;
            SourceFile = "";
            DestinationFile = "";
        }

    }
}
