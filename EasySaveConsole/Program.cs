using EasySaveConsole.controller;

class Program {

    static void Main() {

        if (!Directory.Exists("logs")) {
            Directory.CreateDirectory("logs");
        }

        Controller controller = new();
        controller.MainMenu();
    }
}
