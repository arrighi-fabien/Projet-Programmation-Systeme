using EasySaveConsole.model;

namespace EasySaveConsole.view {
    public class View {

        private Language language = Language.GetInstance();

        public void DisplayOutput(string output) {
            Console.WriteLine(output);
        }

        public string GetInput() {
            return Console.ReadLine();
        }

        public void ClearConsole() {
            Console.Clear();
            DisplaySoftwareName();
            DisplayOutput("");
        }

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

        public void DisplayMainMenu() {
            DisplayOutput($"1. {language.GetString("menu_language")}");
            DisplayOutput($"2. {language.GetString("menu_execute_save")}");
            DisplayOutput($"3. {language.GetString("menu_create_save")}");
            DisplayOutput($"4. {language.GetString("menu_update_save")}");
            DisplayOutput($"5. {language.GetString("menu_delete_save")}");
            DisplayOutput($"6. {language.GetString("menu_quit_application")}");
        }

        public void DisplaySoftwareName() {
            DisplayOutput("███████╗░█████╗░░██████╗██╗░░░██╗░██████╗░█████╗░██╗░░░██╗███████╗");
            DisplayOutput("██╔════╝██╔══██╗██╔════╝╚██╗░██╔╝██╔════╝██╔══██╗██║░░░██║██╔════╝");
            DisplayOutput("█████╗░░███████║╚█████╗░░╚████╔╝░╚█████╗░███████║╚██╗░██╔╝█████╗░░");
            DisplayOutput("██╔══╝░░██╔══██║░╚═══██╗░░╚██╔╝░░░╚═══██╗██╔══██║░╚████╔╝░██╔══╝░░");
            DisplayOutput("███████╗██║░░██║██████╔╝░░░██║░░░██████╔╝██║░░██║░░╚██╔╝░░███████╗");
            DisplayOutput("╚══════╝╚═╝░░╚═╝╚═════╝░░░░╚═╝░░░╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░╚══════╝ by ProSoft");
        }

    }
}
