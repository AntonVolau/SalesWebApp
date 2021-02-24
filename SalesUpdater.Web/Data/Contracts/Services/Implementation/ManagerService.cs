using SalesUpdater.DAL.ReaderWriter;
using SalesUpdater.DAL.ReaderWriter.Implementation;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;

namespace SalesUpdater.Web.Data.Contracts.Services.Implementation
{
    public class ManagerService : IManagerService, IDisposable
    {
        private SalesContext Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IManagerDbReaderWriter ManagerDbReaderWriter { get; }

        public ManagerService()
        {
            Context = new SalesContext();

            Locker = new ReaderWriterLockSlim();

            ManagerDbReaderWriter = new ManagerDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<ManagerDTO>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ManagerDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ManagerDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<ManagerDTO>> Filter(ManagerFilterCoreModel managerFilterCoreModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (managerFilterCoreModel.Surname == null)
            {
                return await GetUsingPagedListAsync(managerFilterCoreModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetUsingPagedListAsync(managerFilterCoreModel.Page ?? 1, pageSize,
                    x => x.Surname.Contains(managerFilterCoreModel.Surname))
                .ConfigureAwait(false);
        }

        public async Task<ManagerDTO> GetAsync(int id)
        {
            return await ManagerDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerDTO> AddAsync(ManagerDTO model)
        {
            return await ManagerDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ManagerDTO> UpdateAsync(ManagerDTO model)
        {
            return await ManagerDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ManagerDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ManagerDTO>> FindAsync(Expression<Func<ManagerDTO, bool>> predicate)
        {
            return await ManagerDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Locker.Dispose();
                    Context.Dispose();
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