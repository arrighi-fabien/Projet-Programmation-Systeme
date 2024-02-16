using EasySaveConsole.model;
using EasySaveConsole.view;

namespace EasySaveConsole.controller {

    public class Controller {

        // Attributes for view, language, tool, saveJobs, and jobStates
        private readonly View view = new();
        private readonly Language language = Language.GetInstance();
        private readonly Tool tool = Tool.GetInstance();
        private List<SaveJob> saveJobs;
        private List<JobState> jobStates = [];

        /// <summary>
        /// Method to display the main menu
        /// </summary>
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
                        ChangeLogFormat();
                        break;
                    case "7":
                        break;
                    default:
                        view.DisplayError(language.GetString("invalid_input"));
                        break;
                }
                // Loop until the user chooses to quit
            } while (choice != "7");
        }

        /// <summary>
        /// Method to change the language
        /// </summary>
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

        /// <summary>
        /// Method to run a save job
        /// </summary>
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

        /// <summary>
        /// Method to create a save job
        /// </summary>
        private void CreateSaveJob() {
            view.DisplayOutput(language.GetString("create_savejob"));
            SaveJob? saveJob = CreateSaveJobInstance();

            if (saveJob != null) {
                saveJobs.Add(saveJob);
                tool.WriteSavedSaveJob(saveJobs);
                view.ClearConsole();
            }
        }

        /// <summary>
        /// Method to update a save job
        /// </summary>
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
                SaveJob? saveJob = CreateSaveJobInstance();
                if (saveJob != null) {
                    saveJobs[choice - 1] = saveJob;
                    tool.WriteSavedSaveJob(saveJobs);
                    view.ClearConsole();
                }
            }
            catch (Exception) {
                view.DisplayError(language.GetString("invalid_input"));
                return;
            }
        }

        /// <summary>
        /// Method to create a savejob instance
        /// </summary>
        /// <returns>SaveJob instance or null if inputs are invalid</returns>
        private SaveJob? CreateSaveJobInstance() {
            view.DisplayOutput(language.GetString("enter_savejob_name"));
            string saveName = view.GetInput();

            // Check if the name is valid
            if (saveName.Length < 1) {
                view.DisplayError(language.GetString("invalid_input"));
                return null;
            }
            view.DisplayOutput(language.GetString("enter_savejob_source"));
            string saveSource = view.GetInput();

            // Check if the source folder exists
            if (!Directory.Exists(saveSource)) {
                view.DisplayError(language.GetString("source_folder_not_found"));
                return null;
            }
            view.DisplayOutput(language.GetString("enter_savejob_destination"));
            string saveDestination = view.GetInput();

            // Check if the destination folder exists
            if (!tool.PathDirectoryIsValid(saveDestination)) {
                view.DisplayError(language.GetString("destination_folder_not_found"));
                return null;
            }
            view.DisplayOutput(language.GetString("enter_savejob_type"));
            view.DisplayOutput($"1. {language.GetString("select_savejob_full")}");
            view.DisplayOutput($"2. {language.GetString("select_savejob_differential")}");
            string saveType = view.GetInput();

            // Switch case for the save type
            SaveJob? saveJob = null;
            switch (saveType) {
                case "1":
                    saveJob = new FullSave(saveName, saveSource, saveDestination);
                    break;
                case "2":
                    saveJob = new DifferentialSave(saveName, saveSource, saveDestination);
                    break;
                default:
                    view.DisplayError(language.GetString("invalid_input"));
                    break;
            }
            return saveJob;
        }

        /// <summary>
        /// Method to delete a save job
        /// </summary>
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

        /// <summary>
        /// Method to change the log format
        /// </summary>
        private void ChangeLogFormat() {
            string actualValue = tool.GetConfigValue("logFormat");
            view.DisplayOutput(language.GetString("select_log_format") + " :");

            // Add (X) to the actual value
            view.DisplayOutput("1. JSON" + (actualValue == "json" ? " (X)" : ""));
            view.DisplayOutput("2. XML" + (actualValue == "xml" ? " (X)" : ""));

            string choice = view.GetInput();
            switch (choice) {
                case "1":
                    tool.WriteConfigValue("logFormat", "json");
                    view.ClearConsole();
                    break;
                case "2":
                    tool.WriteConfigValue("logFormat", "xml");
                    view.ClearConsole();
                    break;
                default:
                    view.DisplayError(language.GetString("invalid_input"));
                    break;
            }
        }

    }
}