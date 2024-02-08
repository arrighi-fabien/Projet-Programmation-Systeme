namespace EasySaveConsole.model {
    public class JobLog : LogSystem {

        private ulong fileSize;
        private double transferTime;

        public ulong FileSize {
            get {
                return fileSize;
            }
            set {
                fileSize = value;
            }
        }

        public double TransferTime {
            get {
                return transferTime;
            }
            set {
                transferTime = value;
            }
        }

        public JobLog(string name, string sourceFile, string destinationFile, string timestamp, ulong fileSize, double transferTime) : base(name, sourceFile, destinationFile, timestamp) {
            FileSize = fileSize;
            TransferTime = transferTime;
        }

    }
}
