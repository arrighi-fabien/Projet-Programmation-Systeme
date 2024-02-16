using EasySaveConsole.model;

namespace EasySaveConsole.view {
    // Class responsible for displaying information to the user
    public class View {

        private Language language = Language.GetInstance();

        /// <summary>
        /// Display the output to the user
        /// </summary>
        /// <param name="output">The output to display.</param>
        public void DisplayOutput(string output) {
            Console.WriteLine(output);
        }

        /// <summary>
        /// Get the input from the user
        /// </summary>
        /// <returns>The input from the user.</returns>
        public string GetInput() {
            return Console.ReadLine();
        }

        /// <summary>
        /// Clear the console screen and display software name
        /// </summary>
        public void ClearConsole() {
            Console.Clear();
            DisplaySoftwareName();
            DisplayOutput("");
        }

        /// <summary>
        /// Display the list of savejobs
        /// </summary>
        /// <param name="saveJobs">The list of savejobs to display.</param>
        public void DisplaySaveJobList(List<SaveJob> saveJobs) {
            int i = 1;
            foreach (SaveJob saveJob in saveJobs) {
                DisplayOutput($"{i}. {saveJob.Name}");
                DisplayOutput($"   {language.GetString("source_folder")}: {saveJob.SourceFolder}");
                DisplayOutput($"   {language.GetString("destination_folder")}: {saveJob.DestinationFolder}");
                DisplayOutput($"   {language.GetString("save_type")}: {saveJob.GetType().Name}");
                DisplayOutput("");
                i++;
            }
        }

        /// <summary>
        /// Display the main menu options
        /// </summary>
        public void DisplayMainMenu() {
            DisplayOutput($"1. {language.GetString("menu_language")}");
            DisplayOutput($"2. {language.GetString("menu_execute_save")}");
            DisplayOutput($"3. {language.GetString("menu_create_save")}");
            DisplayOutput($"4. {language.GetString("menu_update_save")}");
            DisplayOutput($"5. {language.GetString("menu_delete_save")}");
            DisplayOutput($"6. {language.GetString("menu_log_format")}");
            DisplayOutput($"7. {language.GetString("menu_quit_application")}");
        }

        /// <summary>
        /// Display the software name
        /// </summary>
        public void DisplaySoftwareName() {
            DisplayOutput("███████╗░█████╗░░██████╗██╗░░░██╗░██████╗░█████╗░██╗░░░██╗███████╗");
            DisplayOutput("██╔════╝██╔══██╗██╔════╝╚██╗░██╔╝██╔════╝██╔══██╗██║░░░██║██╔════╝");
            DisplayOutput("█████╗░░███████║╚█████╗░░╚████╔╝░╚█████╗░███████║╚██╗░██╔╝█████╗░░");
            DisplayOutput("██╔══╝░░██╔══██║░╚═══██╗░░╚██╔╝░░░╚═══██╗██╔══██║░╚████╔╝░██╔══╝░░");
            DisplayOutput("███████╗██║░░██║██████╔╝░░░██║░░░██████╔╝██║░░██║░░╚██╔╝░░███████╗");
            DisplayOutput("╚══════╝╚═╝░░╚═╝╚═════╝░░░░╚═╝░░░╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░╚══════╝ by ProSoft");
        }

        /// <summary>
        /// Display error message
        /// </summary>
        /// <param name="error">The error message to display.</param>
        public void DisplayError(string error) {
            ClearConsole();
            DisplayOutput(error);
            DisplayOutput("");
        }

    }
}