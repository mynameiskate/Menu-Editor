using System.ComponentModel;

namespace PluginInterfaces
{
    public interface IPlugin 
    {
        void Compress(string path);
        string Decompress(string path);
        string ArchiveType
        {
            get;
        }
    }
}
