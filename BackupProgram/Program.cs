using System;

namespace BackupProgram
{
    class Program
    {

        static void Main(string[] args)
        {
            Worker folders = new Worker
            {
                Settings = new Settings()
            };

            folders.Backup();

            bool cont = true;

            while (cont)
            {
                Console.WriteLine("Выберите дальнейшее действие: " +
                "\n1. Повторить создание backup-файлов" +
                "\n2. Сменить целевую папку" +
                "\n3. Добавить исходную папку" +
                "\n4. Удалить исходную папку" +
                "\n5. Сбросить настройки" +
                "\nЧтобы выйти из программы нажмите Enter");

                string chosenFunction = "";
                chosenFunction = Console.ReadLine();

                switch (chosenFunction)
                {
                    case "1":
                        folders.Backup();
                        break;
                    case "2":
                        folders.UpdateDestinationFolder();
                        break;
                    case "3":
                        folders.AddSourceFolder();
                        break;
                    case "4":
                        folders.DeleteSourceFolder();
                        break;
                    case "5":
                        Console.WriteLine("Вы уверены, что хотите сбросить настройки?(yes/no)");
                        string answer = Console.ReadLine();
                        if (answer == "yes")
                        {
                            folders.ResetSettings();
                        }
                        else continue;
                        break;
                    default:
                        cont = false;
                        break;
                }       
            }
        }
    }
}

