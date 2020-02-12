using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
