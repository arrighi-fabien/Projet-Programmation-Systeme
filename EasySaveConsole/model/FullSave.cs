namespace EasySaveConsole.model {
    
    // Class representing a full save job, inheriting from SaveJob
    public class FullSave : SaveJob {

        // Constructor for FullSave
        public FullSave(string name, string sourceFolder, string destinationFolder) : base(name, sourceFolder, destinationFolder) {

        }

        // Method to determine if a file should be saved (always returns true for FullSave)
        public override bool IsToSave(string path) {
            return true;
        }

    }
}