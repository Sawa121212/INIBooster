using System;
using System.Collections.Generic;
using System.IO;

namespace BoosterINI
{
    public class Conf : Program
    {
        public static void CreateLastConfig(string[] FilesList)
        {
            try
            {
                string filenameConf = "Project.conf";

                StreamWriter output = new StreamWriter(filenameConf);

                output.WriteLine("[Project]\nName=Project\n");

                int counter = 0;
                foreach (string file in FilesList)
                {
                    var dirName = file.Replace(".ini", "");
                    output.WriteLine("[Device" + counter++ + "]");
                    output.WriteLine("Name =" + dirName + "\n");
                }

                output.WriteLine("[Count]\nvalue=" + FilesList.Length);

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
        /// Создаем Config файлы из ini файлов
        /// </summary>
        /// <param name="FilesList"></param>
        public static bool CreateConfig(string[] FilesList)
        {
            string filenameConf = "configuration.conf";
            List<string> RCB = new List<string>();
            bool RCBSaveMode = false;
            bool DatasetSaveMode = false;

            try
            {
                foreach (string file in FilesList)
                {
                    var dirName = file.Replace(".ini", "");
                    StreamWriter output = new StreamWriter(dirName + "\\" + filenameConf);

                    output.WriteLine("[Count]\nvalue=0\n");
                    int rowAddCount = 0;
                    bool addRowMode = false;
                    using (var sr = new StreamReader(file))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (rowAddCount > 0)
                            {
                                output.WriteLine(line);
                                rowAddCount--;
                                continue;
                            }

                            if (line == "[Ethernet1]")
                            {
                                output.WriteLine("[Address]");
                                rowAddCount = 3;
                                continue;
                            }

                            if (line == "[ControlModels]")
                            {
                                output.WriteLine("\n" + line);
                                rowAddCount = 3;
                                continue;
                            }

                            if (line == "[DataSetCount]")
                            {
                                output.WriteLine("\n" + line);
                                DatasetSaveMode = true;
                                addRowMode = true;
                                continue;
                            }


                            if (line == "[GCBcount]")
                            {
                                output.WriteLine(line);
                                DatasetSaveMode = false;
                                rowAddCount = 1;
                                continue;
                            }

                            if (line == "[RCBcount]")
                            {
                                output.WriteLine("[TRs]\nvalue = 8\n");
                                output.WriteLine("[TCTR0]\nscaleFactor = 1000\noffset = 0\n");
                                output.WriteLine("[TCTR1]\nscaleFactor = 1000\noffset = 0\n");
                                output.WriteLine("[TCTR2]\nscaleFactor = 1000\noffset = 0\n");
                                output.WriteLine("[TCTR3]\nscaleFactor = 1000\noffset = 0\n");
                                output.WriteLine("[TVTR0]\nscaleFactor = 100\noffset = 0\n");
                                output.WriteLine("[TVTR1]\nscaleFactor = 100\noffset = 0\n");
                                output.WriteLine("[TVTR2]\nscaleFactor = 100\noffset = 0\n");
                                output.WriteLine("[TVTR3]\nscaleFactor = 100\noffset = 0\n");

                                RCBSaveMode = true;
                                addRowMode = false;
                                continue;
                            }

                            if (line == "[SVCBcount]")
                            {
                                addRowMode = true;

                                output.WriteLine(line);
                                continue;
                            }

                            if (DatasetSaveMode)
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
                                            //Data34="daName=stVal doName=Ind72 fc=ST ldInst=CTRL lnClass=GGIO lnInst=1 prefix=Out"
                                            //Data9=CTRL/OutGGIO1$ST$Ind8$stVal
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
                                        }
                                        else { output.WriteLine(line); }
                                    }
                                    else { output.WriteLine(line); }
                                }
                                else { output.WriteLine(line); }
                                continue;
                            }

                            if (RCBSaveMode)
                            {
                                if (line != "[GooseSubscriberCount]")
                                {
                                    RCB.Add(line);
                                }
                                else
                                {
                                    addRowMode = false;
                                    RCBSaveMode = false;
                                }

                                continue;
                            }

                            // если режим добавления строк
                            if (addRowMode)
                            {
                                output.WriteLine(line);
                            }

                        }

                        output.WriteLine("[RCBcount]");
                        foreach (var rcbRow in RCB)
                        {
                            output.WriteLine(rcbRow);
                        }
                    }

                    output.Close();
                }

                Message.ExcellentMessage("Config файлы созданы");
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при создании Config файлов (CreateConfig)");
                Message.ErrorMessage(e.Message);
                return false;
            }
            return true;
        }

        public static void ClearINIFiles(string[] FilesList)
        {
            try
            {
                foreach (string file in FilesList)
                {
                    File.Delete(file);
                }
                Message.ExcellentMessage("Config файлы созданы");
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при удалении ini файлов (ClearINIFiles)");
                Message.ErrorMessage(e.Message);
            }
        }
    }
}
