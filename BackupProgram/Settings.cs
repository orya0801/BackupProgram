using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram
{
    class Settings
    {
        public StringCollection SourceFolderPaths { get; set; } = Properties.Settings.Default.SourceFolders;
        public string DestinationFolderPath { get; set; } = Properties.Settings.Default.DestinationFolder;
    }
}
