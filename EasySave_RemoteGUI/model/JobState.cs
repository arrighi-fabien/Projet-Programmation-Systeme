using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave_RemoteGUI.model {
    public class JobState {
        public string State {
            get; set;
        }
        public int TotalFiles {
            get; set;
        }
        public long TotalFilesSize {
            get; set;
        }
        public int FilesLeft {
            get; set;
        }
        public long FilesSizeLeft {
            get; set;
        }
        public int Progression {
            get; set;
        }
        public string Name {
            get; set;
        }
        public string SourceFile {
            get; set;
        }
        public string DestinationFile {
            get; set;
        }
    }
}
