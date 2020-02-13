using System.Collections.Specialized;

namespace BackupProgram
{
    class Settings
    {
        public StringCollection SourceFolderPaths { get; set; } = Properties.Settings.Default.SourceFolders;
        public string DestinationFolderPath { get; set; } = Properties.Settings.Default.DestinationFolder;
    }
}
