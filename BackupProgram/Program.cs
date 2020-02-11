using System;
using System.IO;
using static System.Console;

namespace BackupProgram
{
    class Program
    {

        static void Main(string[] args)
        {
            Worker folders = new Worker();

            folders.Settings = new Settings();

            folders.DefaultSetup();

            Console.WriteLine("Первичная настройка завершена.");

            folders.PrintSettings();

            Console.WriteLine("Установление новых настроек...");

            string newPath1 = @"C:\Users\Dan\Desktop\Example\SourceFolder";
            string newPath2 = @"C:\Users\Dan\Desktop\Example2";

            folders.UpdateFoldersSettings(newPath1, newPath2);

            folders.Backup();

            ReadKey();
        }

        

        static void CheckFilesInFolder()
        {
            string dirName = Directory.GetCurrentDirectory();

            if (Directory.Exists(dirName))
            {
                Console.WriteLine("Подкаталоги:");
                string[] dirs = Directory.GetDirectories(dirName);
                foreach (string s in dirs)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
                Console.WriteLine("Файлы:");
                string[] files = Directory.GetFiles(dirName);
                foreach (string s in files)
                {
                    Console.WriteLine(s);
                }
            }
            else Console.WriteLine("Папки не существует");
        }


        //Настройка приложения при первом запуске программы. Создание папки WorkingFolders с подкаталогами DestinationFolder и SourceFolder. 
        static void InitialSetup()
        {
            string path = Directory.GetCurrentDirectory();
            string subpath0 = @"WorkingFolders";
            string workingFolders = Path.Combine(path, subpath0);

            DirectoryInfo workingFolderPath = new DirectoryInfo(workingFolders);
            if (!workingFolderPath.Exists)
            {
                workingFolderPath.Create();
            }

            string subpath1 = @"DestinationFolder";
            string subpath2 = @"SourceFolder";

            DirectoryInfo dirInfo = new DirectoryInfo(workingFolders);

            //Добавить проверку на существование папок перед их созданием
            dirInfo.CreateSubdirectory(subpath1);
            dirInfo.CreateSubdirectory(subpath2);
            
        }

        /*
        //Создание текстового файла в исходной папке для будущей реализации копирвания файлов из одной папки в другую
        static void SetupSourceFolder()
        {
            string path = $@"{sourceDirPath}\hello.txt";

            FileInfo fileInfo = new FileInfo(path);

            //Проверка существования hello.txt
            if (!fileInfo.Exists)
            {
                fileInfo.Create().Close();

                string text = "Привет, мир!";


                //Запись в файл hello.txt фразы "Hello, world"
                try
                {
                    using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }

                    Console.WriteLine("Запись выполнена");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
        static void CreateBackupFolder()
        {
            string path = $@"{destinationDirPath}";
            string newFolder = Path.Combine(path, (DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss")));


            DirectoryInfo newFolderPath = new DirectoryInfo(newFolder);
            if (!newFolderPath.Exists)
            {
                newFolderPath.Create();

                string pathToFile;

                DirectoryInfo sourceDir = new DirectoryInfo($@"{sourceDirPath}");
                DirectoryInfo destDir = new DirectoryInfo(newFolder);
                foreach (var item in sourceDir.GetFiles())
                {
                    pathToFile = Path.Combine(newFolder, item.Name + ".bak");
                    item.CopyTo(pathToFile, true);
                }
                Console.WriteLine("Резервная папка была создана");
            }
        }
        */
    }
}

