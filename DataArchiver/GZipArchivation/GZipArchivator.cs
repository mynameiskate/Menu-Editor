using PluginInterfaces;
using System.IO;
using System.IO.Compression;

namespace GZipArchivation
{
    public class GZipArchivator : IPlugin
    { 
        public string ArchiveType
        {
            get
            {
                return ".gz";
            }
        }

        public void Compress(string path)
        {
            using (FileStream file = File.OpenRead(path))
            {
                if ((File.GetAttributes(path) & FileAttributes.Hidden)
                    != FileAttributes.Hidden)
                {
                    using (FileStream dest = File.Create(path + ArchiveType))
                    {
                        using (GZipStream gzStream = new GZipStream(dest, CompressionMode.Compress))
                        {
                            file.CopyTo(gzStream);
                        }
                    }
                }
            }
        }

        public string Decompress(string path)
        {
            string origName = path.Remove(path.Length - ArchiveType.Length);
            using (FileStream file = File.OpenRead(path))
            { 
                using (FileStream resFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(file, CompressionMode.Decompress))
                    {
                        Decompress.CopyTo(resFile);
                        
                    }
                }
            }
            return origName;
        }
    }
}
