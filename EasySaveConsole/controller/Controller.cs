using EasySaveConsole.model;
using EasySaveConsole.view;

namespace EasySaveConsole.controller {
    public class Controller {

        private readonly View view = new();
        private readonly Language language = Language.GetInstance();
        private Tool tool = Tool.GetInstance();
        private List<SaveJob> saveJobs;
        private List<JobState> jobStates = [];

        public void MainMenu() {

            saveJobs = tool.GetSavedSaveJob();

            view.ClearConsole();
            string choice;

            do {
                view.DisplayMainMenu();
                choice = view.GetInput();
                view.ClearConsole();

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
                        view.DisplayError(language.GetString("invalid_input"));
                        break;
                }
            } while (choice != "6");
        }

        private void ChangeLanguage() {
            view.DisplayOutput(language.GetString("select_language") + " :");
            view.DisplayOutput("1. English");
            view.DisplayOutput("2. Français");

            // Get the user's choice
            string choice = view.GetInput();
            view.ClearConsole();
            switch (choice) {
                case "1":
                    language.SetLanguage("en");
                    break;
                case "2":
                    language.SetLanguage("fr");
                    break;
                default:
                    view.DisplayError(language.GetString("invalid_input"));
                    break;
            }
        }

        private void RunSaveJob() {
            if (saveJobs.Count <= 0) {
                view.DisplayError(language.GetString("no_savejob"));
                return;
            }
            view.DisplayOutput(language.GetString("select_savejob") + " :");
            view.DisplaySaveJobList(saveJobs);
            // Possibility to enter one number or multiple numbers separated by a comma for particular save jobs or a - for a range of save jobs
            string choice = view.GetInput();
            try {
                if (choice.Contains("-")) {
                    string[] range = choice.Split("-");
                    int start = int.Parse(range[0]);
                    int end = int.Parse(range[1]);
                    for (int i = start; i <= end; i++) {
                        saveJobs[i - 1].SaveData(jobStates);
                    }
                }
                else if (choice.Contains(";")) {
                    string[] choices = choice.Split(";");
                    foreach (string c in choices) {
                        saveJobs[int.Parse(c) - 1].SaveData(jobStates);
                    }
                }
                else {
                    saveJobs[int.Parse(choice) - 1].SaveData(jobStates);
                }
            }
            catch (Exception) {
                view.DisplayError(language.GetString("invalid_input"));
                return;
            }
            view.ClearConsole();
        }

        private void CreateSaveJob() {
            view.DisplayOutput(language.GetString("create_savejob"));
            view.DisplayOutput(language.GetString("enter_savejob_name"));
            string saveName = view.GetInput();
            if (saveName.Length < 1) {
                view.DisplayError(language.GetString("invalid_input"));
                return;
            }
            view.DisplayOutput(language.GetString("enter_savejob_source"));
            string saveSource = view.GetInput();
            if (!Directory.Exists(saveSource)) {
                view.DisplayError(language.GetString("source_folder_not_found"));
                return;
            }
            view.DisplayOutput(language.GetString("enter_savejob_destination"));
            string saveDestination = view.GetInput();
            if (!tool.PathDirectoryIsValid(saveDestination)) {
                view.DisplayError(language.GetString("destination_folder_not_found"));
                return;
            }
            view.DisplayOutput(language.GetString("enter_savejob_type"));
            view.DisplayOutput($"1. {language.GetString("select_savejob_full")}");
            view.DisplayOutput($"2. {language.GetString("select_savejob_differential")}");
            string saveType = view.GetInput();

            switch (saveType) {
                case "1":
                    saveJobs.Add(new FullSave(saveName, saveSource, saveDestination));
                    tool.WriteSavedSaveJob(saveJobs);
                    view.ClearConsole();
                    break;
                case "2":
                    saveJobs.Add(new DifferentialSave(saveName, saveSource, saveDestination));
                    tool.WriteSavedSaveJob(saveJobs);
                    view.ClearConsole();
                    break;
                default:
                    view.DisplayError(language.GetString("invalid_input"));
                    break;
            }
        }

        private void UpdateSaveJob() {
            if (saveJobs.Count <= 0) {
                view.DisplayError(language.GetString("no_savejob"));
                return;
            }
            view.DisplayOutput(language.GetString("select_savejob_update") + " :");
            view.DisplaySaveJobList(saveJobs);
            try {
                int choice = int.Parse(view.GetInput());
                view.DisplayOutput(language.GetString("enter_savejob_name"));
                saveJobs[choice - 1].Name = view.GetInput();
                view.DisplayOutput(language.GetString("enter_savejob_source"));
                saveJobs[choice - 1].SourceFolder = view.GetInput();
                view.DisplayOutput(language.GetString("enter_savejob_destination"));
                saveJobs[choice - 1].DestinationFolder = view.GetInput();
                view.DisplayOutput(language.GetString("enter_savejob_type"));
                view.DisplayOutput($"1. {language.GetString("select_savejob_full")}");
                view.DisplayOutput($"2. {language.GetString("select_savejob_differential")}");
                string saveType = view.GetInput();
                switch (saveType) {
                    case "1":
                        saveJobs[choice - 1] = new FullSave(saveJobs[choice - 1].Name, saveJobs[choice - 1].SourceFolder, saveJobs[choice - 1].DestinationFolder);
                        tool.WriteSavedSaveJob(saveJobs);
                        break;
                    case "2":
                        saveJobs[choice - 1] = new DifferentialSave(saveJobs[choice - 1].Name, saveJobs[choice - 1].SourceFolder, saveJobs[choice - 1].DestinationFolder);
                        tool.WriteSavedSaveJob(saveJobs);
                        break;
                    default:
                        view.DisplayError(language.GetString("no_savejob"));
                        break;
                }
                view.ClearConsole();
            }
            catch (Exception) {
                view.DisplayError(language.GetString("no_savejob"));
            }
        }

        private void DeleteSaveJob() {
            if (saveJobs.Count <= 0) {
                view.DisplayError(language.GetString("no_savejob"));
                return;
            }
            view.DisplayOutput(language.GetString("select_savejob_delete") + " :");
            view.DisplaySaveJobList(saveJobs);
            try {
                int choice = int.Parse(view.GetInput());
                saveJobs.RemoveAt(choice - 1);
                tool.WriteSavedSaveJob(saveJobs);
                view.ClearConsole();
            }
            catch (Exception) {
                view.DisplayError(language.GetString("invalid_input"));
            }
        }

    }
}
