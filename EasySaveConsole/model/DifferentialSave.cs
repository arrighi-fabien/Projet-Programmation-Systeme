namespace EasySaveConsole.model {
    // Class representing a differential save job, inheriting from SaveJob
    public class DifferentialSave : SaveJob {

        // Constructor for DifferentialSave
        public DifferentialSave(string name, string sourceFolder, string destinationFolder) : base(name, sourceFolder, destinationFolder) {

        }

        // Method to determine if a file should be saved (based on differences between source and destination)
        public override bool IsToSave(string path) {
            
            // Get instance of Tool class
            Tool tool = Tool.GetInstance();

            // Construct source file/folder path
            string sourceElementPath = this.SourceFolder + path;


            // Construct destination file/folder path
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