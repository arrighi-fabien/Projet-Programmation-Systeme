using EasySaveConsole.controller;

class Program {

    static void Main() {

        // Check if the "logs" directory exists, create it if not
        if (!Directory.Exists("logs")) {
            Directory.CreateDirectory("logs");
        }

        // Create an instance of the Controller class and call its MainMenu method
        Controller controller = new();
        controller.MainMenu();
    }
}