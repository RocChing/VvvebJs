using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace VvvebJs.Api
{
    public static class FileUtils
    {
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }

        public static void WriteAllText(string path, string content)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        public static string[] GetFiles(string path, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories)
        {
            return Directory.GetFiles(path, searchPattern, option);
        }

        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }
    }
}
