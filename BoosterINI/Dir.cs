using System;
using System.IO;

namespace BoosterINI
{
    public class Dir : Program
    {
        public bool CreateDirectory(string[] FilesList)
        {
            try
            {
                foreach (string file in FilesList)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(file);
                    if (!dirInfo.Exists)
                    {
                        dirInfo.Create();
                    }
                }

                Message.ExcellentMessage("Папки созданы");
            }
            catch (Exception e)
            {
                Message.ErrorMessage("Ошибка при создании папок (CreateDirectory)");
                Message.ErrorMessage(e.Message.ToString());
                return false;
            }
            return true;
        }
    }
}