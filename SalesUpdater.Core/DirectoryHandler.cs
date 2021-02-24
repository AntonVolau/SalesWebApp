using SalesUpdater.Interfaces;
using System;
using System.IO;

namespace SalesUpdater.Core
{
    public class DirectoryHandler : IDirectoryHandler
    {
        private FileSystemWatcher FileWatcher { get; }

        public DirectoryHandler(string directoryPath, string filesFilter)
        {
            try
            {
                FileWatcher = new FileSystemWatcher
                {
                    Path = directoryPath,
                    Filter = filesFilter,

                    NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName
                };
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Check directory path in app.config" + e.Message);
            }
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    FileWatcher.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run(IFileHandler fileHandler)
        {
            try
            {
                Mapping.AddEvent(FileWatcher, fileHandler);

                FileWatcher.EnableRaisingEvents = true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Check directory path in app.config" + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Check app.config" + e.Message);
            }
        }

        public void Stop(IFileHandler fileHandler)
        {
            Mapping.RemoveEvent(FileWatcher, fileHandler);

            FileWatcher.EnableRaisingEvents = false;
        }
    }
}
