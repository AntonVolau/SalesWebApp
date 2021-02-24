using System.IO;

namespace SalesUpdater.Interfaces
{
    public interface IFileHandler
    {
        void ProcessFile(object source, FileSystemEventArgs e);
    }
}
