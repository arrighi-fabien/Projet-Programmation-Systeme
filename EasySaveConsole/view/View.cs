using EasySaveConsole.model;

namespace EasySaveConsole.view {
    // Class responsible for displaying information to the user
    public class View {

        private Language language = Language.GetInstance();
        
        // Display output to the console
        public void DisplayOutput(string output) {
            Console.WriteLine(output);
        }

        // Get input from the user
        public string GetInput() {
            return Console.ReadLine();
        }

        // Clear the console screen and display software name
        public void ClearConsole() {
            Console.Clear();
            DisplaySoftwareName();
            DisplayOutput("");
        }

        // Display a list of save jobs
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

        // Display the main menu options
        public void DisplayMainMenu() {
            DisplayOutput($"1. {language.GetString("menu_language")}");
            DisplayOutput($"2. {language.GetString("menu_execute_save")}");
            DisplayOutput($"3. {language.GetString("menu_create_save")}");
            DisplayOutput($"4. {language.GetString("menu_update_save")}");
            DisplayOutput($"5. {language.GetString("menu_delete_save")}");
            DisplayOutput($"6. {language.GetString("menu_quit_application")}");
        }

        // Display the software name
        public void DisplaySoftwareName() {
            DisplayOutput("███████╗░█████╗░░██████╗██╗░░░██╗░██████╗░█████╗░██╗░░░██╗███████╗");
            DisplayOutput("██╔════╝██╔══██╗██╔════╝╚██╗░██╔╝██╔════╝██╔══██╗██║░░░██║██╔════╝");
            DisplayOutput("█████╗░░███████║╚█████╗░░╚████╔╝░╚█████╗░███████║╚██╗░██╔╝█████╗░░");
            DisplayOutput("██╔══╝░░██╔══██║░╚═══██╗░░╚██╔╝░░░╚═══██╗██╔══██║░╚████╔╝░██╔══╝░░");
            DisplayOutput("███████╗██║░░██║██████╔╝░░░██║░░░██████╔╝██║░░██║░░╚██╔╝░░███████╗");
            DisplayOutput("╚══════╝╚═╝░░╚═╝╚═════╝░░░░╚═╝░░░╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░╚══════╝ by ProSoft");
        }

        // Display error message
        public void DisplayError(string error) {
            ClearConsole();
            DisplayOutput(error);
            DisplayOutput("");
        }

    }
}