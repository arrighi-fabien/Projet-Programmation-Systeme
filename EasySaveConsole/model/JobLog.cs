namespace EasySaveConsole.model {
    public class JobLog : LogSystem {

        private ulong fileSize;
        private float transferTime;

        public ulong FileSize {
            get {
                return fileSize;
            }
            set {
                fileSize = value;
            }
        }

        public float TransferTime {
            get {
                return transferTime;
            }
            set {
                transferTime = value;
            }
        }

        public JobLog(string name, string sourceFile, string destinationFile, string timestamp, ulong fileSize, float transferTime) : base(name, sourceFile, destinationFile, timestamp) {
            FileSize = fileSize;
            TransferTime = transferTime;
        }

    }
}
