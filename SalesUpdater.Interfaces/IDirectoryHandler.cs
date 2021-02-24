using System;

namespace SalesUpdater.Interfaces
{
    public interface IDirectoryHandler : IDisposable
    {
        void Run(IFileHandler fileHandler);

        void Stop(IFileHandler fileHandler);
    }
}
