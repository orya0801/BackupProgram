using System;
using System.Collections.Specialized;
using System.IO;

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
            SetPathsToSourceFolders(numberOfFolders);
            SetDestinationFolder();
            UpdateFoldersSettings();
            Console.WriteLine("Первичная настройка завершена.");
        }

        public void SetDestinationFolder()
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

        private void SetPathsToSourceFolders(int numberOfFolders)
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

        private int SetNumberOfSourceFolders()
        {
            Console.Write("Введите количество исходных папок: ");
            int numberOfFolders = 0;
            bool isInt = false;
            while (!isInt)
            {
                try
                {
                    numberOfFolders = int.Parse(Console.ReadLine());
                    if (numberOfFolders > 0)
                        isInt = true;
                    else Console.WriteLine("Количество папок должно представлять положительное число. Введите еще раз: ");
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

        public void UpdateDestinationFolder()
        {
            SetDestinationFolder();
            UpdateFoldersSettings();
        }

        //Установка новых путей до папок
        public void UpdateFoldersSettings()
        {
            folders = new Folders(new Settings());
        }

        public void AddSourceFolder()
        {
            Console.WriteLine("Введите путь до новой исходной папки:");
            string pathToSourceFolder = Console.ReadLine();
            bool isPathExists = false;
            while (!isPathExists)
            {
                if (Directory.Exists(pathToSourceFolder))
                {
                    isPathExists = true;
                }
                else
                {
                    Console.WriteLine("Папки не существует. Введите другой путь: ");
                    pathToSourceFolder = Console.ReadLine();
                }
            }
            Properties.Settings.Default.SourceFolders.Add(pathToSourceFolder);
            Properties.Settings.Default.Save();
            UpdateFoldersSettings();
            PrintSourceFolders();
        }
        
        public void DeleteSourceFolder()
        {
            PrintSourceFolders();
            Console.WriteLine("Введите номер папки, которую хотите удалить");
            int number = int.Parse(Console.ReadLine());
            folders.SourceFolderPaths.RemoveAt(number - 1);
            Properties.Settings.Default.SourceFolders = folders.SourceFolderPaths;
            Properties.Settings.Default.Save();
            UpdateFoldersSettings();
        }

        public void PrintSourceFolders()
        {
            int k = 1;
            foreach(string folder in folders.SourceFolderPaths)
            {
                Console.WriteLine($"{k}: {folder}");
                k += 1;
            }
        }

        public void PrintSettings()
        {
            Console.WriteLine("Текущие настройки: ");
            Console.WriteLine("Исходные папки: ");
            foreach (string folderPath in folders.SourceFolderPaths)
            {
                Console.WriteLine("\t" + folderPath);
            }
            Console.WriteLine("Целевая папка: ");
            Console.WriteLine("\t" + folders.DestinationFolderPath);
        }

        public bool isFirstStart()
        {
            bool checkFirstStart = true;
            if (Properties.Settings.Default.NumberOfStarts != 0 || Properties.Settings.Default.SourceFolders.Count != 0)
            {
                checkFirstStart = false;
            }
            return checkFirstStart;
        }

        public void Backup()
        {
            if (isFirstStart())
            {
                SetDefaultFolders();
            }
            PrintSettings();
            string path = $@"{folders.DestinationFolderPath}";
            string newFolderPath = Path.Combine(path, (DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss")));

            DirectoryInfo newFolder = new DirectoryInfo(newFolderPath);
            try
            {
                newFolder.Create();

                Console.WriteLine("Резервное копирование...");
                string pathToFile;
                foreach (string folder in folders.SourceFolderPaths)
                {
                    DirectoryInfo sourceDir = new DirectoryInfo($"{folder}");
                    if (sourceDir.Exists)
                    {
                        foreach (var item in sourceDir.GetFiles())
                        {
                            pathToFile = Path.Combine(newFolderPath, item.Name + ".bak");
                            FileInfo directory = new FileInfo(pathToFile);
                            if (!directory.Exists)
                            {
                                item.CopyTo(pathToFile, true);
                                Console.WriteLine($"Добавлен файл: {pathToFile}");
                            }
                            else Console.WriteLine($"Ошибка при копировании:\n\tФайл {pathToFile} уже существует. Возможно указано 2 или более одинаковых исходных директорий.");
                            
                        }
                    }
                    else Console.WriteLine("Не удалось найти целевую папку: " + folder);
                }
                Properties.Settings.Default.NumberOfStarts += 1;
                Properties.Settings.Default.Save();
                Console.WriteLine("Резервное копирование завершено\n");
            }
            catch (Exception e)
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
            UpdateFoldersSettings();
        }
    }
}

