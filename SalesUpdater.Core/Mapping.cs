using SalesUpdater.Interfaces;
using System.IO;

namespace SalesUpdater.Core
{
    internal static class Mapping
    {
        internal static void AddEvent(FileSystemWatcher fileWatcher, IFileHandler fileHandler)
        {
            fileWatcher.Created += fileHandler.ProcessFile;
        }

        internal static void RemoveEvent(FileSystemWatcher fileWatcher, IFileHandler fileHandler)
        {
            fileWatcher.Created -= fileHandler.ProcessFile;
        }
    }
}
