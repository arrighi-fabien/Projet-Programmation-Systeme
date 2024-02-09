namespace EasySaveConsole.model {
    
    // Class representing a log entry for a job, inheriting from LogSystem
    public class JobLog : LogSystem {

        // Variables for file size, transfer time, and timestamp
        private ulong fileSize;
        private double transferTime;
        private string timestamp;

        // FileSize property
        public ulong FileSize {
            get {
                return fileSize;
            }
            set {
                fileSize = value;
            }
        }

        // TransferTime property
        public double TransferTime {
            get {
                return transferTime;
            }
            set {
                transferTime = value;
            }
        }

        // Property to get or set the timestamp when the log was created.  
        public string Timestamp {
            get {
                return timestamp;
            }
            set {
                timestamp = value;
            }
        }

        // Constructor for JobLog
        public JobLog(string name, string sourceFile, string destinationFile, ulong fileSize, double transferTime) : base(name, sourceFile, destinationFile) {
            
            // Set file size
            FileSize = fileSize;

            // Set transfer time
            TransferTime = transferTime;

            // Set the current timestamp
            Timestamp = DateTime.Now.ToString();
        }

    }
}