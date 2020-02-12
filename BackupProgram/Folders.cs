using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram
{
    class Folders
    {
        string sourceFolderPath;
        string destinationFolderPath;

        public Folders(Settings settings)
        {
            sourceFolderPath = $"{settings.SourceFolderPath}";
            destinationFolderPath = $"{settings.DestinationFolderPath}";
        }

        public string SourceFolderPath
        {
            get
            {
                return sourceFolderPath;
            }
            set
            {
                SourceFolderPath = value;
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
