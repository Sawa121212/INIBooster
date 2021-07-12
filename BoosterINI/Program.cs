using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using BoosterINI.Managers;

namespace BoosterINI
{
    public class Program
    {
        protected static string[] INIFiles;
        protected static string[] CIDFiles;
        protected static string[] Files;

        protected static bool SmpRateEdit;
        protected static int SmpRateValue;

        // // // 
        // Mod (режим записи)
        protected static List<List<string>> IEDName;
        protected static List<List<string>> Address;
        protected static List<List<string>> Ethernet;
        protected static List<List<string>> ControlModels;
        protected static List<List<string>> Datasets;
        protected static List<List<string>> GCBs;
        protected static List<List<string>> SVCBs;
        protected static List<List<string>> RCBs;
        protected static List<List<string>> GooseSubscribers;

        protected static string[] TRs = {"[TRs]", "value=8", "[TCTR0]", "scaleFactor=1000", "offset=0", "[TCTR1]", "scaleFactor=1000", "offset=0", "[TCTR2]", "scaleFactor=1000", "offset=0", "[TCTR3]", "scaleFactor=1000", "offset=0", "[TVTR0]", "scaleFactor=100", "offset=0", "[TVTR1]", "scaleFactor=100", "offset=0", "[TVTR2]", "scaleFactor=100", "offset=0", "[TVTR3]", "scaleFactor=100", "offset=0"};

        protected static string GlobalDirectoryName = "Project";
        protected static string ErrorMessage = "";

        static void Main(string[] args)
        {
            Console.WriteLine("version 1.6r (MAC, IP)");

            // Удаляем глобальную папку, если она уже есть
            string globalDirectory = GlobalDirectoryName;
            DirectoryInfo dirInfo = new DirectoryInfo(globalDirectory);
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
            }

            // Проверяем стартовые файлы
            var fileManager = new FileManager();
            if (fileManager.FileChecker())
            {
                try
                {
                    // Спрашиваем про SmpRate
                    var smpRate = new SmpRate();
                    smpRate.EditSmpRate();

                    // создаем папки по имени файла для устройств
                    Dir explorer = new Dir();
                    if (explorer.CreateDirectory(Files))
                    {
                        fileManager.CopyCidFiles(Files); // Копируем cid файлы наши каталоги
                        fileManager.CreateSettingsFiles(Files); // Создаем settings.ini
                        ConfigurationManager.CreateConfigurationFiles(Files); // создаем Config файлы из ini файлов
                    }

                    ConfigurationManager.CreateProjectConfig(Files); // создаем Главный Config файл
                }
                catch (Exception e)
                {
                    Message.ErrorMessage("Ошибка в работе");
                    Message.ErrorMessage(ErrorMessage);
                    Message.ErrorMessage(e.Message);
                }

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