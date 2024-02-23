namespace EasySaveGUI.model {
    
    // Class representing a full save job, inheriting from SaveJob
    public class FullSave : SaveJob {

        /// <summary>
        /// Constructor for FullSave
        /// </summary>
        /// <param name="name">Name of savejob</param>
        /// <param name="sourceFolder">Source folder of the savejob</param>
        /// <param name="destinationFolder">Destination folder of the savejob</param>
        public FullSave(string name, string sourceFolder, string destinationFolder) : base(name, sourceFolder, destinationFolder) {

        }

        /// <summary>
        /// Return if the file should be saved
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>True</returns>
        public override bool IsToSave(string path) {
            return true;
        }

    }
}