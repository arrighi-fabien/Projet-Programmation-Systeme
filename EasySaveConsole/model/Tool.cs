namespace EasySaveConsole.model {
    public class Tool {

        private static Tool instance;

        public static Tool getInstance() {
            if (instance == null) {
                instance = new Tool();
            }
            return instance;
        }

        public long GetFileSize(string path) {
            if (File.Exists(path)) {
                return new FileInfo(path).Length;
            }
            return 0;
        }

    }
}
