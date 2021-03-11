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
                Files = new String[FilesList.Length];
                int counter = 0;
                foreach (string file in FilesList)
                {
                    Uri uri = new Uri(file);
                    string filename = Path.GetFileName(uri.LocalPath);
                    Files[counter++] = filename;
                    filename = filename.Replace(".ini", "");

                    DirectoryInfo dirInfo = new DirectoryInfo(filename);
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