using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BoosterINI.Managers
{
    public class ConfigurationManager : Program
    {
        /// <summary>
        /// Создать главный конфигурационный файл проекта
        /// </summary>
        /// <param name="filesList"></param>
        public static void CreateProjectConfig(string[] filesList)
        {
            try
            {
                string filenameConf = GlobalDirectoryName + "\\" + "Project.conf";

                StreamWriter output = new StreamWriter(filenameConf);

                output.WriteLine("[Project]\nName=Project\n");

                int counter = 0;
                foreach (string fileName in filesList)
                {
                    output.WriteLine("[Device" + counter++ + "]");
                    output.WriteLine("Name =" + fileName + "\n");
                }

                output.WriteLine("[Count]\nvalue=" + filesList.Length);

                output.Close();
                Message.ExcellentMessage("Главный Config файл создан");
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при создании главного Config файла");
                Message.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// Создаем Сonfiguration файлы из ini файлов
        /// </summary>
        /// <param name="filesList"></param>
        public static void CreateConfigurationFiles(string[] filesList)
        {
            string filenameConf = "configuration.conf";

            try
            {
                var newGooseSubscribersList = new List<List<string>>();

                int counter = 0;
                foreach (string file in filesList)
                {
                    var dirName = file;
                    StreamWriter output = new StreamWriter(GlobalDirectoryName + "\\" + dirName + "\\" + filenameConf);

                    newGooseSubscribersList.Add(new List<string>());

                    int gooseSubscribersCount = 0;
                    int skipLineCount = 0;

                    foreach (var str in GooseSubscribers[counter])
                    {
                        if (skipLineCount > 0)
                        {
                            skipLineCount--;
                            continue;
                        }

                        if (str.Contains("[GooseSubscriberCount]"))
                        {
                            skipLineCount++;
                            continue;
                        }

                        if (str.Contains("[ExtIn"))
                        {
                            gooseSubscribersCount++;

                            newGooseSubscribersList[counter].Add(str);
                            continue;
                        }

                        if (str.Contains("GoCBRef="))
                        {
                            string[] rowParts = str.Split('=');
                            if (rowParts.Length == 2)
                            {
                                if (rowParts[1] == String.Empty)
                                {
                                    // удалить последний элемент
                                    for (int i = 0; i < 4; i++)
                                    {
                                        newGooseSubscribersList[counter].Remove(newGooseSubscribersList[counter].Last());
                                    }

                                    gooseSubscribersCount--;
                                    break;
                                }
                            }

                            newGooseSubscribersList[counter].Add(str);
                            continue;
                        }

                        newGooseSubscribersList[counter].Add(str);
                    }

                    // GooseSubscribers
                    output.WriteLine("[Count]\nvalue=" + gooseSubscribersCount);
                    foreach (var str in newGooseSubscribersList[counter])
                    {
                        output.WriteLine(str);
                    }

                    output.Write("\n");

                    // Address
                    foreach (var str in Address[counter])
                    {
                        output.WriteLine(str);
                    }

                    output.Write("\n");

                    // Ethernet
                    foreach (var str in Ethernet[counter])
                    {
                        output.WriteLine(str);
                    }

                    output.Write("\n");

                    foreach (var str in ControlModels[counter])
                    {
                        output.WriteLine(str);
                    }

                    output.Write("\n");

                    foreach (var line in Datasets[counter])
                    {
                        string goodRow = "";
                        string[] rowParts = line.Split('=');

                        if (rowParts.Length > 1)
                        {
                            if (rowParts[0].Length > 4)
                            {
                                string dataNumber = rowParts[0].Remove(0, 4);
                                string data = rowParts[0].Replace(dataNumber, "");

                                if (data == "Data")
                                {
                                    rowParts[1] = rowParts[1].Replace(".", "$");
                                    string[] paramParts = rowParts[1].Split('/');
                                    string[] dataParts = paramParts[1].Split('$');

                                    string prefix = "";
                                    string lnClass;
                                    if (dataParts[0].Length > 5)
                                    {
                                        prefix = dataParts[0].Remove(dataParts[0].Length - 5);
                                        lnClass = dataParts[0].Replace(prefix, "");
                                    }
                                    else
                                    {
                                        lnClass = dataParts[0];
                                    }

                                    string goodlnClass = lnClass.Remove(lnClass.Length - 1);
                                    string lnInst = lnClass.Replace(goodlnClass, "");

                                    string daName = dataParts.Length > 3 ? dataParts[3] : "";
                                    daName = dataParts.Length > 4 ? dataParts[3] + "$" + dataParts[4] : daName;
                                    goodRow = rowParts[0] + "=\"" +
                                        "daName=" + daName +
                                        " doName=" + dataParts[2] +
                                        " fc=" + dataParts[1] +
                                        " ldInst=" + paramParts[0] +
                                        " lnClass=" + goodlnClass +
                                        " lnInst=" + lnInst +
                                        " prefix=" + prefix + "\"";
                                    output.WriteLine(goodRow);
                                    output.WriteLine("Comment" + dataNumber + "=");

                                    continue;
                                }
                            }
                        }

                        output.WriteLine(line);
                    }

                    foreach (var str in GCBs[counter])
                    {
                        output.WriteLine(str);
                    }

                    // TRs
                    foreach (var str in TRs)
                    {
                        output.WriteLine(str);
                    }

                    output.Write("\n");


                    // SVCBs
                    if (!SmpRateEdit)
                    {
                        foreach (var str in SVCBs[counter])
                        {
                            output.WriteLine(str);
                        }
                    }
                    else
                    {
                        foreach (var str in SVCBs[counter])
                        {
                            if (str.Contains("smpRate="))
                            {
                                output.WriteLine("smpRate=" + SmpRateValue);
                                Message.ExcellentMessage("Значения smpRate поменялись у " + file + " на " + SmpRateValue);
                            }
                            else
                            {
                                output.WriteLine(str);
                            }
                        }
                    }

                    // RCBs
                    foreach (var str in RCBs[counter])
                    {
                        output.WriteLine(str);
                    }

                    counter++;
                    output.Close();
                }

                Message.ExcellentMessage("Config файлы созданы");
            }
            catch (Exception)
            {
                ErrorMessage = "Ошибка при создании Config файлов (CreateConfig)";
            }
        }
    }
}