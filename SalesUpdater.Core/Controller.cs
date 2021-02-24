using SalesUpdater.DAL;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces;
using SalesUpdater.Interfaces.Core;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SalesUpdater.Core
{
    public class Controller : IController, IDisposable
    {
        private SalesContext _context;

        private ReaderWriterLockSlim _locker { get; }

        private IDirectoryHandler DirectoryHandler { get; }
        private IUnit Unit { get; }
        private IParser Parser { get; }
        private IFileHandler FileHandler { get; }

        public Controller(string directoryPath, string filesFilter)
        {
            _context = new SalesContext();

            _locker = new ReaderWriterLockSlim();

            DirectoryHandler = new DirectoryHandler(directoryPath, filesFilter);

            Unit = new Unit(_context, _locker);

            Parser = new Parser();

            FileHandler = new FileHandler(Unit, Parser, _locker);
        }

        public void Run()
        {
            DirectoryHandler.Run(FileHandler);
        }

        public void Stop()
        {
            DirectoryHandler.Stop(FileHandler);
        }

        public IEnumerable<SaleDTO> ShowAllSales()
        {
            return Unit.GetAll();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DirectoryHandler.Dispose();
                    _locker.Dispose();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
