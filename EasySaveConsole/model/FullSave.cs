namespace EasySaveConsole.model {
    public class FullSave : SaveJob {

        public FullSave(string name, string sourceFolder, string destinationFolder) : base(name, sourceFolder, destinationFolder) {

        }

        public override bool IsToSave(string path) {
            return true;
        }

    }
}
