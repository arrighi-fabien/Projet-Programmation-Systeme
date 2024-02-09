namespace EasySaveConsole.model {
    // Class representing a differential save job, inheriting from SaveJob
    public class DifferentialSave : SaveJob {

        /// <summary>
        /// Constructor for DifferentialSave
        /// </summary>
        /// <param name="name">Name of savejob</param>
        /// <param name="sourceFolder">Source folder of the savejob</param>
        /// <param name="destinationFolder">Destination folder of the savejob</param>
        public DifferentialSave(string name, string sourceFolder, string destinationFolder) : base(name, sourceFolder, destinationFolder) {

        }

        /// <summary>
        /// Return if the file should be saved
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if the file should be saved, false otherwise</returns>
        public override bool IsToSave(string path) {
            // Get instance of Tool class
            Tool tool = Tool.GetInstance();
            // Get source path of the element
            string sourceElementPath = this.SourceFolder + path;
            // Get destination path of the element
            string destinationElementPath = this.DestinationFolder + path;

            // Check if the destination element exists
            if (File.Exists(destinationElementPath)) {
                // If the file sizes match, no need to save
                if (tool.GetFileSize(sourceElementPath) == tool.GetFileSize(destinationElementPath)) {
                    return false;
                }
            }
            // If the destination element is a directory, no need to save
            else if (Directory.Exists(destinationElementPath)) {
                return false;
            }
            // If file doesn't exist in destination or if sizes don't match, save the file
            return true;
        }

    }
}