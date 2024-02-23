namespace EasySaveGUI.model {

    // Class representing a log entry for a job, inheriting from LogSystem
    public class JobLog : LogSystem {

        // Attributes for fileSize, transferTime, and timestamp
        private ulong fileSize;
        private double transferTime;
        private string timestamp;
        private double cipherTime;

        // Properties for fileSize, transferTime, and timestamp
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

        public double CipherTime {
            get {
                return cipherTime;
            }
            set {
                cipherTime = value;
            }
        }


        /// <summary>
        /// Constructor for JobLog
        /// </summary>
        public JobLog() : base() {

        }

        /// <summary>
        /// Constructor for JobLog
        /// </summary>
        /// <param name="name">Name of savejob</param>
        /// <param name="sourceFile">Source files of the savejob</param>
        /// <param name="destinationFile">Destination files of the savejob</param>
        /// <param name="fileSize">Size of the file</param>
        /// <param name="transferTime">Time taken to transfer the file</param>
        /// <param name="cypherTime">Time taken to cypher the file</param>
        public JobLog(string name, string sourceFile, string destinationFile, ulong fileSize, double transferTime, double cypherTime) : base(name, sourceFile, destinationFile) {
            FileSize = fileSize;
            TransferTime = transferTime;
            CipherTime = cypherTime;
            Timestamp = DateTime.Now.ToString();
        }

    }
}