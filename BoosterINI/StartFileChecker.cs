using System;
using System.IO;

namespace BoosterINI
{
    public class Files : Program
    {
        Dir _dir = new Dir();
        public bool FileChecker()
        {
            try
            {
                INIFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.ini*");
                Console.WriteLine("INI");
                WriteFileList(INIFiles);

                Console.WriteLine("\nCID");
                CIDFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.cid*");
                WriteFileList(CIDFiles);

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
            if (_dir.CreateDirectory(INIFiles))
            {
                if (MoveFiles(Files))
                {
                }
                else { return false; }
            }
            else { return false; }

            return true;
        }

        private static void WriteFileList(string[] FilesList)
        {
            int counter = 1;
            foreach (string filename in FilesList)
            {
                Console.WriteLine(counter + ". " + filename);
                counter++;
            }
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
                        var filename = file.Replace(".ini", ".cid");
                        var dirName = file.Replace(".ini", "");
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
                    var dirName = file.Replace(".ini", "");

                    // читаем построчно
                    using (var sr = new StreamReader(file))
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