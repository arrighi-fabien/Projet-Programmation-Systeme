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

        public bool PathDirectoryIsValid(string path) {
            if (string.IsNullOrEmpty(path))
                return false;

            if (!Path.IsPathRooted(path))
                return false;

            char[] invalidChars = Path.GetInvalidPathChars();
            if (path.IndexOfAny(invalidChars) != -1)
                return false;

            if (path.Length >= 260)
                return false;

            if (!Path.IsPathFullyQualified(path))
                return false;

            return true;
        }

    }
}
