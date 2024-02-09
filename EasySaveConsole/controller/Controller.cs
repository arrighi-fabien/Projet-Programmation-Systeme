using EasySaveConsole.model;
using EasySaveConsole.view;

namespace EasySaveConsole.controller {
    
    // Class responsible
    public class Controller {

        // Variables for view, language, and tool
        private readonly View view = new();

        // Language instance
        private readonly Language language = Language.GetInstance();
        
        // Tool instance
        private Tool tool = Tool.GetInstance();
        
        // List of save jobs
        private List<SaveJob> saveJobs;
        
        // List of job states
        private List<JobState> jobStates = [];

        // Method to display the main menu
        public void MainMenu() {

            // Get the saved save jobs
            saveJobs = tool.GetSavedSaveJob();

            // Clear the console
            view.ClearConsole();
            string choice;

            // Display the main menu
            do {
                view.DisplayMainMenu();
                choice = view.GetInput();
                view.ClearConsole();

                // Switch case for the main menu
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
            // Loop until the user chooses to quit
            } while (choice != "6");
        }

        // Method to change the language
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

        // Method to run a save job
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
                // Save the data for the selected save jobs
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

        // Method to create a save job
        private void CreateSaveJob() {
            view.DisplayOutput(language.GetString("create_savejob"));
            view.DisplayOutput(language.GetString("enter_savejob_name"));
            string saveName = view.GetInput();

            // Check if the name is valid
            if (saveName.Length < 1) {
                view.DisplayError(language.GetString("invalid_input"));
                return;
            }
            view.DisplayOutput(language.GetString("enter_savejob_source"));
            string saveSource = view.GetInput();
            
            // Check if the source folder exists
            if (!Directory.Exists(saveSource)) {
                view.DisplayError(language.GetString("source_folder_not_found"));
                return;
            }
            view.DisplayOutput(language.GetString("enter_savejob_destination"));
            string saveDestination = view.GetInput();
            
            // Check if the destination folder exists
            if (!tool.PathDirectoryIsValid(saveDestination)) {
                view.DisplayError(language.GetString("destination_folder_not_found"));
                return;
            }
            view.DisplayOutput(language.GetString("enter_savejob_type"));
            view.DisplayOutput($"1. {language.GetString("select_savejob_full")}");
            view.DisplayOutput($"2. {language.GetString("select_savejob_differential")}");
            string saveType = view.GetInput();

            // Switch case for the save type
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
        
        // Method to update a save job
        private void UpdateSaveJob() {
            
            // Check if there are save jobs
            if (saveJobs.Count <= 0) {
                view.DisplayError(language.GetString("no_savejob"));
                return;
            }
            view.DisplayOutput(language.GetString("select_savejob_update") + " :");
            view.DisplaySaveJobList(saveJobs);
            
            // Get the user's choice
            try {
                int choice = int.Parse(view.GetInput());
                view.DisplayOutput(language.GetString("enter_savejob_name"));
                string saveName = view.GetInput();
                
                // Check if the name is valid
                if (saveName.Length < 1) {
                    view.DisplayError(language.GetString("invalid_input"));
                    return;
                }
                view.DisplayOutput(language.GetString("enter_savejob_source"));
                string saveSource = view.GetInput();
                
                // Check if the source folder exists
                if (!Directory.Exists(saveSource)) {
                    view.DisplayError(language.GetString("source_folder_not_found"));
                    return;
                }
                view.DisplayOutput(language.GetString("enter_savejob_destination"));
                string saveDestination = view.GetInput();
                
                // Check if the destination folder exists
                if (!tool.PathDirectoryIsValid(saveDestination)) {
                    view.DisplayError(language.GetString("destination_folder_not_found"));
                    return;
                }
                view.DisplayOutput(language.GetString("enter_savejob_type"));
                view.DisplayOutput($"1. {language.GetString("select_savejob_full")}");
                view.DisplayOutput($"2. {language.GetString("select_savejob_differential")}");
                string saveType = view.GetInput();

                // Switch case for the save type
                switch (saveType) {
                    case "1":
                        saveJobs[choice - 1] = new FullSave(saveName, saveSource, saveDestination);
                        tool.WriteSavedSaveJob(saveJobs);
                        break;
                    case "2":
                        saveJobs[choice - 1] = new DifferentialSave(saveName, saveSource, saveDestination);
                        tool.WriteSavedSaveJob(saveJobs);
                        break;
                    default:
                        view.DisplayError(language.GetString("no_savejob"));
                        break;
                }
                view.ClearConsole();
            }
            // Catch invalid input
            catch (Exception) {
                view.DisplayError(language.GetString("no_savejob"));
            }
        }

        // Method to delete a save job
        private void DeleteSaveJob() {

            // Check if there are save jobs
            if (saveJobs.Count <= 0) {
                view.DisplayError(language.GetString("no_savejob"));
                return;
            }
            view.DisplayOutput(language.GetString("select_savejob_delete") + " :");
            view.DisplaySaveJobList(saveJobs);
            
            // Get the user's choice
            try {
                int choice = int.Parse(view.GetInput());
                saveJobs.RemoveAt(choice - 1);
                tool.WriteSavedSaveJob(saveJobs);
                view.ClearConsole();
            }
            
            // Catch invalid input
            catch (Exception) {
                view.DisplayError(language.GetString("invalid_input"));
            }
        }

    }
}