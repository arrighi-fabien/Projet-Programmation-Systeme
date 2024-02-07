using EasySaveConsole.model;
using EasySaveConsole.view;

namespace EasySaveConsole.controller {
    public class Controller {

        private readonly View view = new();
        private Language language = new();
        private List<SaveJob> saveJobs = new List<SaveJob>();

        public void MainMenu() {

            view.ClearConsole();

            string choice;
            do {
                do {
                    view.DisplayOutput("1. " + language.GetString("menu_language"));
                    view.DisplayOutput("2. " + language.GetString("menu_execute_save"));
                    view.DisplayOutput("3. " + language.GetString("menu_create_save"));
                    view.DisplayOutput("4. " + language.GetString("menu_update_save"));
                    view.DisplayOutput("5. " + language.GetString("menu_delete_save"));
                    view.DisplayOutput("6. " + language.GetString("quit_application"));
                    choice = view.GetInput();
                } while (choice.Length < 0);

                switch (choice) {
                    case "1":
                        ChangeLanguage();
                        break;
                    case "2":
                        RunSaveJob();
                        break;
                    case "3":
                        CreateSaveJob();
                        break;
                    case "4":
                        UpdateSaveJob();
                        break;
                    case "5":
                        DeleteSaveJob();
                        break;
                    case "6":
                        break;
                    default:
                        view.DisplayOutput(language.GetString("invalid_choice"));
                        break;
                }
            } while (choice != "6");
        }

        private void ChangeLanguage() {
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

        private void RunSaveJob() {
            Console.WriteLine(saveJobs.Count);
            for (int i = 0; i < saveJobs.Count; i++) {
                Console.WriteLine(saveJobs[i].Name);
                Console.WriteLine(saveJobs[i].SourceFolder);
                Console.WriteLine(saveJobs[i].DestinationFolder);
                Console.WriteLine(saveJobs[i].GetType());
            }
        }

        private void CreateSaveJob() {
            view.DisplayOutput(language.GetString("create_savejob"));
            view.DisplayOutput(language.GetString("enter_savejob_name"));
            string saveName = view.GetInput();
            view.DisplayOutput(language.GetString("enter_savejob_source"));
            string saveSource = view.GetInput();
            view.DisplayOutput(language.GetString("enter_savejob_destination"));
            string saveDestination = view.GetInput();
            view.DisplayOutput(language.GetString("enter_savejob_type"));
            string saveType = view.GetInput();
            FullSave savejob = new(saveName, saveSource, saveDestination);
            saveJobs.Add(savejob);
        }

        private void UpdateSaveJob() {

        }

        private void DeleteSaveJob() {

        }

    }
}
