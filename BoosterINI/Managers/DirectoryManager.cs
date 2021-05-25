using System;
using System.IO;

namespace BoosterINI.Managers
{
    public class Dir : Program
    {
        /// <summary>
        /// Создаем каталоги по устройства
        /// </summary>
        /// <param name="FilesList"></param>
        /// <returns></returns>
        public bool CreateDirectory(string[] FilesList)
        {
            try
            {
                foreach (string file in FilesList)
                {
                    string filePosition = GlobalDirectoryName + "\\" + file;
                    DirectoryInfo dirInfo = new DirectoryInfo(filePosition);
                    dirInfo.Create();
                }

                Message.ExcellentMessage("Папки IED-ов созданы");
            }
            catch (Exception)
            {
                ErrorMessage = "Ошибка при создании папок (CreateDirectory)";
            }

            return true;
        }
    }
}