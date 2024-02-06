namespace EasySaveConsole.view {
    public class View {

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
