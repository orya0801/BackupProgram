using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram
{
    class Worker
    {
        Settings settings;
        Folders folders;

        #region Properties

        public Settings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
                folders = new Folders(settings);
            }
        }

        #endregion

        public void SetDefaultFolders()
        {
            Console.WriteLine("Это первый запуск программы на этом компьютере. Для начала работы требуется провести начальную настройку.");
            int numberOfFolders = SetNumberOfSourceFolders();
            setPathsToSourceFolders(numberOfFolders);
            SetDestinationFolder();
            UpdateFoldersSettings(Properties.Settings.Default.SourceFolders, Properties.Settings.Default.DestinationFolder);
            Console.WriteLine("Первичная настройка завершена.");
        }

        private void SetDestinationFolder()
        {
            Console.WriteLine("Введите путь до целевой папки");
            string pathToDestFolder = Console.ReadLine();
            bool isPathExists = false;
            while (!isPathExists)
            {
                if (Directory.Exists(pathToDestFolder))
                {
                    isPathExists = true;
                }
                else
                {
                    Console.WriteLine("Папки не существует. Введите другой путь: ");
                    pathToDestFolder = Console.ReadLine();
                }
            }
            Properties.Settings.Default.DestinationFolder = pathToDestFolder;
            Properties.Settings.Default.Save();
        }

        private  void setPathsToSourceFolders(int numberOfFolders)
        {
            Console.WriteLine("Введите пути до исходных папок");
            string pathToSourceFolder;
            StringCollection sourceFolders = new StringCollection();
            for (int i = 0; i < numberOfFolders; i++)
            {
                pathToSourceFolder = Console.ReadLine();
                sourceFolders.Add(pathToSourceFolder);
            }
            Properties.Settings.Default.SourceFolders = sourceFolders;
            Properties.Settings.Default.Save();
        }

        //Добавить проверку на отриц. знач.
        private int SetNumberOfSourceFolders()
        {
            Console.Write("Введите количество папок: ");
            int numberOfFolders = 0;
            bool isInt = false;
            while (!isInt)
            {
                try
                {
                    numberOfFolders = int.Parse(Console.ReadLine());
                    isInt = true;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Количество папок слишком велико. (Рекомендуется указать не более 10 папок для удобства использования)");
                }
                catch
                {
                    Console.WriteLine("Введите целое количество папок!");
                }
            }
            return numberOfFolders;
        }


        //Функция InitialSetup выполняет первичную настройку приложения
        public void DefaultSetup()
        {
            //Установка пути до исходной и целевой папок по умолчанию (создание папки WorkingFolders в файле приложения)
            string path = Directory.GetCurrentDirectory();
            string subpath = @"WorkingFolders";
            string workingFolders = Path.Combine(path, subpath);

            DirectoryInfo workingFolderPath = new DirectoryInfo(workingFolders);

            if (!workingFolderPath.Exists)
            {
                //Создание папки WorkingFolders в файле проекта
                workingFolderPath.Create();

                //Настройка дочерних директорий папки Working Folders
                string subpathDest = @"DestinationFolder";
                string subpathSource = @"SourceFolder";

                DirectoryInfo dirInfo = new DirectoryInfo(workingFolders);

                //Создание дочерних директорий папки Working Folders
                dirInfo.CreateSubdirectory(subpathDest);
                dirInfo.CreateSubdirectory(subpathSource);
                SetupSourceFolder();
            }
        }

        //Создание текстового файла в папке SourceFolder для демонстрации работы программы при дефолтных настройках
        private void SetupSourceFolder()
        {
            string path = $@"{folders.SourceFolderPaths}\hello.txt";

            FileInfo fileInfo = new FileInfo(path);

            //Проверка существования hello.txt
            if (!fileInfo.Exists)
            {
                fileInfo.Create().Close();

                string text = "Привет, мир!";

                //Заполнение текстового фалйа
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

        //Установка новых путей до папок
        public void UpdateFoldersSettings(StringCollection sourceFolder, string destFolder)
        {
            Properties.Settings.Default.SourceFolders = sourceFolder;
            Properties.Settings.Default.DestinationFolder = destFolder;
            Properties.Settings.Default.Save();

            folders = new Folders(new Settings());
        }

        public void PrintSettings()
        {
            Console.WriteLine("Текущие настройки: ");
            Console.WriteLine("Исходные папки: ");
            foreach(string folderPath in folders.SourceFolderPaths)
            {
                Console.WriteLine("\t" + folderPath);
            }
            Console.WriteLine("Целевая папка: ");
            Console.WriteLine("\t" + folders.DestinationFolderPath);
        }

        public void Backup()
        {
            string path = $@"{folders.DestinationFolderPath}";
            string newFolderPath = Path.Combine(path, (DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss")));

            DirectoryInfo newFolder = new DirectoryInfo(newFolderPath);
            try
            {
                newFolder.Create();

                Console.WriteLine("Резервное копирование...");
                string pathToFile;
                foreach(string folder in folders.SourceFolderPaths)
                {
                    DirectoryInfo sourceDir = new DirectoryInfo($"{folder}");
                    if (sourceDir.Exists)
                    {
                        foreach (var item in sourceDir.GetFiles())
                        {
                            pathToFile = Path.Combine(newFolderPath, item.Name + ".bak");
                            item.CopyTo(pathToFile, true);
                            Console.WriteLine($"Добавлен файл: {pathToFile}");
                        }
                    }
                    else Console.WriteLine("Не удалось найти целевую папку: " + folder) ;
                }
                Console.WriteLine("Резервное копирование завершено");
            }
            catch(Exception e)
            {
                return;
            }
        }

        public void ResetSettings()
        {
            Properties.Settings.Default.NumberOfStarts = 0;
            Properties.Settings.Default.DestinationFolder = "";
            Properties.Settings.Default.SourceFolders.Clear();
            Properties.Settings.Default.Save();

        }
    }
}
