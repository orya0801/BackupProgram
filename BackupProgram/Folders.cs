using System.Collections.Specialized;

namespace BackupProgram
{
    class Folders
    {
        StringCollection sourceFolderPaths;
        string destinationFolderPath;

        public Folders(Settings settings)
        {
            sourceFolderPaths = settings.SourceFolderPaths;
            destinationFolderPath = $"{settings.DestinationFolderPath}";
        }

        public StringCollection SourceFolderPaths
        {
            get
            {
                return sourceFolderPaths;
            }
            set
            {
                SourceFolderPaths = value;
            }
        }

        public string DestinationFolderPath
        {
            get
            {
                return destinationFolderPath;
            }
            set
            {
                DestinationFolderPath = value;
            }
        }

    }
}
