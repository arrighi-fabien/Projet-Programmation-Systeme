using System.Text.Json;
using EasySaveConsole.model;
using EasySaveConsole.view;

namespace EasySaveConsole.controller {
    public class Controller {

        private readonly View view = new();
        private Language language = new();

        public void MainMenu() {

            view.ClearConsole();

            string choice;
            do {
                do {
                    view.DisplayOutput("1. " + language.GetString("menu_language"));
                    view.DisplayOutput("2. " + language.GetString("quit_application"));
                    choice = view.GetInput();
                } while (choice.Length < 0);

                switch (choice) {
                    case "1":
                        ChangeLanguage();
                        break;
                    case "2":
                        break;
                    default:
                        view.DisplayOutput(language.GetString("invalid_choice"));
                        break;
                }
            } while (choice != "2");
        }

        public void ChangeLanguage() {
            string languageCode = "";
            view.DisplayOutput(language.GetString("language_selection"));
            view.DisplayOutput("1. English");
            view.DisplayOutput("2. Français");
            view.DisplayOutput("3. Corsu");

            // Get the user's choice
            string choice = view.GetInput();
            switch (choice) {
                case "1":
                    languageCode = "en";
                    break;
                case "2":
                    languageCode = "fr";
                    break;
                case "3":
                    languageCode = "co";
                    break;
                default:
                    view.DisplayOutput(language.GetString("invalid_choice"));
                    break;
            }
            language.SetLanguage(languageCode);
            view.ClearConsole();
        }

    }
}
