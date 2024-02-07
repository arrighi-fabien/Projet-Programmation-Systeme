namespace EasySaveConsole.model {
    public class Tool {

        private static Tool instance;

        public static Tool getInstance() {
            if (instance == null) {
                instance = new Tool();
            }
            return instance;
        }

        public uint GetFileSize(string path) {
            return 0;
        }

    }
}
