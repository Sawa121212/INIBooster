using System;
using System.Collections.Generic;
using System.IO;

namespace BoosterINI.Managers
{
    public class FileManager : Program
    {
        /// <summary>
        /// Проверяем стартовые файлы на наличие
        /// </summary>
        /// <returns></returns>
        public bool FileChecker()
        {
            try
            {
                CIDFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.cid");
                INIFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.ini");

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

                Console.WriteLine("\t.-----+------------------+---------------+---------------.");
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
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Ошибка при стартовых чтении файлов";
            }

            return true;
        }

        /// <summary>
        /// Копируем cid файлы
        /// </summary>
        /// <param name="filesList"></param>
        /// <returns></returns>
        public void CopyCidFiles(string[] filesList)
        {
            try
            {
                foreach (string file in filesList)
                {
                    var filename = file + ".cid";
                    var dirName = file;
                    File.Copy(filename, Environment.CurrentDirectory + "\\" + GlobalDirectoryName + "\\" + dirName + "\\" + filename);
                }

                Message.ExcellentMessage("Файлы cid скопируем");
            }
            catch (Exception)
            {
                ErrorMessage = "Ошибка при копировании cid файлов (CopyFiles)";
            }
        }

        /// <summary>
        /// Создаем settings.ini
        /// </summary>
        /// <param name="filesList"></param>
        /// <returns></returns>
        public void CreateSettingsFiles(string[] filesList)
        {
            try
            {
                //инициализация
                IEDName = new List<List<string>>();
                Address = new List<List<string>>();
                Ethernet = new List<List<string>>();
                ControlModels = new List<List<string>>();
                Datasets = new List<List<string>>();
                GCBs = new List<List<string>>();
                SVCBs = new List<List<string>>();
                RCBs = new List<List<string>>();
                GooseSubscribers = new List<List<string>>();

                //добавление новой строки
                foreach (var file in filesList)
                {
                    IEDName.Add(new List<string>());
                    Address.Add(new List<string>());
                    Ethernet.Add(new List<string>());
                    ControlModels.Add(new List<string>());
                    Datasets.Add(new List<string>());
                    GCBs.Add(new List<string>());
                    SVCBs.Add(new List<string>());
                    RCBs.Add(new List<string>());
                    GooseSubscribers.Add(new List<string>());
                }

                int counter = 0;
                foreach (string file in filesList)
                {
                    // создаем временный файл, вставляем строки из ini файла
                    string tempFile = Path.GetTempFileName();
                    var dirName = file;

                    // читаем построчно
                    using (var row = new StreamReader(file + ".ini"))
                    {
                        string line;
                        while ((line = row.ReadLine()) != null)
                        {
                            if (line == "")
                            {
                                continue;
                            }

                            if (line == "[IEDNAME]")
                            {
                                IEDName[counter].Add(line);
                                IEDName[counter].Add(row.ReadLine());
                            }

                            if (line == "[Address]")
                            {
                                Address[counter].Add(line);
                                Address[counter].Add(row.ReadLine());
                                Address[counter].Add(row.ReadLine());
                                Address[counter].Add(row.ReadLine());
                            }

                            if (line == "[Ethernet1]")
                            {
                                Ethernet[counter].Add(line);
                                Ethernet[counter].Add(row.ReadLine());
                                Ethernet[counter].Add(row.ReadLine());
                                Ethernet[counter].Add(row.ReadLine());
                            }

                            if (line == "[ControlModels]")
                            {
                                ControlModels[counter].Add(line);
                                ControlModels[counter].Add(row.ReadLine());
                                ControlModels[counter].Add(row.ReadLine());
                                ControlModels[counter].Add(row.ReadLine());
                            }

                            if (line == "[DataSetCount]")
                            {
                                Datasets[counter].Add(line);
                                while ((line = row.ReadLine()) != "[GCBcount]")
                                {
                                    Datasets[counter].Add(line);
                                }
                            }

                            if (line == "[GCBcount]")
                            {
                                GCBs[counter].Add(line);
                                while ((line = row.ReadLine()) != "[RCBcount]")
                                {
                                    GCBs[counter].Add(line);
                                }
                            }

                            if (line == "[RCBcount]")
                            {
                                RCBs[counter].Add(line);
                                while ((line = row.ReadLine()) != "[GooseSubscriberCount]")
                                {
                                    RCBs[counter].Add(line);
                                }
                            }

                            if (line == "[GooseSubscriberCount]")
                            {
                                GooseSubscribers[counter].Add(line);
                                while ((line = row.ReadLine()) != "[SVCBcount]")
                                {
                                    GooseSubscribers[counter].Add(line);
                                }
                            }

                            if (line == "[SVCBcount]")
                            {
                                SVCBs[counter].Add(line);
                                while ((line = row.ReadLine()) != null)
                                {
                                    SVCBs[counter].Add(line);
                                }
                            }
                        }
                    }

                    using (var sw = new StreamWriter(tempFile))
                    {
                        foreach (var str in IEDName[counter])
                        {
                            sw.WriteLine(str);
                        }

                        sw.Write("\n");

                        // Address
                        foreach (var str in Address[counter])
                        {
                            sw.WriteLine(str);
                        }

                        sw.Write("\n");

                        // Ethernet
                        foreach (var str in Ethernet[counter])
                        {
                            sw.WriteLine(str);
                        }

                        sw.Write("\n");

                        foreach (var str in ControlModels[counter])
                        {
                            sw.WriteLine(str);
                        }

                        sw.Write("\n");

                        foreach (var str in Datasets[counter])
                        {
                            sw.WriteLine(str);
                        }

                        foreach (var str in GCBs[counter])
                        {
                            sw.WriteLine(str);
                        }

                        // TRs
                        foreach (var str in TRs)
                        {
                            sw.WriteLine(str);
                        }

                        sw.Write("\n");

                        foreach (var str in SVCBs[counter])
                        {
                            sw.WriteLine(str);
                        }

                        foreach (var str in RCBs[counter])
                        {
                            sw.WriteLine(str);
                        }

                        foreach (var str in GooseSubscribers[counter])
                        {
                            sw.WriteLine(str);
                        }
                    }

                    counter++;

                    // перемещаем заполненный временный файл
                    File.Move(tempFile, Environment.CurrentDirectory + "\\" + GlobalDirectoryName + "\\" + dirName + "\\" + "settings.ini");
                }

                Message.ExcellentMessage("Файлы ini созданы и переименованы в settings.ini");
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при создании settings.ini файлов (EditFiles)");
                Message.ErrorMessage(e.Message);
            }
        }
    }
}