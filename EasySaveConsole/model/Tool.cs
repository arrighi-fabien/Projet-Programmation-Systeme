namespace EasySaveConsole.model {
    public class Tool {

        private static Tool instance;

        public Tool getInstance() {
            if (instance == null) {
                instance = new Tool();
            }
            return instance;
        }

    }
}
