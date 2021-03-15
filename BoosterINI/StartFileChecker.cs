using System;
using System.IO;

namespace BoosterINI
{
    public class Files : Program
    {
        public bool FileChecker()
        {
            Dir _dir = new Dir();
            try
            {
                CIDFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.cid");
                // WriteFileList(CIDFiles);

                INIFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.ini");
                //WriteFileList(INIFiles);

                // [cid][ini] - проверка наличия файлов
                int[,] fileIsFinded = CIDFiles.Length > INIFiles.Length ? new int[CIDFiles.Length, 2] : new int[INIFiles.Length, 2];

                // список файлов
                string[] fileNames = CIDFiles.Length > INIFiles.Length ? CIDFiles : INIFiles;
                Files = new String[fileNames.Length];

                int counter = 0;
                foreach (var file in fileNames)
                {
                    // удаление пути файла 
                    Uri uri = new Uri(file);
                    string fileName = Path.GetFileName(uri.LocalPath);

                    fileName = fileName.Replace(".cid", "");
                    fileName = fileName.Replace(".ini", "");

                    fileIsFinded[counter, 0] = File.Exists(fileName + ".cid") ? 1 : 0;
                    fileIsFinded[counter, 1] = File.Exists(fileName + ".ini") ? 1 : 0;
                    Files[counter++] = fileName;
                }

                Console.WriteLine("\t|{0,2}   |{1,15}   |{2, 15}|{3,15}|", " №", " IED", " cid file", "ini file");
                Console.WriteLine("\t|-----+------------------+---------------+---------------|");

                for (int i = 0; i < Files.Length; i++)
                {
                    Console.Write("\t|{0,2}   |{1,15}   |", i + 1, Files[i]);
                    for (int j = 0; j < 2; j++)
                    {
                        if (fileIsFinded[i, j] == 1)
                        {
                            Message.ExcellentMessageAdd(" available |");
                        }
                        else
                        {
                            Message.ErrorMessageAdd(" not available |");
                        }
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("\t'-----+------------------+---------------+---------------'");


                if (INIFiles.Length == CIDFiles.Length && INIFiles.Length != 0)
                {
                    Message.ExcellentMessage("Количество файлов ini и cid совпадают");
                }
                else
                {
                    Message.ErrorMessage("Количество файлов ini и cid не совпадают");
                    return false;
                }
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при чтении файлов");
                Message.ErrorMessage(e.Message);
            }

            // создаем папки по имени файла
            if (_dir.CreateDirectory(Files))
            {
                if (MoveFiles(Files))
                {
                }
                else { return false; }
            }
            else { return false; }

            return true;
        }

        /// <summary>
        /// Перемещаем cid файлы
        /// </summary>
        /// <param name="FilesList"></param>
        /// <returns></returns>
        public bool MoveFiles(string[] FilesList)
        {
            try
            {
                // перед перемещением создаем settings.ini
                if (EditFiles(FilesList))
                {
                    // перемещаем cid файлы 
                    foreach (string file in FilesList)
                    {
                        var filename = file + ".cid";
                        var dirName = file;
                        File.Move(filename, Environment.CurrentDirectory + "\\" + dirName + "\\" + filename);
                    }

                    Message.ExcellentMessage("Файлы cid перемещены");
                }
                else
                {
                    Message.ErrorMessage("Ошибка при редактировании ini файлов (EditFiles)");
                    return false;
                }
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при перемещении файлов (MoveFiles)");
                Message.ErrorMessage(e.Message.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Создаем settings.ini
        /// </summary>
        /// <param name="FilesList"></param>
        /// <returns></returns>
        public bool EditFiles(string[] FilesList)
        {
            try
            {
                string[] addressSettings = { "", "", "" };
                int addressSettingsCount = -1;

                foreach (string file in FilesList)
                {
                    // создаем временный файл, вставляем строки из ini файла
                    string tempFile = Path.GetTempFileName();
                    var dirName = file;

                    // читаем построчно
                    using (var sr = new StreamReader(file + ".ini"))
                    using (var sw = new StreamWriter(tempFile))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (addressSettingsCount >= 0)
                            {
                                addressSettings[addressSettingsCount] = line;
                                addressSettingsCount--;

                                if (addressSettingsCount < 0)
                                {
                                    sw.WriteLine(line);
                                    sw.WriteLine("\n[Ethernet1]");
                                    for (int i = 2; i >= 0; i--)
                                    {
                                        sw.WriteLine(addressSettings[i]);
                                    }
                                    continue;
                                }
                            }

                            if (line == "[Address]")
                            {
                                addressSettingsCount = 2;
                            }

                            if (line == "OSI-TSEL=0001" ||
                                line == "OSI-PSEL=00000001" ||
                                line == "OSI-SSEL=0001")
                            {
                                continue;
                            }

                            sw.WriteLine(line);
                        }
                    }

                    // перемещаем заполненный временный файл
                    File.Move(tempFile, Environment.CurrentDirectory + "\\" + dirName + "\\" + "settings.ini");

                    // создаем Config файлы из ini файлов

                }
                if (Conf.CreateConfig(FilesList)) { }
                else { return false; }

                Message.ExcellentMessage("Файлы ini созданы и переименованы в settings.ini");
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при создании settings.ini файлов (EditFiles)");
                Message.ErrorMessage(e.Message);
                return false;
            }
            return true;
        }
    }
}