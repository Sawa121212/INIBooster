using System;

namespace BoosterINI
{
    public class Program
    {
        public static string[] INIFiles;
        public static string[] CIDFiles;
        public static string[] Files;

        static void Main(string[] args)
        {
            var d = new Files();
            if (d.FileChecker())
            {
                Conf.ClearINIFiles(Files);

                // создаем Главный Config файл
                Conf.CreateLastConfig(Files);

                Message.ExcellentMessage("=================");
                Message.ExcellentMessage("=== ВЫПОЛНЕНО ===");
                Message.ExcellentMessage("=================");
            }
            else
            {
                Message.ErrorMessage("Ошибка в работе");
            }
            Console.WriteLine("version 1.1");
            Console.ReadKey();
        }
    }
}
