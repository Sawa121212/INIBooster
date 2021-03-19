using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace BoosterINI
{
    public class Program
    {
        public static string[] INIFiles;
        public static string[] CIDFiles;
        public static string[] Files;

        // // // 
        // Mod (режим записи)
        public static List<List<string>> IEDName;
        public static List<List<string>> Address;
        public static List<List<string>> ControlModels;
        public static List<List<string>> Datasets;
        public static List<List<string>> GCBs;
        public static List<List<string>> SVCBs;
        public static List<List<string>> RCBs;
        public static List<List<string>> GooseSubscribers;

        public static string[] TRs = { "[TRs]", "value=8", "[TCTR0]", "scaleFactor=1000", "offset=0", "[TCTR1]", "scaleFactor=1000", "offset=0", "[TCTR2]", "scaleFactor=1000", "offset=0", "[TCTR3]", "scaleFactor=1000", "offset=0", "[TVTR0]", "scaleFactor=100", "offset=0", "[TVTR1]", "scaleFactor=100", "offset=0", "[TVTR2]", "scaleFactor=100", "offset=0", "[TVTR3]", "scaleFactor=100", "offset=0" };

        static void Main(string[] args)
        {
            Console.WriteLine("version 1.2r");
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
            Console.Write("Нажмите на любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}