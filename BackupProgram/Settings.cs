using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram
{
    class Settings
    {
        public string SourceFolderPath { get; set; } = Properties.Settings.Default.SourceFolder;
        public string DestinationFolderPath { get; set; } = Properties.Settings.Default.DestinationFolder;
    }
}
