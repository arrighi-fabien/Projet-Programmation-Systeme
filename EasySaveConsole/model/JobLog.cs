namespace EasySaveConsole.model {
    public class JobLog : LogSystem {

        private ulong fileSize;
        private double transferTime;
        private string timestamp;

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

        public string Timestamp {
            get {
                return timestamp;
            }
            set {
                timestamp = value;
            }
        }

        public JobLog(string name, string sourceFile, string destinationFile, ulong fileSize, double transferTime) : base(name, sourceFile, destinationFile) {
            FileSize = fileSize;
            TransferTime = transferTime;
            Timestamp = DateTime.Now.ToString();
        }

    }
}
