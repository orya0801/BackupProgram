using System;
using System.Collections.Generic;
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
            string path = $@"{folders.SourceFolderPath}\hello.txt";

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
        public void UpdateFoldersSettings(string sourceFolder, string destFolder)
        {
            Properties.Settings.Default.SourceFolder = sourceFolder;
            Properties.Settings.Default.DestinationFolder = destFolder;
            Properties.Settings.Default.Save();

            folders = new Folders(new Settings());
        }

        public void PrintSettings()
        {
            Console.WriteLine("Текущие настройки: ");
            Console.WriteLine(folders.SourceFolderPath);
            Console.WriteLine(folders.DestinationFolderPath);
        }

        public void Backup()
        {
            string path = $@"{folders.DestinationFolderPath}";
            string newFolder = Path.Combine(path, (DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss")));


            DirectoryInfo newFolderPath = new DirectoryInfo(newFolder);
            try
            {
                newFolderPath.Create();

                string pathToFile;

                DirectoryInfo sourceDir = new DirectoryInfo($@"{folders.SourceFolderPath}");
                DirectoryInfo destDir = new DirectoryInfo(newFolder);
                foreach (var item in sourceDir.GetFiles())
                {
                    pathToFile = Path.Combine(newFolder, item.Name + ".bak");
                    item.CopyTo(pathToFile, true);
                    Console.WriteLine($"Добавлен файл: {pathToFile}");
                }
                Console.WriteLine("Резервное копирование завершено");
            }
            catch(Exception e)
            {
                return;
            }
        }
    }
}
