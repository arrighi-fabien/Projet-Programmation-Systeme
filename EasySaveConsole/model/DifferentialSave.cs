namespace EasySaveConsole.model {
    public class DifferentialSave : SaveJob {

        public DifferentialSave(string name, string sourceFolder, string destinationFolder) : base(name, sourceFolder, destinationFolder) {
            this.Name = name;
            this.SourceFolder = sourceFolder;
            this.DestinationFolder = destinationFolder;
        }

        public override bool IsToSave(string path) {
            Tool tool = Tool.getInstance();
            string sourceElementPath = this.SourceFolder + path;
            string destinationElementPath = this.DestinationFolder + path;
            if (File.Exists(destinationElementPath)) {
                if (tool.GetFileSize(sourceElementPath) == tool.GetFileSize(destinationElementPath)) {
                    return false;
                }
            }
            else if (Directory.Exists(destinationElementPath)) {
                return false;
            }
            return true;
        }

    }
}
