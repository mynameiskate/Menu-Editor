using PluginInterfaces;
using System.IO;
using System.IO.Compression;

namespace ZipArchiver
{
    public class ZipArchiver : IPlugin
    {
        public string ArchiveType
        {
            get
            {
                return ".zip";
            }
        }

        public void Compress(string path)
        {
            using (FileStream fs = new FileStream(Path.GetFileNameWithoutExtension(path) + ArchiveType, FileMode.Create))
            {
                using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(@path, Path.GetFileName(path));
                }
            }
        }

        public string Decompress(string path)
        {
            string dest = string.Empty;
            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry file in archive.Entries)
                {
                    string destPath = Path.Combine(Path.GetDirectoryName(path), file.FullName);
                    if (File.Exists(destPath))
                    {
                        File.Delete(destPath);
                    }
                    file.ExtractToFile(destPath);
                    dest = destPath;
                }
            }
            return dest;
        } 
    }
}
